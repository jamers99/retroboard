namespace RetroBoard;

public class Card
{
    public int? Id { get; init; }
    public string? Title { get; init; }
    public CardType Type { get; init; }
    public bool IsBlocked { get; init; }
    public string Color => Type switch
    {
        CardType.Bug => "rgba(120, 30, 20, 0.5)",
        CardType.PBI => "rgba(30, 30, 120, 0.5)",
        _ => "rgba(200, 200, 200, 0.5)"
    };
}