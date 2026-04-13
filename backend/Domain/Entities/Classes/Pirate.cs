using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Pirate : Entity
    {
        public Pirate(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Pirate; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            LifeMax = 150;
            Life = 150;
            ManaMax = 40;
            Mana = 40;
            Power = 40;
            Defense = 15;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} com seu sabre rapidamente arranhando seu pescoço!");
            target.TakeDamage(Power);
        }
    }
}