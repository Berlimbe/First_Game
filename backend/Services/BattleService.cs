using System;
using First_Game.backend.Domain.Entities;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Services
{
    public class BattleService
    {
        public void StartBattle(Entity player, Entity enemy)
        {
            Console.WriteLine($"\n ---- Batalha Iniciada: {player.Name} VS {enemy.Name} ----");

            int round = 1;

            while (player.Life > 0 && enemy.Life > 0)
            {
                Console.WriteLine($"\n[Rodada {round}]");

                player.ExecuteTurn(enemy);

                if(enemy.Life <= 0)
                {
                    Console.WriteLine($"\nVITÓRIA! O inimigo {enemy.Name} foi derrotado.");
                    break;
                }

                enemy.ExecuteTurn(player);

                if(player.Life <= 0)
                {
                    Console.WriteLine($"\nGAME OVER! {player.Name} foi brutalmente macetado!");
                    break;
                }

                round++;
            }
        }
    }
}