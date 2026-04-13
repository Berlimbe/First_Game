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
            
            LifeMax = 170;
            Life = 170;
            ManaMax = 60;
            Mana = 60;
            Power = 50;
            Defense = 11;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} Revivendo os mortos e fazendo tudo apodrecer!");
            target.TakeDamage(Power);
        }
    }
}