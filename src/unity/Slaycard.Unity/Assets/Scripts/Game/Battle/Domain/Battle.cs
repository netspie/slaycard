#nullable enable

using Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Game.Battle.Domain
{
    public class Battle : Entity<BattleId>
    {
        public Player[] Players { get; private set; }

        public Battle(BattleId id, IEnumerable<Player> players) : base(id)
        {
            Players = players.ToArray();
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
        public void UseSkill(
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
