using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Cleric : Entity
    {
        public Cleric(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Cleric; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            LifeMax = 300;
            Life = 300;
            Power = 20;
            Defense = 8;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} batendo seu cajado maciço!");
            target.TakeDamage(Power);
        }
    }
}