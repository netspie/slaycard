namespace Slaycard.Api.Features.Combats.Domain;

public interface IBattleRepository
{
    Task Add(Battle battle);
    Task<Battle> Get(BattleId id);
}
