namespace RetroBoard;

public interface IDataAccess
{
    Task<Board> LoadBoardAsync(string team);
}
