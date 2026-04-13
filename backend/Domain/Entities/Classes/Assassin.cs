using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Assassin : Entity
    {
        public Assassin(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Assassin; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            LifeMax = 100;
            Life = 100;
            ManaMax = 65;
            Mana = 65;
            Power = 40;
            Defense = 4;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} com suas adagas furtivas!");
            target.TakeDamage(Power);
        }
    }
}