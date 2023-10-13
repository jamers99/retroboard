namespace RetroBoard.Azure;

public class AzureDataAccess : IDataAccess
{
    AzureSettings settings;
    public AzureDataAccess(AzureSettings azureSettings)
    {
        settings = azureSettings;
    }
 
    public string Name => settings.Url;
}
