using System;

namespace API.Helpers;

public class CloudinarySettings
{
    public required string CloudName { get; set; } 
    public required string ApiKey { get; set; } 
    public required string ApiSecret { get; set; } 

    public CloudinarySettings() { }

    public CloudinarySettings(string cloudName, string apiKey, string apiSecret)
    {
        CloudName = cloudName;
        ApiKey = apiKey;
        ApiSecret = apiSecret;
    }

}
