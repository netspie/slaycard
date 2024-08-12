using Core.Collections;
using Core.Domain;
using Game.Battle.Domain.PassiveSkills;

namespace Game.Battle.Domain;

public class Unit : IEntity<UnitId>
{
    public UnitId Id { get; init; }

    public StatGroup BaseStats { get; private set; }
    public StatGroup CombatStats { get; private set; }

    //public AssemblySpace? AssemblySpace { get; private set; }

    public List<Artifact> AssembledArtifacts { get; private set; }
    public List<Artifact> Artifacts { get; private set; }
    //public PassiveSkill[]? Artifacts { get; private set; }

    public List<PassiveSkill> PassiveSkills { get; private set; }

    public Unit(UnitId id)
    {
        Id = id;
    }

    //public ActionRow ActionRow { get; private set; }

    public IDomainEvent[] AssembleArtifacts(
        ArtifactId originArtifactId,
        ArtifactId? targetArtifactId = null,
        int? assemblyIndex = null)
    {
        var max = PassiveSkills.SingleOfType<MaxAssembledArtifactsPassiveSkill>().MaxAssembledArtifacts;
        if (targetArtifactId is null && AssembledArtifacts.Count >= max)
            throw new Exception("max_ass_arts_reached");

        var originArtifact = Artifacts.GetOfId(originArtifactId);
        var targetArtifact = targetArtifactId is null ? null : AssembledArtifacts.GetOfId(targetArtifactId);
        if (targetArtifactId is not null && targetArtifact is null)
            throw new Exception("target_art_not_found");

        if (assemblyIndex is int index)
            AssembledArtifacts.Insert(index, originArtifact);
        else
        if (targetArtifact is not null)
        {
            var result = targetArtifact.Assemble(originArtifact);
            if (result.Artifact is null)
                throw new Exception("cant_assemble_with_this_art");

            AssembledArtifacts.Replace(targetArtifact, result.Artifact);

            return result.Events;
        }

        throw new Exception("error_occured");
    }

    public void GenerateArtifacts()
    {
        // TO DO: only once per turn or when have extra by skill or item?
        //var maxArtifacts = PassiveSkills.GetSingleOfType<MaxGeneratedArtifactsPassiveSkill>()

    }

    public void ApplyArtifact(
        Unit originUnit,
        Artifact artifact)
    {
        //artifact.ApplyToTarget(
        //    originUnit.CombatStats,
        //    CombatStats);
    }
}

public record UnitId(string Value);

public static class UnitExtensions
{
    public static void ApplyArtifact(
        this IEnumerable<Unit> units,
        Artifact artifact,
        Unit originUnit,
        UnitId targetUnitId)
    {
        var targetUnit = units.GetOfId(targetUnitId);
        targetUnit.ApplyArtifact(originUnit, artifact);
    }

    public static IDomainEvent[] AssembleArtifacts(
        this IEnumerable<Unit> units,
        UnitId unitId,
        ArtifactId originArtifactId,
        ArtifactId? targetArtifactId = null)
    {
        var unit = units.GetOfId(unitId);
        return unit.AssembleArtifacts(originArtifactId, targetArtifactId);
    }
}