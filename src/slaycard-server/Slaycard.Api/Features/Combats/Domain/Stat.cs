namespace Slaycard.Api.Features.Combats.Domain;

public class Stat
{
    public int OriginalValue { get; }
    public List<StatModifier> Modifiers { get; private set; } = new();

    public Stat(int originalValue)
    {
        OriginalValue = originalValue;
    }

    public void Modify(double value, string id, bool isFactor = false) =>
        Modifiers.Add(new(value, isFactor, id));

    public int CalculatedValue
    {
        get
        {
            var value = OriginalValue;

            Modifiers.ForEach(m =>
                value = m.IsFactor ? value = (int)(value * m.Value) : (int)(value + m.Value));

            return value;
        }
    }
}

public record StatModifier(double Value, bool IsFactor = false, string? Id = null);
