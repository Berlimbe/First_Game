using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Marksman : Entity
    {
        public Marksman(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Marksman; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            LifeMax = 200;
            Life = 200;
            Power = 45;
            Defense = 6;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} atirando em suas pernas e mirando na sua cabeça!");
            target.TakeDamage(Power);
        }
    }
}