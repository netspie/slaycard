using Core.Domain;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Domain;

public class Battle : Entity<BattleId>
{
    public Player[] Players { get; private set; }
    public ActionController<PlayerId> PlayerActionController { get; private set; } = new();
    public ActionController<UnitId> UnitActionController { get; private set; } = new();

    public Battle(BattleId id, IEnumerable<Player> players) : base(id)
    {
        Players = players.ToArray();
        PlayerActionController.SetActionExpectedNext(nameof(Start)).By(Players.GetIds());

        var units = Players.GetUnits();
        //var unitsOrder = UnitOrderCalculator.CalculateActionOrder(units);
        //var unitsIdsByOrder = unitsOrder.Select(i => units[i].Id).ToArray();

        //UnitActionController
        //    .SetActionExpectedNext(nameof(AssembleArtifacts), ActionRepeat.Multiple)
        //    .SetActionExpectedNext(nameof(ApplyArtifact), ActionRepeat.Multiple)
        //    .SetActionExpectedNext(nameof(Pass))
        //    .By(unitsIdsByOrder, mustObeyOrder: true);
    }

    public void Start(PlayerId playerId)
    {
        if (!PlayerActionController.CanMakeAction(nameof(Start), playerId))
            throw new Exception("cant_start_the_battle_again");

        AddEvent(new PlayerStartedBattleEvent(
            Id, playerId));

        PlayerActionController
            .SetActionDone(nameof(Start), playerId)
            .SetActionExpectedNext(nameof(AssembleArtifacts), ActionRepeat.Multiple)
            .SetActionExpectedNext(nameof(ApplyArtifact), ActionRepeat.Multiple)
            .SetActionExpectedNext(nameof(Pass), ActionRepeat.Single);
    }

    public void AssembleArtifacts(
        PlayerId playerId,
        UnitId unitId,
        ArtifactId originArtifactId,
        ArtifactId? targetArtifactId = null)
    {
        if (!PlayerActionController.CanMakeAction(nameof(AssembleArtifacts), playerId))
            throw new Exception("cant_perform_this_operation");

        if (!UnitActionController.CanMakeAction(nameof(AssembleArtifacts), unitId))
            throw new Exception("cant_perform_this_operation");

        var events = Players.AssembleArtifacts(
            playerId,
            unitId,
            originArtifactId,
            targetArtifactId);

        AddEvents(events);

        UnitActionController.SetActionDone(nameof(AssembleArtifacts), unitId);
    }

    // Change so it supports area/group applies
    public void ApplyArtifact(
        PlayerId originPlayerId,
        UnitId originUnitId,
        ArtifactId artifactId,
        PlayerId targetPlayerId,
        UnitId targetUnitId)
    {
        if (!PlayerActionController.CanMakeAction(nameof(AssembleArtifacts), originPlayerId))
            throw new Exception("cant_perform_this_operation");

        if (!UnitActionController.CanMakeAction(nameof(AssembleArtifacts), originUnitId))
            throw new Exception("cant_perform_this_operation");

        Players.ApplyArtifact(
            originPlayerId,
            originUnitId,
            targetPlayerId,
            artifactId,
            targetUnitId);

        // if can't do anymore, then automatically pass
        // Pass(originPlayerId, originUnitId);

        UnitActionController.SetActionDone(nameof(ApplyArtifact), originUnitId);
        //UnitActionController.ActionInfo.GetAction().
    }

    public void Pass(
        PlayerId originPlayerId,
        UnitId originUnitId)
    {
        if (!PlayerActionController.CanMakeAction(nameof(Pass), originPlayerId))
            throw new Exception("cant_perform_this_operation");

        if (!UnitActionController.CanMakeAction(nameof(Pass), originUnitId))
            throw new Exception("cant_perform_this_operation");

        UnitActionController.SetActionDone(nameof(Pass), originUnitId);

        //Players.GenerateActionCards();
    }
}

public record BattleId(string Value) : EntityId(Value)
{
    public BattleId() : this(NewGuid) { }
}