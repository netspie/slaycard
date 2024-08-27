using FluentValidation;
using FluentValidation.Validators;

namespace Slaycard.Api.Features.Combats.UseCases.Common;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, TProperty> MustBeGuid<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new GuidValidator<T, TProperty>());
    }

    private class GuidValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "GuidValidator";

        public override bool IsValid(ValidationContext<T> context, TProperty value) =>
            value is string str ?
                Guid.TryParse(str, out _) : false;

        protected override string GetDefaultMessageTemplate(string errorCode) =>
            "The id must be a valid guid.";
    }
}
