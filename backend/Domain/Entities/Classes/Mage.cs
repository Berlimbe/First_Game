using System;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Domain.Entities
{
    public class Mage : Entity
    {
        public Mage(string name)
        {
            Name = name;
            Class = ClassType.Mage; // Vinculando ao seu Enum
            
            // Status Iniciais do Mago
            LifeMax = 80;
            Life = 80;
            ManaMax = 200;
            Mana = 200;
            Power = 5; // Ataque físico base é bem fraco
            Defense = 4;
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