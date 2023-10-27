namespace RetroBoard;

public class Card
{
    public int? Id { get; init; }
    public string? Title { get; init; }
    public CardType Type { get; init; }
    public bool IsBlocked { get; init; }
    public string Color => Type switch
    {
        CardType.Bug => "rgb(204, 41, 61)",
        CardType.PBI => "rgb(0, 156, 204)",
        _ => "rgb(200, 200, 200)"
    };
}