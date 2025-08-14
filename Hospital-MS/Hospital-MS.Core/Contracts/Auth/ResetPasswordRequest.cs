namespace Hospital_MS.Core.Contracts.Auth;
public record ResetPasswordRequest(
     string Email,
     string Code,
     string NewPassword
);
