namespace RetroBoard.Azure;

public class AzureSettings
{
    public AzureSettings(string url, string pat)
    {
        AzureUrl = url;
        AzurePat = pat;
    }

    public string AzureUrl { get; }
    public string AzurePat { get; }
}