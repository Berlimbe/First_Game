using Microsoft.AspNetCore.Mvc;
using First_Game.backend.Data;
using First_Game.backend.Domain.Models;

namespace First_Game.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        // Variável que vai guardar o nosso "caderno" (Banco de Dados)
        private readonly AppDbContext _context;

        // CONSTRUTOR: Aqui acontece a mágica da Injeção de Dependência.
        // O C# entrega o banco de dados pronto para nós usarmos.
        public GameController(AppDbContext context)
        {
            _context = context;
        }

        // Esta rota ficará: http://localhost:5000/api/game/start
        [HttpPost("start")]
        public IActionResult StartGame([FromBody] StartGameRequest request)
        {
            // 1. Criamos a ficha do Jogador para salvar no banco
            var newPlayer = new PlayerModel
            {
                Name = request.PlayerName,
                SelectedClass = request.ClassName
            };

            _context.Players.Add(newPlayer); // Prepara para salvar
            _context.SaveChanges();          // Salva de verdade (isso gera o ID do jogador!)

            // 2. Criamos a Partida (Run) e amarramos ela ao ID do Jogador
            var newRun = new RunModel
            {
                PlayerId = newPlayer.Id, 
                CurrentRound = 1
            };

            _context.Runs.Add(newRun);
            _context.SaveChanges();

            // 3. Devolvemos uma resposta de sucesso para o React
            return Ok(new { 
                mensagem = $"Bem-vindo, {newPlayer.Name}! Partida iniciada.", 
                runId = newRun.Id,
                playerId = newPlayer.Id
            });
        }
    }

    // DTO: A caixa que o React vai nos enviar, esses nomes de "variáveis", vão vir do React
    public class StartGameRequest
    {
        public string PlayerName { get; set; }
        public string ClassName { get; set; }
    }
}