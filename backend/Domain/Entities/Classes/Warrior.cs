namespace First_Game.backend.Domain.Entities.Classes
{
    public class Warrior : Entity
    {
        // O Construtor: Roda automaticamente quando o objeto é criado
        public Warrior(string name)
        {
            Name = name;
            Class = ClassType.Guerreiro; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            // Status Iniciais
            LifeMax = 150;
            Life = 150;
            ManaMax = 30;
            Mana = 30;
            Power = 15;
            Defense = 10;
        }

        // Polimorfismo: Como o Guerreiro age no turno
        public override void ExecuteTurne(Entity alvo)
        {
            Console.WriteLine($"{Name} ataca {target.Name} com sua espada!");
            target.TakeDamage(Power);
        }
    }
}