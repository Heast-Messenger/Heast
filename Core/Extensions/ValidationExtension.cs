using Core.Network;
using FluentValidation;

namespace Core.Extensions;

public static class ValidationExtension
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, ErrorCodes errorCode)
    {
        var code = (ulong)errorCode;
        return rule.WithErrorCode(code.ToString());
    }
}