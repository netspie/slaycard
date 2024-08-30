using Core.Domain;
using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Api.Features.CombatsBots.Domain;

public class Bot(
    PlayerId id, 
    BattleId battleId) : Entity<PlayerId>(id)
{
    public BattleId BattleId { get; } = battleId;
}
