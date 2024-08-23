using Core.Domain;

namespace Slaycard.Api.Features.Combats.Domain;

public class Player : IEntity<PlayerId>
{
    public PlayerId Id { get; init; }

    public Unit[] Units { get; private set; } = [];

    public Player(PlayerId id)
    {
        Id = id;
    }

    public void ApplyArtifact(
        Player originPlayer,
        UnitId originUnitId,
        ArtifactId artifactId,
        UnitId targetUnitId)
    {
        var artifact = originPlayer.TakeArtifact(originUnitId, artifactId);
        var originUnit = originPlayer.GetUnit(originUnitId);

        Units.ApplyArtifact(artifact, originUnit, targetUnitId);
    }

    public IDomainEvent[] AssembleArtifacts(
        UnitId unitId,
        ArtifactId originArtifactId,
        ArtifactId? targetArtifactId = null)
    {
        return Units.AssembleArtifacts(unitId, originArtifactId, targetArtifactId);
    }

    private Artifact TakeArtifact(
        UnitId unitId,
        ArtifactId skillId)
    {
        return default;
    }

    private Unit GetUnit(UnitId unitId) =>
        Units.GetOfId(unitId);
}

public record PlayerId(string Value) : EntityId(Value);

public static class PlayerExtensions
{
    public static void ApplyArtifact(
        this IEnumerable<Player> players,
        PlayerId originPlayerId,
        UnitId originUnitId,
        PlayerId targetPlayerId,
        ArtifactId artifactId,
        UnitId targetUnitCardId)
    {
        var originPlayer = players.GetOfId(originPlayerId);
        var targetPlayer = players.GetOfId(targetPlayerId);

        targetPlayer.ApplyArtifact(
            originPlayer, originUnitId, artifactId, targetUnitCardId);
    }

    public static IDomainEvent[] AssembleArtifacts(
        this IEnumerable<Player> players,
        PlayerId playerId,
        UnitId unitId,
        ArtifactId originArtifactId,
        ArtifactId? targetArtifactId = null)
    {
        var player = players.GetOfId(playerId);
        return player.AssembleArtifacts(unitId,  originArtifactId, targetArtifactId);
    }

    public static Unit[] GetUnits(this IEnumerable<Player> players) =>
        players.SelectMany(player => player.Units).ToArray();
}
