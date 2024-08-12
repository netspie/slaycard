using Core.Collections;

namespace Slaycard.Combats.Domain;

public static class CombatFormulas
{
    #region Hit Chance
    public static bool CalculateIfHit(Stat attackerAccuracy, Stat defenderDodge, Random? random) =>
        CalculateIfHit(attackerAccuracy.CalculatedValue, defenderDodge.CalculatedValue, random);

    public static bool CalculateIfHit(int attackerAccuracy, int defenderDodge, Random? random)
    {
        var percent = random.Percent();
        var hitChance = CalculateHitChance(attackerAccuracy, defenderDodge);

        return percent <= hitChance;
    }

    public const double MinHitChance = 0.05;
    public const double MidHitChance = 0.8;
    public const double MaxHitChance = 0.95;
    public static double CalculateHitChance(int attackerAccuracy, int defenderDodge)
    {
        double quotient = attackerAccuracy / (double) defenderDodge;
        double hitChance = quotient * MidHitChance;

        return Math.Clamp(hitChance, MinHitChance, MaxHitChance);
    }

    #endregion

    #region Damage

    public static double CalculateDamage(
        Stat attackerAttack,
        Stat defenderDefence,
        out (double low, double high) damageRange,
        Random? random = null) =>
        CalculateDamage(
            attackerAttack.CalculatedValue,
            defenderDefence.CalculatedValue,
            out damageRange,
            random);

    public const int DefenceFactor = 1;
    public const int DamageCalculationFactor = 1;
    public static double CalculateDamage(
        int attackerAttack,
        int defenderDefense, 
        out (double low, double high) damageRange,
        Random? random = null)
    {
        var midDamage = ((attackerAttack + defenderDefense) / (double) (defenderDefense) * DefenceFactor);
        var diffDamage = midDamage * 0.1;
        var lowestDamage = midDamage - diffDamage;
        var higherDamage = midDamage + diffDamage;

        damageRange = (lowestDamage, higherDamage);

        var l = (int) lowestDamage * DamageCalculationFactor;
        var h = (int) (higherDamage * DamageCalculationFactor) + 1;

        return (random ?? new()).Next(l, h) / DamageCalculationFactor;
    }

    #endregion

    #region Critics Chance

    public static bool CalculateIfCriticHit(
        Stat attackerCritics, Stat defenderCritics, out double criticalHitChance, Random? random = null)
    {
        criticalHitChance = CalculateCriticHitChance(attackerCritics, defenderCritics);
        var percent = random.Percent();

        return percent <= criticalHitChance;
    }

    public static double CalculateCriticHitChance(
        Stat attackerCritics, Stat defenderCritics) =>
        CalculateCriticHitChance(
            attackerCritics.CalculatedValue, defenderCritics.CalculatedValue);

    public const double MinCriticChance = 0.1;
    public const double MidCriticChance = 0.1;
    public const double MaxCriticChance = 0.9;
    public static double CalculateCriticHitChance(
        int attackerCritics, int defenderCritics)
    {
        double quotient = attackerCritics / (double) defenderCritics;
        double hitChanceMid = quotient * quotient * MidCriticChance;

        if (hitChanceMid < MinCriticChance)
            return MinCriticChance;

        if (hitChanceMid > MaxCriticChance)
            return MaxCriticChance;

        return hitChanceMid;
    }

    #endregion

    #region Critics Factor

    public static double CalculateCriticalDamage(
        double originalDamage, Random? random = null)
    {
        return originalDamage * 1.4;
    }

    #endregion
}
