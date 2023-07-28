using Core.Extensions;
using Core.Network;
using Core.Network.Packets.C2S;
using FluentValidation;

namespace Core.Validation;

public class SignupC2SPacketValidator : AbstractValidator<SignupC2SPacket>
{
    public SignupC2SPacketValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithError(ErrorCodes.UsernameEmpty)
            .Matches("^[A-Za-z0-9]*$")
            .WithError(ErrorCodes.InvalidUsername)
            .MinimumLength(minimumLength: 3)
            .WithError(ErrorCodes.UsernameTooShort)
            .MaximumLength(maximumLength: 256)
            .WithError(ErrorCodes.UsernameTooLong);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithError(ErrorCodes.EmailEmpty)
            .EmailAddress()
            .WithError(ErrorCodes.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(ErrorCodes.PasswordEmpty)
            .MinimumLength(minimumLength: 8)
            .WithError(ErrorCodes.PasswordTooShort)
            .MaximumLength(maximumLength: 4096)
            .WithError(ErrorCodes.PasswordTooLong);
        /*.Matches(@".*")*/ // WithError InvalidPassword
    }
}