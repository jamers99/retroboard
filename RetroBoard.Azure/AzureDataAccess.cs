namespace RetroBoard.Azure;

public class AzureDataAccess : IDataAccess
{
    AzureSettings _azureSettings;
    public AzureDataAccess(AzureSettings azureSettings)
    {
        _azureSettings = azureSettings;
    }
}
