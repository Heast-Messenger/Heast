using Core.Network.Packets.C2S;
using FluentValidation;

namespace Core.Validation;

public class SignupC2SPacketValidator : AbstractValidator<SignupC2SPacket>
{
    public SignupC2SPacketValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).MinimumLength(8).MaximumLength(4096) /*.Matches(@".*")*/;
    }
}