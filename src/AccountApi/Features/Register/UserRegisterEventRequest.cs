namespace AccountApi.Features.Register;

public record UserRegisterEventRequest(string username, string password, string fullname);