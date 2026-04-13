using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Warrior : Entity
    {
        // O Construtor: Roda automaticamente quando o objeto é criado
        public Warrior(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Warrior; // Vinculando ao seu Enum
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
        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} com sua espada!");
            target.TakeDamage(Power);
        }
    }
}