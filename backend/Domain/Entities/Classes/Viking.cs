using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Viking : Entity
    {
        public Viking(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Viking; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            LifeMax = 200;
            Life = 200;
            ManaMax = 0;
            Mana = 0;
            Power = 60;
            Defense = 17;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} jogando seu machado e batendo sua espada!");
            target.TakeDamage(Power);
        }
    }
}