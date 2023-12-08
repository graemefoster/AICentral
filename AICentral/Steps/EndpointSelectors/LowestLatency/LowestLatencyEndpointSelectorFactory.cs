﻿using AICentral.Core;
using AICentral.Steps.Endpoints;

namespace AICentral.Steps.EndpointSelectors.LowestLatency;

public class LowestLatencyEndpointSelectorFactory : IAICentralEndpointSelectorFactory
{
    private readonly IAICentralEndpointDispatcherFactory[] _openAiServers;
    private readonly Lazy<LowestLatencyIaiCentralEndpointSelector> _endpointSelector;

    public LowestLatencyEndpointSelectorFactory(IAICentralEndpointDispatcherFactory[] openAiServers)
    {
        _openAiServers = openAiServers.ToArray();
        _endpointSelector = new Lazy<LowestLatencyIaiCentralEndpointSelector>(() => new LowestLatencyIaiCentralEndpointSelector(
            _openAiServers.Select(x => x.Build()).ToArray()));
    }

    public IAICentralEndpointSelector Build()
    {
        return _endpointSelector.Value;
    }

    public void RegisterServices(IServiceCollection services)
    {
    }

    public static string ConfigName => "LowestLatency";

    public static IAICentralEndpointSelectorFactory BuildFromConfig(
        ILogger logger, 
        IConfigurationSection configurationSection,
        Dictionary<string, IAICentralEndpointDispatcherFactory> endpoints
        )
    {
        var properties = configurationSection.GetSection("Properties").Get<LowestLatencyEndpointConfig>();
        Guard.NotNull(properties, configurationSection, "Properties");

        return new LowestLatencyEndpointSelectorFactory(
            Guard.NotNull(properties!.Endpoints, configurationSection, "Endpoints")
                .Select(x => endpoints.TryGetValue(x, out var ep) ? ep : throw new ArgumentException($"Cannot find Endpoint {x} in built endpoints"))
                .ToArray());
    }
    
    
    public object WriteDebug()
    {
        return new
        {
            Type = "Lowest Latency Router",
            Endpoints = _openAiServers.Select(x => x.WriteDebug())
        };
    }
}