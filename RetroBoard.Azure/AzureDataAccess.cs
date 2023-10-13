using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace RetroBoard.Azure;

public class AzureDataAccess : IDataAccess
{
    #region Fields

    public const string DescriptionField = "System.Title";
    public const string StateField = "System.State";
    public const string TitleField = "System.Title";
    public const string WorkItemTypeField = "System.WorkItemType";
    public const string BlockedField = "Microsoft.VSTS.CMMI.Blocked";

    object Get(WorkItem workItem, string field)
    {
        if (workItem.Fields.TryGetValue(field, out var value))
            return value;

        return "";
    }

    #endregion

    TeamHttpClient teamClient;
    TeamHttpClient TeamClient
    {
        get
        {
            if (teamClient == null)
            {
                var vssConnection = new VssConnection(new Uri(settings.Url), new VssBasicCredential("pat", settings.Pat));
                teamClient = vssConnection.GetClient<TeamHttpClient>();
            }

            return teamClient;
        }
    }

    WorkItemTrackingHttpClient workItemClient;
    WorkItemTrackingHttpClient WorkItemClient
    {
        get
        {
            if (workItemClient == null)
            {
                var vssConnection = new VssConnection(new Uri(settings.Url), new VssBasicCredential("pat", settings.Pat));
                workItemClient = vssConnection.GetClient<WorkItemTrackingHttpClient>();
            }

            return workItemClient;
        }
    }

    string project;
    AzureSettings settings;
    public AzureDataAccess(AzureSettings azureSettings)
    {
        settings = azureSettings;
        project = settings.Project;
    }

    public string Name => settings.Url;

    public async Task<Board> LoadBoardAsync(string teamName)
    {
        var teams = await TeamClient.GetTeamsAsync(project);
        var team = teams.FirstOrDefault(t => t.Name == teamName) ?? throw new ArgumentException("Team was not found", nameof(teamName));

        var wiql = new Wiql()
        {
            Query = $"select [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State], [System.Tags] " +
                        $"from WorkItems where [System.TeamProject] = '{project}' " +
                        $"and [System.AreaPath] = @teamAreas({getTeamForMacro()}) " +
                        $"and [System.WorkItemType] in ('Product Backlog Item', 'Bug') " +
                        $"and [System.IterationPath] = @currentIteration({getTeamForMacro()}) " +
                        $"order by [System.State]",
        };

        string getTeamForMacro() => $"'[{project}]\\{team.Name} <id:{team.Id}>'";

        var query = await WorkItemClient.QueryByWiqlAsync(wiql, project);
        var ids = query.WorkItems.Select(x => x.Id).ToList();
        var workItems = await WorkItemClient.GetWorkItemsAsync(project, ids, expand: WorkItemExpand.Relations);

        var board = new Board()
        {
            Columns = new List<Column>()
            {
                new()
                {
                    Name = "New",
                    Cards = GetCardsForStates(workItems, "New", "Pending design meeting")
                },
                new()
                {
                    Name = "Approved",
                    Cards = GetCardsForStates(workItems, "Approved")
                },
                new()
                {
                    Name = "In Progress",
                    Cards = GetCardsForStates(workItems, "Committed")
                },
                new()
                {
                    Name = "Waiting for CR",
                    Cards = GetCardsForStates(workItems, "Code review needed")
                },
                new()
                {
                    Name = "Waiting for Validation",
                    Cards = GetCardsForStates(workItems, "Validation needed")
                },
                new()
                {
                    Name = "Done",
                    Cards = GetCardsForStates(workItems, "Done", "No Change")
                }
            }
        };
        return board;
    }

    List<Card> GetCardsForStates(List<WorkItem> workItems, params string[] states)
    {
        return workItems.Where(wi => states.Contains((string)Get(wi, StateField)))
            .Select(wi => new Card()
            {
                Title = (string)wi.Fields.GetValueOrDefault(TitleField)!,
                Id = wi.Id,
                IsBlocked = (string)Get(wi, BlockedField) == "Yes",
                Type = (string)Get(wi, WorkItemTypeField) == "Bug" ? CardType.Bug : CardType.PBI,
            }).ToList();
    }
}
