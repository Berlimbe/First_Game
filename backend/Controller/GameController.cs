using Microsoft.AspNetCore.Mvc;
using First_Game.backend.Data;
using First_Game.backend.Domain.Models;
using First_Game.backend.Domain.Entities;
using First_Game.backend.Domain.Entities.Classes;
using First_Game.backend.Domain.Enums;

namespace First_Game.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GameController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("start")]
        public IActionResult StartGame([FromBody] StartGameRequest request)
        {
            var newPlayer = new PlayerModel
            {
                Name = request.PlayerName,
                SelectedClass = request.ClassName
            };

            _context.Players.Add(newPlayer);
            _context.SaveChanges();

            // DADOS PARA O INIMIGO/BOSS
            string enemyName;
            string enemyClass;
            int enemyMaxHp;
            string? bossLore = null; // Começa vazio

            // ATENÇÃO: Mude para 11 se quiser testar o Boss agora mesmo!
            int currentRound = 11;
            double difficultyMultiplier = 1.0 + (currentRound * 0.1);

            // SE FOR O BOSS (ANDAR 11)
            if (currentRound == 11)
            {
                var bossData = GenerateBossStory(); // Chama a tupla
                bossLore = bossData.lore;
                enemyClass = bossData.currentClass;
                enemyName = "Lord of Cinders"; 

                // Cria as entidades baseadas nas strings retornadas!
                Entity pastEntity = CreateEntity(bossData.pastClass, enemyName);
                Entity currentEntity = CreateEntity(bossData.currentClass, enemyName);

                enemyMaxHp = (int)((pastEntity.Life + currentEntity.Life) * difficultyMultiplier);
            }
            // SE FOR UM INIMIGO COMUM (ANDARES 1 A 10)
            else
            {
                string[] classesDisponiveis = { "Warrior", "Mage", "Archer", "Pirate", "Cleric", "Viking", "Hunter", "Marksman", "Necromancer", "Barbarian", "Assassin" };
                string[] nomesInimigos = {
            "Ragnar", "Sylvanas", "Grommash", "Kael'thas", "Zul'jin", "Morrigan", "Draugr",
            "Malekith", "Lilith", "Astaroth", "Vladimir", "Carmilla", "Gargantua", "Fenrir",
            "Balthazar", "Lucian", "Seraphina", "Miguel Sanches", "Azrael", "Gideon", "Malakor",
            "Marcos Aurelio", "Darius", "Mordekaiser", "Manus Portador do Caos", "Surtur", "Ymir", "Marcão CTV", "Robert Priebsche",
            "Grendel", "Voldark", "Nefarian", "Bernardo Aurelio", "Illidan", "Arthas", "Bolvar",
            "Kerrigan", "Alarak", "Artanis", "Zeratul", "Diablo", "Mephisto", "Baal",
            "Malthael", "Tyrael", "Imperius", "Auriel", "Itherael", "Azmodan", "Belial"
        };

                Random random = new Random();
                enemyName = nomesInimigos[random.Next(nomesInimigos.Length)];
                enemyClass = classesDisponiveis[random.Next(classesDisponiveis.Length)];

                Entity inimigoVirtual = CreateEntity(enemyClass, enemyName);
                enemyMaxHp = (int)(inimigoVirtual.Life * difficultyMultiplier);
            }

            // SALVA A PARTIDA
            var newRun = new RunModel
            {
                PlayerId = newPlayer.Id,
                CurrentRound = currentRound,
                EnemyName = enemyName,
                EnemyClass = enemyClass,
                EnemyMaxHp = enemyMaxHp,
                EnemyCurrentHp = enemyMaxHp,
                BossLore = bossLore
            };

            _context.Runs.Add(newRun);
            _context.SaveChanges();

            // 5. Devolvemos uma resposta para o React
            return Ok(new
            {
                mensagem = currentRound == 11 ? "O BOSS DESPERTOU!" : "Partida iniciada!",
                runId = newRun.Id,
                playerId = newPlayer.Id
            });
        }

        // Esta rota ficará: http://localhost:5173/api/game/run/1
        [HttpGet("run/{runId}")]
        public IActionResult GetRunStatus(int runId)
        {
            var run = _context.Runs.FirstOrDefault(r => r.Id == runId);
            if (run == null) return NotFound("Partida não encontrada.");

            var player = _context.Players.FirstOrDefault(p => p.Id == run.PlayerId);
            if (player == null) return NotFound("Jogador não encontrado.");

            // Usando a nossa nova Mini-Fábrica para ficar limpo!
            Entity playerVirtual = CreateEntity(player.SelectedClass, player.Name);

            var response = new
            {
                player = new { name = player.Name, className = player.SelectedClass, currentHp = playerVirtual.Life, maxHp = playerVirtual.Life },
                enemy = new { name = run.EnemyName, className = run.EnemyClass, currentHp = run.EnemyCurrentHp, maxHp = run.EnemyMaxHp, bossLore = run.BossLore },
                round = run.CurrentRound
            };

            return Ok(response);
        }

        private Entity CreateEntity(string className, string name)
        {
            switch (className)
            {
                case "Mage": return new Mage(name, ControlType.AI);
                case "Archer": return new Archer(name, ControlType.AI);
                case "Marksman": return new Marksman(name, ControlType.AI);
                case "Necromancer": return new Necromancer(name, ControlType.AI);
                case "Barbarian": return new Barbarian(name, ControlType.AI);
                case "Pirate": return new Pirate(name, ControlType.AI);
                case "Viking": return new Viking(name, ControlType.AI);
                case "Hunter": return new Hunter(name, ControlType.AI);
                case "Assassin": return new Assassin(name, ControlType.AI);
                default: return new Warrior(name, ControlType.AI);
            }
        }

        private (string lore, string currentClass, string pastClass) GenerateBossStory()
        {
            string[] classes = { "Warrior", "Mage", "Archer", "Pirate", "Cleric", "Viking", "Hunter", "Marksman", "Necromancer", "Barbarian", "Assassin" };
            Random rand = new Random();

            var selected = classes.OrderBy(x => rand.Next()).Take(3).ToList();
            string past = selected[0];
            string growth = selected[1];
            string present = selected[2];

            string story = $"This master of combat was born a {past}, grew up under the traditions of a {growth}, and now dominates the battlefield as a ruthless {present}.";

            // A Tupla garante que o C# devolva as 3 informações de uma vez só!
            return (story, present, past);
        }
    }

    // DTO: A caixa que o React vai nos enviar
    public class StartGameRequest
    {
        public string PlayerName { get; set; }
        public string ClassName { get; set; }
    }
}