namespace Game.Battle.Domain.PassiveSkills
{
    /// <summary>
    /// Maximum generated artifacts passive skill
    /// </summary>
    public record MaxGeneratedArtifactsPassiveSkill : PassiveSkill
    {
        public int MaxGeneratedArtifacts { get; }

        public MaxGeneratedArtifactsPassiveSkill(int maxAssembledArtifacts) 
        {
            MaxGeneratedArtifacts = maxAssembledArtifacts;
        }
    }
}
