using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Hunter : Entity
    {
        public Hunter(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Hunter; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            LifeMax = 180;
            Life = 180;
            Power = 40;
            Defense = 4;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} jogando lâminas e adagas e ameaça com seu arco!");
            target.TakeDamage(Power);
        }
    }
}