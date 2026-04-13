using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Archer : Entity
    {
        public Archer(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Archer; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            LifeMax = 120;
            Life = 120;
            ManaMax = 45;
            Mana = 45;
            Power = 25;
            Defense = 8;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} dando flechadas!");
            target.TakeDamage(Power);
        }
    }
}