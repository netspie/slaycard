namespace Game.Battle.Domain.PassiveSkills
{
    /// <summary>
    /// Maximum assembled artifacts passive skill
    /// </summary>
    public record MaxAssArtsPS : PassiveSkill
    {
        public int MaxAssembledArtifacts { get; }

        public MaxAssArtsPS(int maxAssembledArtifacts) 
        {
            MaxAssembledArtifacts = maxAssembledArtifacts;
        }
    }
}
