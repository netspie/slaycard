using Core.Domain;
using Slaycard.Api.Core.Domain;

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
            throw new DomainException("player_cant_have_zero_units_err");
    }

    public IDomainEvent[] ApplyArtifact(
        BattleId battleId,
        Player originPlayer,
        UnitId originUnitId,
        ArtifactId artifactId,
        UnitId targetUnitId,
        Random? random)
    {
        var originUnit = originPlayer
            .GetUnit(originUnitId)
            .ThrowIfNull("origin_unit_doesnt_exist_err");

        var artifact = originUnit
            .GetArtifact(artifactId)
            .ThrowIfNull("artifact_doesnt_exist_err");

        var targetUnit = Units
            .GetOfId(targetUnitId)
            .ThrowIfNull("target_unit_doesnt_exist_err");

        var args = new ApplyArtifactArgs(
            battleId,
            originPlayer, 
            originUnit, 
            TargetPlayer: this, 
            [targetUnit]);

        return artifact.ApplyToTarget(args, random);
    }

    public bool IsUnitAlive(UnitId unitId)
    {
        var unit = GetUnit(unitId).ThrowIfNull("unit_doesnt_exist_err");
        return unit.IsAlive();
    }

    public Unit? GetWeakestUnit() =>
        Units
            .OrderBy(u => u.CombatStats.HP.CalculatedValue)
            .FirstOrDefault(u => u.IsAlive());

    public bool HasSomeUnitsAlive() =>
        Units.All(unit => unit.IsAlive());

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
    public static IDomainEvent[] ApplyArtifact(
        this IEnumerable<Player> players,
        BattleId battleId,
        PlayerId originPlayerId,
        UnitId originUnitId,
        PlayerId targetPlayerId,
        UnitId targetUnitId,
        ArtifactId artifactId,
        Random? random = null)
    {
        var originPlayer = players.GetOfId(originPlayerId).ThrowIfNull("origin_player_doesnt_exist_err");
        var targetPlayer = players.GetOfId(targetPlayerId).ThrowIfNull("target_player_doesnt_exist_err");

        return targetPlayer.ApplyArtifact(
            battleId, originPlayer, originUnitId, artifactId, targetUnitId, random);
    }

    public static Unit[] GetUnits(this IEnumerable<Player> players) =>
        players.SelectMany(player => player.Units).ToArray();

    public static bool IsUnitAlive(
        this IEnumerable<Player> players,
        PlayerId originPlayerId,
        UnitId originUnitId)
    {
        var originPlayer = players.GetOfId(originPlayerId).ThrowIfNull("player_doesnt_exist_err");
        return originPlayer.IsUnitAlive(originUnitId);
    }

    public static bool IsAnyPlayerOutOfUnitsAlive(
        this IEnumerable<Player> players)
    {
        return players.Any(player => !player.HasSomeUnitsAlive());
    }
}
