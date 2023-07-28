namespace Client.ViewModel;

public interface IEmailVerification
{
    void VerifySignupCode(string code);
}