namespace WordGame.Logic
{
    public class Encounter
    {
        public virtual Encounter TakeDamage(int combatValue)
        {
            return this;
        }
    }
}
