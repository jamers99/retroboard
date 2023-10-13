namespace RetroBoard;

public class Board
{
    public string Team { get; init; }
    public List<Column> Columns { get; set; } = new();
}
