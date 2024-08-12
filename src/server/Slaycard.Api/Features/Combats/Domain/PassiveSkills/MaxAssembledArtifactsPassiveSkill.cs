namespace Slaycard.Combats.Domain.PassiveSkills;

/// <summary>
/// Maximum assembled artifacts passive skill
/// </summary>
public record MaxAssembledArtifactsPassiveSkill : PassiveSkill
{
    public int MaxAssembledArtifacts { get; }

    public MaxAssembledArtifactsPassiveSkill(int maxAssembledArtifacts) 
    {
        MaxAssembledArtifacts = maxAssembledArtifacts;
    }
}
