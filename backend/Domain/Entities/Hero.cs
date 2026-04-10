namespace First_Game.backend.Domain.Entities
{
    public abstract class Hero : Entity
    {
        public int Experience { get; set; }

        // O Herói pode ter um método de subir de nível
        public void GanharXP(int quantidade)
        {
            Experience += quantidade;
            if (Experience >= 100) // Lógica simples 
            {
                Nivel++;
                Experience = 0;
                Console.WriteLine($"{Nome} subiu para o nível {Level}!");
            }
        }
    }
}