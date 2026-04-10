namespace First_Game.backend.Domain.Entities 
{
    public abstract class Entity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Cria um identificador único para cada jogador
        public string Name { get; set; }
        public int Level { get; set; } = 1;
        public int Life { get; set; }
        public int LifeMax { get; set; }      
        public int Mana {get; set;}
        public int ManaMax{get; set;}      
        public int Power { get; set; }
        public int Defense { get; set; }
        public ClassType Class {get; set;}
        public ControlType Controller { get; set; }
        
        public virtual void TakeDamage(int damage) //"virtual" permite que os "filhos" mudem como o dano é recebido
        {
            int finalDamage = damage - Defense;
            if (finalDamage < 0) finalDamage = 0;

            Life -= FinalDamage;
            if(Life < 0) Life = 0;

            System.Console.WriteLine($"{Name} recebeu {FinalDamage} de dano. Vida restante: {Life}");
        }

        public void Heal(int amount)
        {
            Life += amount;
            if (Life > LifeMax) Life = LifeMax;
        }

        public abstract void ExecuteTurn(Entity alvo);
    }
}