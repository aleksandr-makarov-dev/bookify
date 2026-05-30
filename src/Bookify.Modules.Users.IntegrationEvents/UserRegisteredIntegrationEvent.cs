namespace Bookify.Modules.Users.IntegrationEvents;

public sealed record UserRegisteredIntegrationEvent(string Email, string EmailConfirmationToken);