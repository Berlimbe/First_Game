namespace First_Game.backend.Domain.Entities
{
    public abstract class Enemy : Entity
    {
        public int XPReward { get; set; }

        // Agora ele simplesmente foca no único herói que está na arena
        public virtual Entity SelectTarget(Hero player)
        {
            return player;
        }
    }
}