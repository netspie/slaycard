using Core.Domain;
using Mediator;
using Slaycard.Api.Core.Domain;
using Slaycard.Api.Features.Combats.Domain;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.CombatsBots.Domain;

public class Bot(
    PlayerId id, 
    BattleId battleId,
    Player[] players) : Entity<PlayerId>(id)
{
    private readonly BattleId _battleId = battleId;
    private readonly Player[] _players = players;

    public readonly List<ICommand> _commands = new();
    public IEnumerable<ICommand> Commands => _commands;

    public void HandleEvent(PlayerStartedBattleEvent ev)
    {
        //_commands.Add(new )
    }

    public void HandleEvent(DamagedEvent ev)
    {
        var originPlayer = _players.GetOfId(ev.OriginPlayerId);
        if (originPlayer is null)
            throw new DomainException("no_player_of_given_id");

        var targetPlayer = _players.GetOfId(ev.TargetPlayerId);
        if (targetPlayer is null)
            throw new DomainException("no_player_of_given_id");

        var weakestUnit = originPlayer.GetWeakestUnit();
        if (weakestUnit is null)
            throw new DomainException("no_units_to_attack");

        AddEvents(
            _players.ApplyArtifact(
                _battleId,
                ev.TargetPlayerId,
                ev.OriginUnitId,
                originPlayer.Id,
                weakestUnit.Id,
                new ArtifactId("attack")));
    }
}
