#nullable enable

using Core.Domain;
using Game.Battle.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Battle.Domain
{
    public class Battle : Entity<BattleId>
    {
        public Player[] Players { get; private set; }
        public PlayerId[]? UnitsMoveOrder { get; private set; }
        public PlayerActionController PlayerActionController { get; private set; } = new();

        public Battle(BattleId id, IEnumerable<Player> players) : base(id)
        {
            Players = players.ToArray();
            PlayerActionController.SetActionExpectedNext(nameof(Start)).By(Players.GetIds());
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
                .SetActionExpectedNext(nameof(Pass));
        }

        public void AssembleArtifacts(
            PlayerId playerId,
            UnitId unitId,
            ArtifactId originArtifactId,
            ArtifactId? targetArtifactId = null)
        {
            var events = Players.AssembleArtifacts(
                playerId,
                unitId,
                originArtifactId,
                targetArtifactId);

            AddEvents(events);
        }

        // Change so it supports area/group applies
        public void ApplyArtifact(
            PlayerId originPlayerId,
            UnitId originUnitId,
            ArtifactId artifactId,
            PlayerId targetPlayerId,
            UnitId targetUnitId)
        {
            Players.ApplyArtifact(
                originPlayerId,
                originUnitId,
                targetPlayerId,
                artifactId,
                targetUnitId);
        }

        public void Pass()
        {
            //Players.GenerateActionCards();
        }
    }

    public record BattleId(string Value);
}
