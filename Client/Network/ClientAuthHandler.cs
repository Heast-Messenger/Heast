using System.Threading.Tasks;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.S2C;

namespace Client.Network;

public class ClientAuthHandler : IClientAuthListener
{
    public ClientAuthHandler(ClientConnection ctx)
    {
        TaskCompletionSource = new TaskCompletionSource();
        Ctx = ctx;
    }

    private ClientConnection Ctx { get; }
    public TaskCompletionSource TaskCompletionSource { get; }

    public void OnSignupResponse(SignupS2CPacket packet)
    {
        // ignored
    }

    public void OnVerifyResponse(VerifyEmailS2CPacket packet)
    {
        // ignored
    }

    public void OnLoginResponse(LoginS2CPacket packet)
    {
        // ignored
    }

    public void OnAccountRequest(AccountRequestS2CPacket packet)
    {
        // ignored
    }
}