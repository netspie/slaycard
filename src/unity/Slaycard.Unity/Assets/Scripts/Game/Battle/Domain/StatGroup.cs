#nullable enable

namespace Game.Battle.Domain
{
    public class StatGroup
    {
        public Stat HP { get; }
        public Stat Energy { get; }

        public Stat Damage { get; }
        public Stat Defence { get; }
        public Stat Accuracy { get; }
        public Stat Dodge { get; }
        public Stat Critics{ get; }
        public Stat Speed { get; }
    }
}
