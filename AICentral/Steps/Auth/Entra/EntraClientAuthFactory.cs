﻿using Microsoft.Identity.Web;

namespace AICentral.Steps.Auth.Entra;

public class EntraClientAuthFactory : IAICentralClientAuthFactory
{
    private readonly IConfigurationSection _configSection;
    private readonly string _id;
    private readonly Lazy<EntraClientAuthProvider> _provider;

    public EntraClientAuthFactory(IConfigurationSection configSection)
    {
        _configSection = configSection;
        _id = Guid.NewGuid().ToString();
        _provider = new Lazy<EntraClientAuthProvider>(() => new EntraClientAuthProvider());
    }

    /// <summary>
    /// Add an AAD provider for this particular config section.
    /// </summary>
    public void RegisterServices(IServiceCollection services)
    {
        services.AddAuthentication().AddMicrosoftIdentityWebApi(_configSection, "Properties", _id);
        services.AddAuthorizationBuilder().AddPolicy(_id, policyBuilder => policyBuilder.RequireAuthenticatedUser().AddAuthenticationSchemes(_id));
    }

    public static string ConfigName => "Entra";

    public IAICentralClientAuthStep Build()
    {
        return _provider.Value;
    }

    public static IAICentralClientAuthFactory BuildFromConfig(
        ILogger logger, 
        IConfigurationSection configurationSection)
    {
        return new EntraClientAuthFactory(configurationSection);
    }
    
    /// <summary>
    /// Entra Auth is provided at the route scope. The runtime step is a no-op.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="route"></param>
    public void ConfigureRoute(WebApplication app, IEndpointConventionBuilder route)
    {
        route.RequireAuthorization(_id);
    }

    public object WriteDebug()
    {
        return new
        {
            Type = "Entra"
        };
    }
}