namespace First_Game.backend.Domain.Models
{
    public class RunModel
    {
        public int Id { get; set; }
        public int PlayerId { get; set; } // Chave Estrangeira
        public int CurrentRound { get; set; }
        public int EnemiesDefeated { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; } = DateTime.Now;
    }
}