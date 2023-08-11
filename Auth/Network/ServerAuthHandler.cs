using Auth.Model;
using Auth.Services;
using Core.Extensions;
using Core.Network;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;
using Core.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Network;

public class ServerAuthHandler : IServerAuthListener
{
    private readonly Dictionary<string, Confirmation> _awaitingConfirmation = new();

    public ServerAuthHandler(
        ClientConnection ctx,
        AuthDbContext db,
        IServiceProvider serviceProvider,
        IHashingService hashingService,
        ITwoFactorService twoFactorService,
        IEmailService emailService,
        ITokenService tokenService)
    {
        TaskCompletionSource = new TaskCompletionSource();
        Ctx = ctx;
        Db = db;
        ServiceProvider = serviceProvider;
        HashingService = hashingService;
        TwoFactorService = twoFactorService;
        EmailService = emailService;
        TokenService = tokenService;
    }

    private ClientConnection Ctx { get; }
    private AuthDbContext Db { get; }
    private IServiceProvider ServiceProvider { get; }
    private IHashingService HashingService { get; }
    private ITokenService TokenService { get; }
    private ITwoFactorService TwoFactorService { get; }
    private IEmailService EmailService { get; }

    public TaskCompletionSource TaskCompletionSource { get; }

    public async void OnSignup(SignupC2SPacket packet)
    {
        var validator = ServiceProvider.GetRequiredService<IValidator<SignupC2SPacket>>();
        var result = await validator.ValidateAsync(packet);
        if (!result.IsValid)
        {
            var errors = result.Errors.MapFlags(failure => (ErrorCodes)ulong.Parse(failure.ErrorCode));
            await Ctx.Send(new SignupS2CPacket(SignupS2CPacket.ResponseStatus.Error), errors, packet.Guid);
            return;
        }

        if (Db.Accounts.Any(x => x.Name == packet.Username))
        {
            await Ctx.Send(new SignupS2CPacket(SignupS2CPacket.ResponseStatus.Error), ErrorCodes.UsernameExists, packet.Guid);
            return;
        }

        if (Db.Accounts.Any(x => x.Email == packet.Email))
        {
            await Ctx.Send(new SignupS2CPacket(SignupS2CPacket.ResponseStatus.Error), ErrorCodes.EmailExists, packet.Guid);
            return;
        }

        await Ctx.Send(new SignupS2CPacket(SignupS2CPacket.ResponseStatus.AwaitingConfirmation), guid: packet.Guid);

        if (!await VerifyByEmail(packet.Email, packet.Username))
        {
            return;
        }

        var hash = HashingService.Hash(
            packet.Password,
            out var salt);

        Db.Accounts.Add(new Account
        {
            Name = packet.Username,
            Email = packet.Email,
            Hash = hash,
            Salt = salt,
            AddedServers = new List<Server>()
        });

        await Db.SaveChangesAsync();
    }

    public async void OnLogin(LoginC2SPacket packet)
    {
        var validator = ServiceProvider.GetRequiredService<IValidator<LoginC2SPacket>>();
        var result = await validator.ValidateAsync(packet);
        if (!result.IsValid)
        {
            var errors = result.Errors.MapFlags(failure => (ErrorCodes)ulong.Parse(failure.ErrorCode));
            await Ctx.Send(new LoginS2CPacket(LoginS2CPacket.ResponseStatus.Error), errors, packet.Guid);
            return;
        }

        var account = Db.Accounts.FirstOrDefault(x => x.Email == packet.Email);
        if (account is null)
        {
            await Ctx.Send(new LoginS2CPacket(LoginS2CPacket.ResponseStatus.UserNotKnown), guid: packet.Guid);
            return;
        }

        var passwordCorrect = HashingService.Verify(
            packet.Password,
            account.Hash,
            account.Salt);

        if (!passwordCorrect)
        {
            await Ctx.Send(new LoginS2CPacket(LoginS2CPacket.ResponseStatus.PasswordWrong), guid: packet.Guid);
            return;
        }

        await Ctx.Send(new LoginS2CPacket(LoginS2CPacket.ResponseStatus.AwaitingConfirmation), guid: packet.Guid);

        if (!await VerifyByEmail(packet.Email, packet.Email))
        {
        }
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

    public void OnVerifyEmail(VerifyEmailC2SPacket packet)
    {
        if (_awaitingConfirmation.TryGetValue(packet.Email, out var confirmation))
        {
            if (confirmation.Code == packet.Code)
            {
                Ctx.Send(new VerifyEmailS2CPacket(VerifyEmailS2CPacket.ResponseStatus.Success), guid: packet.Guid);
                confirmation.Confirmed();
                return;
            }

            if (++confirmation.Attempts > 3)
            {
                Ctx.Send(new VerifyEmailS2CPacket(VerifyEmailS2CPacket.ResponseStatus.Unauthorized), guid: packet.Guid);
                confirmation.Failed();
                return;
            }

            Ctx.Send(new VerifyEmailS2CPacket(VerifyEmailS2CPacket.ResponseStatus.WrongCode), guid: packet.Guid);
        }
    }

    public void OnGuest(GuestC2SPacket packet)
    {
        throw new NotImplementedException();
    }

    public void OnAccountRequest(AccountRequestC2SPacket packet)
    {
    }

    private Task<bool> VerifyByEmail(string email, string username)
    {
        Console.Out.WriteLine($"Sending code to {email}");
        var verificationCode = TwoFactorService.GetVerificationCode();
        var blockLink = EmailService.GenerateBlockLink(email);
        EmailService.SendSignupCode(email, username, verificationCode, blockLink);
        var tcs = new TaskCompletionSource();

        _awaitingConfirmation.Remove(email);
        _awaitingConfirmation.Add(email, new Confirmation { Tcs = tcs, Code = verificationCode });

        var cts = new CancellationTokenSource();
        var timeout = Task.Delay(TimeSpan.FromMinutes(value: 5), cts.Token);
        return Task.WhenAny(tcs.Task, timeout).Then(task =>
        {
            _awaitingConfirmation.Remove(email);
            if (task == timeout)
            {
                return false;
            }

            cts.Cancel();
            return task.IsCompletedSuccessfully;
        });
    }
}