namespace App.BuildingBlocks.Infrastructure.Emails;

public class EmailConfiguration
{
    public string AppUrl { get; set; }
    
    public string FromName { get; set; }
    
    public string FromEmail { get; set; }
    
    public string Host { get; set; }
    
    public int Port { get; set; }
    
    public bool UseSsl { get; set; }
}
