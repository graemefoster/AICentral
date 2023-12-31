﻿using AICentral.Core;

namespace AICentral.Steps.Auth;

public interface IAICentralClientAuthFactory : IAICentralPipelineStepFactory<IAICentralClientAuthStep>
{
    static virtual IAICentralClientAuthFactory BuildFromConfig(ILogger logger, IConfigurationSection configurationSection)
    {
        throw new NotImplementedException();
    }

    object WriteDebug();

    void ConfigureRoute(WebApplication app, IEndpointConventionBuilder route);
}