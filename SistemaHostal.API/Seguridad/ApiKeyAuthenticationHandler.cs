using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace SistemaHostal.API.Seguridad;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IOptions<ApiKeySettings> apiKeySettings) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "ApiKey";
    private const string HeaderName = "X-Api-Key";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderName, out var apiKeyRecibida))
            return Task.FromResult(AuthenticateResult.Fail($"Falta el header {HeaderName}."));

        var apiKeyConfigurada = apiKeySettings.Value.Key;

        if (string.IsNullOrEmpty(apiKeyConfigurada) || apiKeyRecibida != apiKeyConfigurada)
            return Task.FromResult(AuthenticateResult.Fail("API key inválida."));

        var claims = new[] { new Claim(ClaimTypes.Name, "n8n") };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}