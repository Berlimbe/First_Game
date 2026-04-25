using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities.Classes
{
    public class Mage : Entity
    {
        public Mage(string name, ControlType controller)
        {
            Name = name;
            Class = ClassType.Mage; // Vinculando ao seu Enum
            Controller = controller;
            
            // Status Iniciais do Mago
            LifeMax = 180;
            Life = 180;
            Power = 50; 
            Defense = 20;
        }

        public override void ExecuteTurn(Entity alvo)
        {
            // O Mago toma decisões no turno baseadas em recursos
            if (Mana >= 20)
            {
                Console.WriteLine($"{Name} conjura uma Bola de Fogo massiva!");
                Mana -= 20;
                alvo.TakeDamage(Power * 4); // Multiplicador de magia
            }
            else
            {
                Console.WriteLine($"{Name} está sem mana e desfere um soco fraco...");
                alvo.TakeDamage(Power); // Dano base fraco
            }
        }
    }
}