using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.CombatsBots.Domain;

public interface IBotRepository
{
    Task<Bot> Get(PlayerId id);
    Task Add(Bot bot);
    Task Update(Bot bot);
    Task Delete(PlayerId id);
}
