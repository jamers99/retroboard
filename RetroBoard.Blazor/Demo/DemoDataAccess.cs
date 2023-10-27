namespace RetroBoard.Blazor.Demo;

public class DemoDataAccess : IDataAccess
{
    public Task<Board> LoadBoardAsync(string team) => Task.FromResult(GetDemoBoard());

    public Board GetDemoBoard()
    {
        var random = new Random();
        var workItemTypes = new[] {CardType.Bug, CardType.PBI};
        return new Board
        {
            Columns = Enumerable.Range(1, 3).Select(ci => new Column
            {
                Name = $"Column {ci}",
                Cards = Enumerable.Range(1, 10).Select(wi => new Card
                {
                    Id = wi,
                    Title = $"Demo card {wi} with more of a description",
                    Type = workItemTypes[random.Next(workItemTypes.Length)]
                }).ToList()
            }).ToList()
        };
    }
}
