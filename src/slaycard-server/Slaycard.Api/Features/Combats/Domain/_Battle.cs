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

        AddEvent(
            new BattleInstantiatedEvent(
                Id,
                Players,
                UnitTurnController.Ids));
    }

    public void Start(PlayerId playerId) =>
        PlayerActionController.Run(playerId, () =>
            AddEvent(new PlayerStartedBattleEvent(Id, playerId)));

    public void ApplyArtifact(
        PlayerId originPlayerId,
        UnitId originUnitId,
        ArtifactId artifactId,
        PlayerId? targetPlayerId = null,
        UnitId? targetUnitId = null,
        RandomizerConfiguration? randomConfig = null,
        Random? random = null)
    {
        if (!PlayerActionController.IsAllDone)
            throw new DomainException("cant_apply_artifact_when_all_players_not_started_err");

        if (IsGameOver)
            throw new DomainException("cant_continue_if_game_over_err");

        // Initialize target if not specified
        if (targetPlayerId is null || targetUnitId is null)
        {
            var targetPlayer = Players.GetNotOfId(originPlayerId);

            targetPlayerId = targetPlayer.Id;
            targetUnitId = targetPlayer.Units[0].Id;
        }

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
                    randomConfig,
                    random));

           AddEvent(new PassedEvent(BattleId: Id, originPlayerId));

            if (!Players.IsUnitAlive(targetPlayerId, targetUnitId))
                AddEvent(
                    new GameOverEvent(
                        BattleId: Id, 
                        WinnerId: originPlayerId,
                        PlayerIds: Players.GetIds()));
        });
    }

    public bool IsGameOver =>
        Players.IsAnyPlayerOutOfUnitsAlive();

    public PlayerId CurrentPlayerId => Players.GetOfUnitId(CurrentUnitId)!.Id;
    public PlayerId NextPlayerId => Players.GetOfUnitId(NextUnitId)!.Id;
    public UnitId CurrentUnitId => UnitTurnController.CurrentUnitId;
    public UnitId NextUnitId => UnitTurnController.NextUnitId;

    #region Equals

    public override bool Equals(object? obj) =>
        obj is Battle battle?
            battle.Id == Id :
            false;

    public override int GetHashCode() => Id.GetHashCode();

    #endregion

    #region Version

    private int _version;
    int IAggregateRoot.Version { get => _version; set => _version = value; }
    public int Version => _version;
 
    #endregion
}

public record BattleId(string Value) : EntityId(Value)
{
    public BattleId() : this(NewGuid) {}
}
