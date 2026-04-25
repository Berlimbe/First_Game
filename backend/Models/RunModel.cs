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

        //Novas colunas para o Player

        public int PlayerCurrentHp{ get; set; }

        public int PlayerMaxHp{ get; set; }

        //coluna para o BOSS
        public string? BossLore { get; set; } // O "?" indica que pode ser nulo (vazio) até chegar no andar 11

        // Novas colunas para evolução
        public int PlayerLevel { get; set; } = 1;
        public int PlayerXp { get; set; } = 0;
        public int PlayerBasePower { get; set; } = 50; // O dano base que vai para os Chips de Strategy!
        public int PlayerDefense { get; set; } = 5;

        //para a poção de força
        public int NextAttackBonus { get; set; } = 0;
    }
}