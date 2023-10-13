using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace RetroBoard.Azure;

public class AzureDataAccess : IDataAccess
{
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

    AzureSettings settings;
    public AzureDataAccess(AzureSettings azureSettings)
    {
        settings = azureSettings;
    }

    public string Name => settings.Url;

    public Task<Board> LoadBoardAsync(string team)
    {
        return Task.FromResult(new Board() {Team = "Blah"}); //todo
    }
}
