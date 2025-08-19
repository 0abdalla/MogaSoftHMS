namespace Hospital_MS.Core.Contracts.Auth;
public record ResetPasswordRequest(
     string UserName,
     string Code,
     string NewPassword
);
