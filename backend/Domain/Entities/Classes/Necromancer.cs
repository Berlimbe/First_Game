using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Necromancer : Entity
    {
        public Necromancer(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Necromancer; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            LifeMax = 250;
            Life = 250;
            Power = 25;
            Defense = 12;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} Revivendo os mortos e fazendo tudo apodrecer!");
            target.TakeDamage(Power);
        }
    }
}