using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Barbarian : Entity
    {
        public Barbarian(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Barbarian; // Vinculando ao seu Enum
            Controller = controller; // Aqui a gente ta definindo quem ele é
            
            LifeMax = 180;
            Life = 180;
            ManaMax = 60;
            Mana = 10;
            Power = 80;
            Defense = 18;
        }

        public override void ExecuteTurn(Entity target)
        {
            Console.WriteLine($"{Name} ataca {target.Name} quebrando ossos e esmagando sem dó algum...");
            target.TakeDamage(Power);
        }
    }
}