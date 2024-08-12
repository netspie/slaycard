namespace Slaycard.Api.Features.Combats.Domain;

public record Chance
{
    public int Value { get; }

    public Chance(int value)
    {
        if (value < 0 || value > 100)
            throw new Exception("Chance must be in 0 to 100 % range");

        Value = value;
    }
}
