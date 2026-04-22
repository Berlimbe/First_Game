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

        //Novas colunas na nossa tabela de runs para o nosso inimigo
        public string EnemyName { get; set; } = string.Empty;
        public string EnemyClass { get; set; } = string.Empty;
        public int EnemyCurrentHp { get; set; }
        public int EnemyMaxHp { get; set; }

        //coluna para o BOSS
        public string? BossLore { get; set; } // O "?" indica que pode ser nulo (vazio) até chegar no andar 11
    }
}