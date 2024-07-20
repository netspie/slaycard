using Core.Domain;

namespace Game.Units
{
    public class Unit : Entity<UnitId>
    {
        // Skills ??
        // Equipment ??
        // Orbs ??
        // Stats ??

        public ActiveSkill[] ActiveSkills { get; set; }
        public PassiveSkill[] PassiveSkills { get; set; }

        public Unit(UnitId id) : base(id) {}
    }

    public record UnitId(string Value);

    public class Skill
    {
        
    }

    public class PassiveSkill
    {

    }

    public record ActiveSkill
    {
        public int MaxOccurence { get; }
        public int ChanceToOccure { get; }
        public int Chance { get; }
    }

    public abstract record SkillDefinition(string Id)
    {

    }

    public record QuickAttackSkillDefinition() : SkillDefinition("quick_attack")
    {
        
    }

    public record EnergySkillDefinition() : SkillDefinition("quick_attack")
    {

    }
}
