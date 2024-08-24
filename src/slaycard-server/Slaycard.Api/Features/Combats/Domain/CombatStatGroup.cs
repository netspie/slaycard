namespace Slaycard.Api.Features.Combats.Domain;

public record CombatStatGroup(
    Stat HP,
    Stat Damage,
    Stat Defence,
    Stat Accuracy,
    Stat Dodge,
    Stat Critics,
    Stat Speed);
