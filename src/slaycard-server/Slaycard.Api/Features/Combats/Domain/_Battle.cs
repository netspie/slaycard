﻿using Core.Domain;
using Slaycard.Api.Features.Combats.Domain.Events;

namespace Slaycard.Api.Features.Combats.Domain;

public class Battle : Entity<BattleId>
{
    public DateTime TimeCreated { get; } = DateTime.UtcNow;

    public Player[] Players { get; private set; }
    public ActionController<PlayerId> PlayerActionController { get; private set; } = new();
    public ActionController<UnitId> UnitActionController { get; private set; } = new();

    public Battle(BattleId id, IEnumerable<Player> players) : base(id)
    {
        Players = players.ToArray();
        if (Players.Length < 2)
            throw new Exception("cant_have_less_than_two_players_err");

        PlayerActionController.SetActionExpectedNext(nameof(Start)).By(Players.GetIds());

        var units = Players.GetUnits();
        var unitsOrder = UnitOrderCalculator.CalculateActionOrder(units);
        var unitsIdsByOrder = unitsOrder.Select(i => units[i].Id).ToArray();

        UnitActionController
            .SetActionExpectedNext(nameof(ApplyArtifact))
            .By(unitsIdsByOrder, mustObeyOrder: true);
    }

    public void Start(PlayerId playerId)
    {
        if (!PlayerActionController.CanMakeAction(nameof(Start), playerId))
            throw new Exception("cant_start_the_battle_again_err");

        AddEvent(new PlayerStartedBattleEvent(
            Id, playerId));

        PlayerActionController
            .SetActionDone(nameof(Start), playerId)
            .SetActionExpectedNext(nameof(ApplyArtifact), ActionRepeat.Multiple)
            .By(Players.GetIds(), mustObeyOrder: true);
    }

    public void ApplyArtifact(
        PlayerId originPlayerId,
        UnitId originUnitId,
        ArtifactId artifactId,
        PlayerId targetPlayerId,
        UnitId targetUnitId,
        Random? random = null)
    {
        if (IsGameOver)
            throw new Exception("cant_continue_if_game_over_err");

        if (!UnitActionController.CanMakeAction(nameof(ApplyArtifact), originUnitId))
            throw new Exception("cant_perform_this_operation_err");

        AddEvents(
            Players.ApplyArtifact(
                battleId: Id,
                originPlayerId,
                originUnitId,
                targetPlayerId,
                artifactId,
                targetUnitId,
                random));

        if (!Players.IsUnitAlive(targetPlayerId, targetUnitId))
            AddEvent(new GameOverEvent(BattleId: Id, WinnerId: originPlayerId));

        UnitActionController.SetActionDone(nameof(ApplyArtifact), originUnitId);
    }

    public bool IsGameOver =>
        Players.IsAnyPlayerOutOfUnitsAlive();

    public override bool Equals(object? obj) =>
        obj is Battle battle?
            battle.Id == Id :
            false;

    public override int GetHashCode() => Id.GetHashCode();
}

public record BattleId(string Value) : EntityId(Value)
{
    public BattleId() : this(NewGuid) {}
}
