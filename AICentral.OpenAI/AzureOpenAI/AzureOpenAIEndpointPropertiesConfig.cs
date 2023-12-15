﻿namespace AICentral.OpenAI.AzureOpenAI;

public class AzureOpenAIEndpointPropertiesConfig
{
    public string? LanguageEndpoint { get; init; }
    public Dictionary<string, string>? ModelMappings { get; init; }
    public string? AuthenticationType { get; init; }
    public string? ApiKey { get; set; }
    public int? MaxConcurrency { get; set; }
}