namespace AccountApi.Features;

public record ActivateUserRequest(string Username, String ActivationKey, String Password, String PasswordConfirm, String Email);