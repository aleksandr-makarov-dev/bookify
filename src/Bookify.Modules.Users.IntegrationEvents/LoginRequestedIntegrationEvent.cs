namespace Bookify.Modules.Users.IntegrationEvents;

public record LoginRequestedIntegrationEvent(string Email, string LoginToken);