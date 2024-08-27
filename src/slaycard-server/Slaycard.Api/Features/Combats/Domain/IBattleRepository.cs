namespace Slaycard.Api.Features.Combats.Domain;

public interface IBattleRepository
{
    Task Add(Battle battle);
    Task Update(Battle battle);
    Task Delete(BattleId id);
    Task<Battle> Get(BattleId id);
    Task<Battle[]> GetMany(int Offset = 0, int Limit = 25);
}
