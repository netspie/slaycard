using Core.Domain;
using Slaycard.Api.Core.Domain;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Domain;

public class Battle : Entity<BattleId>, IAggregateRoot<BattleId>
{
    public DateTime TimeCreated { get; } = DateTime.UtcNow;

    public Player[] Players { get; private set; }

    public ActionTurnController<UnitId> UnitTurnController { get; private set; }
    public ActionParallelController<PlayerId> PlayerActionController { get; private set; }

    public Battle(BattleId id, IEnumerable<Player> players) : base(id)
    {
        Players = players.ToArray();
        if (Players.Length < 2)
            throw new DomainException("battle_cant_have_less_than_two_players_err");

        PlayerActionController = new(Players.GetIds());

        var units = Players.GetUnits();
        var unitsOrder = UnitOrderCalculator.CalculateActionOrder(units);
        var unitsIdsByOrder = unitsOrder.Select(i => units[i].Id).ToArray();

        UnitTurnController = new(unitsIdsByOrder);
    }

    public void Start(PlayerId playerId) =>
        PlayerActionController.Run(playerId, () =>
            AddEvent(new PlayerStartedBattleEvent(Id, playerId)));

    public void ApplyArtifact(
        PlayerId originPlayerId,
        UnitId originUnitId,
        ArtifactId artifactId,
        PlayerId targetPlayerId,
        UnitId targetUnitId,
        Random? random = null)
    {
        if (!PlayerActionController.IsAllDone)
            throw new DomainException("cant_apply_artifact_when_all_players_not_started_err");

        if (IsGameOver)
            throw new DomainException("cant_continue_if_game_over_err");

        UnitTurnController.Run(originUnitId, () =>
        {
            AddEvents(
                Players.ApplyArtifact(
                    battleId: Id,
                    originPlayerId,
                    originUnitId,
                    targetPlayerId,
                    targetUnitId,
                    artifactId,
                    random));

            if (!Players.IsUnitAlive(targetPlayerId, targetUnitId))
                AddEvent(new GameOverEvent(BattleId: Id, WinnerId: originPlayerId));
        });
    }

    public bool IsGameOver =>
        Players.IsAnyPlayerOutOfUnitsAlive();

    public override bool Equals(object? obj) =>
        obj is Battle battle?
            battle.Id == Id :
            false;

    public override int GetHashCode() => Id.GetHashCode();

    private int _version;
    int IAggregateRoot.Version { get => _version; set => _version = value; }
    public int Version => _version;
}

public record BattleId(string Value) : EntityId(Value)
{
    public BattleId() : this(NewGuid) {}
}
