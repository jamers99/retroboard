namespace RetroBoard;

public class Card
{
    public int? Id { get; init; }
    public string? Title { get; init; }
    public CardType Type { get; init; }
    public bool IsBlocked { get; init; }
}