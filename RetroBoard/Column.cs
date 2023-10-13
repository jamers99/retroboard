namespace RetroBoard;

public class Column
{
    public string Name { get; init; }
    public List<WorkItem> WorkItems { get; } = new();
}
