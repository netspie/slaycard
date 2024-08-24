using Core.Collections;
using Core.Domain;

namespace Slaycard.Api.Features.Combats.Domain;

public class Player : IEntity<PlayerId>
{
    public PlayerId Id { get; init; }

    public Unit[] Units { get; private set; } = [];

    public Player(PlayerId id, IEnumerable<Unit> units)
    {
        Id = id;

        Units = units.ToArray();
        if (Units.Length == 0)
            throw new Exception("cant_have_zero_units");
    }

    public void ApplyArtifact(
        Player originPlayer,
        UnitId originUnitId,
        ArtifactId artifactId,
        UnitId targetUnitId,
        Random? random)
    {
        var artifact = originPlayer.GetArtifact(originUnitId, artifactId);
        if (artifact is null)
            throw new Exception("artifact_does_not_exist");

        var originUnit = originPlayer.GetUnit(originUnitId);
        if (originUnit is null)
            throw new Exception("origin_unit_does_not_exist");

        var targetUnit = Units.GetOfId(targetUnitId);
        if (targetUnit is null)
            throw new Exception("target_unit_does_not_exist");

        var args = new ApplyArtifactArgs(
            originPlayer, 
            originUnit, 
            TargetPlayer: this, 
            [targetUnit]);

        artifact.ApplyToTarget(args, random);
    }

    private Artifact? GetArtifact(
        UnitId unitId,
        ArtifactId artifactId)
    {
        return default;
    }

    private Unit? GetUnit(UnitId unitId) =>
        Units.GetOfId(unitId);

    public override bool Equals(object? obj) =>
        obj is Player player ?
            player.Id == Id :
            false;

    public override int GetHashCode() => Id.GetHashCode();
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
        UnitId targetUnitCardId,
        Random? random = null)
    {
        var originPlayer = players.GetOfId(originPlayerId).ThrowIfNull("origin_player_doesnt_exist");
        var targetPlayer = players.GetOfId(targetPlayerId).ThrowIfNull("target_player_doesnt_exist");

        targetPlayer.ApplyArtifact(
            originPlayer, originUnitId, artifactId, targetUnitCardId, random);
    }

    public static Unit[] GetUnits(this IEnumerable<Player> players) =>
        players.SelectMany(player => player.Units).ToArray();
}
