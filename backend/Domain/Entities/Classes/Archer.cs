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
            
            LifeMax = 150;
            Life = 150;
            Power = 30;
            Defense = 4;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} dando flechadas!");
            target.TakeDamage(Power);
        }
    }
}