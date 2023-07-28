namespace Auth.Model;

public class Confirmation
{
    public int Attempts = 0;
    public required string Code { get; init; }
    public required TaskCompletionSource Tcs { get; init; }

    public void Confirmed()
    {
        Tcs.SetResult();
    }

    public void Failed()
    {
        Tcs.SetCanceled();
    }
}