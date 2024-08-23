namespace Slaycard.Api.Features.Combats.Domain;

public interface IBattleRepository
{
    Task<bool> Add(Battle battle);
    Task<Battle> Get(BattleId id);
    Task<Battle[]> GetMany(int Offset = 0, int Limit = 25);
}
