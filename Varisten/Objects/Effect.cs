namespace Varisten.Objects
{
    public class Effect
    {
        public double DodgeChance { get; protected set; }
        public double CriticChance { get; protected set; }
        public double CriticDamage { get; protected set; }

        public Effect(double dodgeChance, double criticChance, double criticDamage)
        {
            DodgeChance = dodgeChance;
            CriticChance = criticChance;
            CriticDamage = criticDamage;
        }
    }
}
