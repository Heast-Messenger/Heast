using Auth.Model;
using Auth.Services;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Network;

public class ServerAuthHandler : IServerAuthListener
{
    public ServerAuthHandler(ClientConnection ctx, AuthDbContext db, IServiceProvider serviceProvider)
    {
        TaskCompletionSource = new TaskCompletionSource();
        Ctx = ctx;
        Db = db;
        ServiceProvider = serviceProvider;
    }

    private ClientConnection Ctx { get; }
    private AuthDbContext Db { get; }
    private IServiceProvider ServiceProvider { get; }
    public TaskCompletionSource TaskCompletionSource { get; }

    public void OnSignup(SignupC2SPacket packet)
    {
        var validator = ServiceProvider.GetRequiredService<IValidator<SignupC2SPacket>>();
        var result = validator.Validate(packet);
        if (result.IsValid)
        {
        }

        Db.Accounts.Add(new Account
        {
            Name = packet.Username,
            Email = packet.Email,
            Password = packet.Password, // TODO: HASH
            AddedServers = new List<Server>()
        });

        Db.SaveChanges();
    }

    public void OnLogin(LoginC2SPacket packet)
    {
        throw new NotImplementedException();
    }

    public void OnReset(ResetC2SPacket packet)
    {
        throw new NotImplementedException();
    }

    public void OnDelete(DeleteC2SPacket packet)
    {
        throw new NotImplementedException();
    }

    public void OnLogout(LogoutC2SPacket packet)
    {
        throw new NotImplementedException();
    }

    public void OnVerify(VerifyC2SPacket packet)
    {
        throw new NotImplementedException();
    }

    public void OnGuest(GuestC2SPacket packet)
    {
        throw new NotImplementedException();
    }
}