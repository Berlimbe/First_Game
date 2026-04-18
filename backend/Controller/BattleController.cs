using Microsoft.AspNetCore.Mvc;
using First_Game.backend.Domain.Entities.Classes; // Importando seus heróis
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BattleController : ControllerBase
    {
        [HttpPost("attack")]
        public IActionResult Attack([FromBody] AttackRequest request)
        {
            // 1. O C# "acordou". Vamos recriar os modelos temporariamente
            // com os dados exatos que vieram da caixa do React (DTO)
            
            Warrior player = new Warrior("Jogador", ControlType.Player);
            player.Power = request.PlayerPower;

            Archer enemy = new Archer("Inimigo", ControlType.AI);
            enemy.Life = request.EnemyHp;
            enemy.Defense = request.EnemyDefense; // Adicionamos a defesa aqui!

            // 2. A MÁGICA DA SUA ARQUITETURA AQUI:
            // O inimigo usa o método que VOCÊ criou para receber o dano do jogador
            enemy.TakeDamage(player.Power);

            // 3. Prepara a resposta atualizada para o React
            var response = new 
            {
                newHp = enemy.Life,
                message = $"Você atacou! O inimigo tem agora {enemy.Life} de vida."
            };

            return Ok(response);
        }
    }

    // O nosso DTO (A caixa de transporte)
    public class AttackRequest
    {
        public int PlayerPower { get; set; }
        public int EnemyHp { get; set; }
        public int EnemyDefense { get; set; } // Precisamos da defesa para a matemática do seu TakeDamage funcionar
    }
}