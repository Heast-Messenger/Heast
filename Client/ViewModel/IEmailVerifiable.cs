namespace Client.ViewModel;

public interface IEmailVerifiable
{
    void VerifySignupCode(string code);
}