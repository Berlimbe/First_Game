using Microsoft.AspNetCore.Mvc;
using First_Game.backend.Data;
using First_Game.backend.Domain.Models;
using System.Linq;
using System;

namespace First_Game.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BattleController : ControllerBase
    {
        // 1. Criamos a variável vazia para guardar o Banco de Dados
        private readonly AppDbContext _context;
        // O nosso "Livro de Magias" mapeando Nomes para Estratégias!
        private readonly Dictionary<string, IAttackStrategy> _skillBook;

        // 2. O CONSTRUTOR: Aqui o C# injeta o banco de dados pronto para usarmos!
        public BattleController(AppDbContext context)
        {
            _context = context;

            // Ligamos cada habilidade do jogo à sua respectiva estratégia de dano:
            _skillBook = new Dictionary<string, IAttackStrategy>
            {
                // MAGE
                { "Bola de Fogo", new BalancedAttack() },
                { "Lança Espiritual", new SafeAttack() },
                { "Feixe de Luz Transluzente", new RiskyAttack() },
                
                // WARRIOR
                { "Corte Profundo", new SafeAttack() },
                { "Combo de Golpes", new BalancedAttack() },
                { "Fúria do Soldado de Elite", new RiskyAttack() },
                
                // ARCHER
                { "Flechada Certeira", new SafeAttack() },
                { "Flechas em Dobro", new BalancedAttack() },
                { "Tempestade de Flechas", new RiskyAttack() },
                
                // CLERIC
                { "Porrada de Cajado", new SafeAttack() },
                { "Magia de Invitalidade", new BalancedAttack() },
                { "Magia Suprema da Eternidade", new RiskyAttack() },
                
                // ASSASSIN
                { "Apunhalada de Adaga", new SafeAttack() },
                { "Salto Mortal", new BalancedAttack() },
                { "Combo do Assassino", new RiskyAttack() },
                
                // BARBARIAN
                { "Estocada Bárbara", new SafeAttack() },
                { "Corte de Espada Pesada", new BalancedAttack() },
                { "Três espadas do Louco", new RiskyAttack() },
                
                // PIRATE
                { "Sabre de Sangue", new SafeAttack() },
                { "Escopeta Amaldiçoada", new BalancedAttack() },
                { "Espada da Invocação do Kraken", new RiskyAttack() },
                
                // HUNTER
                { "Corte Impiedoso", new SafeAttack() },
                { "Armas do Caçador", new BalancedAttack() },
                { "Ritual de Caça: Coração Pulsante", new RiskyAttack() },
                
                // VIKING
                { "Ataque Bárbaro", new SafeAttack() },
                { "Machadada Dupla", new BalancedAttack() },
                { "Berserker", new RiskyAttack() },
                
                // MARKSMAN
                { "Bala Certeira", new SafeAttack() },
                { "Artigo de Balas", new BalancedAttack() },
                { "Fúria do Pistoleiro: Kick-Shot", new RiskyAttack() },
                
                // NECROMANCER
                { "Mortos-vivos", new SafeAttack() },
                { "Golem de Carne Transmumificado", new BalancedAttack() },
                { "Magia-extrema: Gigante dos céus", new RiskyAttack() },

                // DEFAULT (Caso a classe não tenha habilidades cadastradas)
                { "Ataque Básico", new SafeAttack() },
                { "Defesa", new SafeAttack() },
                { "Fuga", new RiskyAttack() }
            };
        }

        // 3. AQUI VEM O SEU MÉTODO POST DE ATAQUE!
        [HttpPost("attack")]
        public IActionResult ProcessTurn([FromBody] AttackRequest request)
        {
            var run = _context.Runs.FirstOrDefault(r => r.Id == request.RunId);
            if (run == null) return NotFound("Partida não encontrada.");

            if (run.EnemyCurrentHp <= 0) return BadRequest("O inimigo já está morto!");
            if (run.PlayerCurrentHp <= 0) return BadRequest("Você está morto!");

            var aneisEquipados = _context.Items
        .Where(i => i.RunId == run.Id && i.Type == "Ring" && i.IsEquipped)
        .ToList();

            double multiplicadorDano = 1.0;
            foreach (var anel in aneisEquipados)
            {
                if (anel.Name == "Anel de Poder") multiplicadorDano += 0.25;
                if (anel.Name == "Anel da Força") multiplicadorDano += 0.50;
                if (anel.Name == "Anel Invencível") multiplicadorDano += 0.8;
                if (anel.Name == "Anel Inútil") multiplicadorDano += 0.1;
            }

            // 2. CALCULAR PODER TOTAL (Base * Anéis + Poção de Força)
            int poderFinal = (int)(run.PlayerBasePower * multiplicadorDano) + run.NextAttackBonus;

            // 1. TURNO DO JOGADOR
            int danoDoJogador = 0;

            // AGORA SIM! Pegamos o poder real do jogador salvo no banco!
            int powerDoPlayer = run.PlayerBasePower;

            if (_skillBook.TryGetValue(request.SkillName, out IAttackStrategy strategy))
            {
                danoDoJogador = strategy.CalculateDamage(powerDoPlayer);
            }
            else
            {
                IAttackStrategy defaultStrategy = new SafeAttack();
                danoDoJogador = defaultStrategy.CalculateDamage(powerDoPlayer);
            }

            run.EnemyCurrentHp -= danoDoJogador;
            if (run.EnemyCurrentHp < 0) run.EnemyCurrentHp = 0;

            // 2. SISTEMA DE VITÓRIA, LEVEL UP E TURNO DO INIMIGO
            bool leveledUp = false;
            int xpGained = 0;
            int danoDoInimigo = 0;
            string nomeAtaqueInimigo = "";

            if (run.EnemyCurrentHp <= 0)
            {
                // SE O INIMIGO MORREU
                run.EnemiesDefeated++;
                Random randXp = new Random();
                xpGained = randXp.Next(30, 101);
                run.PlayerXp += xpGained;

                if (run.PlayerXp >= 100)
                {
                    run.PlayerLevel++;
                    run.PlayerXp -= 100;

                    run.PlayerMaxHp += 50;
                    run.PlayerCurrentHp = run.PlayerMaxHp; // Cura total
                    run.PlayerBasePower += 15; // Aumenta o dano do próximo ataque!
                    run.PlayerDefense += 3;

                    leveledUp = true;
                }

                nomeAtaqueInimigo = "Nenhum";
            }
            else
            {
                // SE O INIMIGO SOBREVIVEU, ELE ATACA!
                Random rand = new Random();
                int chance = rand.Next(1, 101);

                // VERIFICA SE É O BOSS (Andar 11) OU INIMIGO COMUM (Andares 1 a 10)
                if (run.CurrentRound == 11)
                {
                    // --- LÓGICA DO BOSS ---
                    if (chance <= 33)
                    {
                        danoDoInimigo = rand.Next(80, 120);
                        nomeAtaqueInimigo = "Espada Flamejante - Golpe Pesado";
                    }
                    else if (chance <= 58)
                    {
                        danoDoInimigo = rand.Next(100, 180);
                        nomeAtaqueInimigo = "Sopro das Sombras Dilacerantes";
                    }
                    else if (chance <= 83)
                    {
                        danoDoInimigo = rand.Next(120, 220);
                        nomeAtaqueInimigo = "Espada Encantada de Alma das Cinzas Reais";
                    }
                    else
                    {
                        danoDoInimigo = rand.Next(150, 300);
                        nomeAtaqueInimigo = "Abstrador de Núcleo de Almas";
                    }
                }
                else
                {
                    // --- LÓGICA DO INIMIGO COMUM ---
                    // O dano escala com o andar atual (CurrentRound) para o jogo ficar mais difícil aos poucos
                    int multiplicadorAndar = run.CurrentRound * 5;

                    if (chance <= 50)
                    {
                        // Ataque Básico do Inimigo Comum
                        danoDoInimigo = rand.Next(5, 15) + multiplicadorAndar;
                        nomeAtaqueInimigo = "Golpe Rápido";
                    }
                    else if (chance <= 85)
                    {
                        // Ataque Forte
                        danoDoInimigo = rand.Next(10, 25) + multiplicadorAndar;
                        nomeAtaqueInimigo = $"Ataque Brutal de {run.EnemyClass}"; // Usa o nome da classe dele!
                    }
                    else
                    {
                        // Ataque Crítico do Inimigo
                        danoDoInimigo = rand.Next(20, 40) + multiplicadorAndar;
                        nomeAtaqueInimigo = "Fúria do Monstro";
                    }
                    int danoReal = danoDoInimigo - run.PlayerDefense;
                    if (danoReal < 0) danoReal = 0;

                    run.PlayerCurrentHp -= danoReal;
                    if (run.PlayerCurrentHp < 0) run.PlayerCurrentHp = 0;
                }

                // Desconta a vida do jogador
                run.PlayerCurrentHp -= danoDoInimigo;
                if (run.PlayerCurrentHp < 0) run.PlayerCurrentHp = 0;
            }

            _context.SaveChanges();

            string msgVitoria = leveledUp
                ? $"VITÓRIA! +{xpGained} XP. LEVEL UP (Nv {run.PlayerLevel})! HP Restaurado!"
                : $"VITÓRIA! +{xpGained} XP. (XP Atual: {run.PlayerXp}/100)";

            return Ok(new
            {
                playerLog = $"Você usou {request.SkillName} (-{danoDoJogador} HP).",
                enemyLog = run.EnemyCurrentHp <= 0 ? msgVitoria : $"O inimigo usou {nomeAtaqueInimigo} (-{danoDoInimigo} HP).",

                enemyHpAtual = run.EnemyCurrentHp,
                playerHpAtual = run.PlayerCurrentHp,
                playerMaxHpAtual = run.PlayerMaxHp,

                playerXpAtual = run.PlayerXp,
                playerLevelAtual = run.PlayerLevel,

                inimigoMorreu = run.EnemyCurrentHp <= 0,
                playerMorreu = run.PlayerCurrentHp <= 0
            });
        }

        // O nosso DTO (A caixa de transporte)
        public class AttackRequest
        {
            public int RunId { get; set; }
            public int PlayerId { get; set; }
            public string SkillName { get; set; }
        }

        // 1. O CONTRATO (A Interface)
        // Toda estratégia de ataque no jogo OBRIGATORIAMENTE tem que calcular dano.
        public interface IAttackStrategy
        {
            int CalculateDamage(int basePower);
        }

        // 2. OS CHIPS (As Estratégias Reais)
        public class SafeAttack : IAttackStrategy
        {
            public int CalculateDamage(int basePower) => (int)(basePower * 0.8) + new Random().Next(1, 5);
        }

        public class BalancedAttack : IAttackStrategy
        {
            public int CalculateDamage(int basePower) => basePower + new Random().Next(-10, 20);
        }

        public class RiskyAttack : IAttackStrategy
        {
            public int CalculateDamage(int basePower) => new Random().Next(10, (int)(basePower * 2.5));
        }

        [HttpPost("next-round/{runId}")]
        public IActionResult NextRound(int runId)
        {
            var run = _context.Runs.FirstOrDefault(r => r.Id == runId);
            if (run == null) return NotFound();

            if (run.EnemyCurrentHp > 0) return BadRequest("O inimigo ainda está vivo!");

            // 1. Avança o andar
            run.CurrentRound++;

            Random rand = new Random();

            // 2. LÓGICA DO BOSS (A cada 10 andares: 10, 20, 30...)
            if (run.CurrentRound % 10 == 0)
            {
                run.EnemyName = "Lorde do Abismo";
                run.EnemyClass = "BOSS";
                // HP do Boss escala muito mais (Base 500 + 150 por dezena de andar)
                run.EnemyMaxHp = 500 + (run.CurrentRound * 15);
                run.BossLore = "Um pesadelo esquecido que desperta a cada dez camadas da masmorra...";
            }
            else
            {
                // INIMIGO COMUM
                string[] nomes = { "Carcereiro", "Sombra", "Infectado", "Gárgula", "Espectro" };
                string[] classes = { "Warrior", "Mage", "Archer", "Assassin", "Necromancer" };

                run.EnemyName = nomes[rand.Next(nomes.Length)];
                run.EnemyClass = classes[rand.Next(classes.Length)];
                run.BossLore = null;

                // Dificuldade Escalável: Aumentamos o multiplicador conforme o andar
                // HP base 100 + (Andar * 25) -> No andar 20 terá 600 HP
                run.EnemyMaxHp = 100 + (run.CurrentRound * 25);
            }

            run.EnemyCurrentHp = run.EnemyMaxHp;

            // 3. Sorteio de Itens
            Random randLoot = new Random();
            int categoryRoll = randLoot.Next(1, 101);
            string itemLog = "";

            // 30% DE CHANCE: ANÉIS
            if (categoryRoll <= 30)
            {
                int ringRoll = randLoot.Next(1, 101);
                ItemModel novoAnel = new ItemModel { RunId = runId, Type = "Ring" };

                if (ringRoll <= 10) { novoAnel.Name = "Anel Invencível"; novoAnel.Value = 50; } // Muito Raro
                else if (ringRoll <= 30) { novoAnel.Name = "Anel da Força"; novoAnel.Value = 25; }
                else if (ringRoll <= 50) { novoAnel.Name = "Anel da Vida"; novoAnel.Value = 50; }
                else if (ringRoll <= 80) { novoAnel.Name = "Anel de Poder"; novoAnel.Value = 10; }
                else { novoAnel.Name = "Anel Inútil"; novoAnel.Value = 1; }

                _context.Items.Add(novoAnel);
                itemLog = $"⭐ LENDÁRIO! Você encontrou um {novoAnel.Name}!";
            }
            // 70% DE CHANCE: CONSUMÍVEIS (Poções e Amuletos)
            else
            {
                int potionRoll = randLoot.Next(1, 101);
                ItemModel novoConsumivel = new ItemModel { RunId = runId, Type = "Consumable" };

                if (potionRoll <= 20) { novoConsumivel.Name = "Poção de Cura Pequena"; novoConsumivel.Value = 35; }
                else if (potionRoll <= 40) { novoConsumivel.Name = "Poção de Cura"; novoConsumivel.Value = 70; }
                else if (potionRoll <= 55) { novoConsumivel.Name = "Poção de Cura Grande"; novoConsumivel.Value = 150; }
                else if (potionRoll <= 70) { novoConsumivel.Name = "Poção da Força"; novoConsumivel.Value = 50; }
                else if (potionRoll <= 80) { novoConsumivel.Name = "Amuleto da Sorte"; novoConsumivel.Value = 0; }
                else if (potionRoll <= 90) { novoConsumivel.Name = "Amuleto Maldito"; novoConsumivel.Value = 0; }
                else
                {
                    // ARMADILHA (Poção Envenenada): Aplica o dano instantaneamente e NÃO vai para o inventário!
                    run.PlayerCurrentHp -= 20;
                    if (run.PlayerCurrentHp < 0) run.PlayerCurrentHp = 0;

                    itemLog = "☠️ AZAR! Você tocou em uma Poção Envenenada e perdeu 20 HP!";
                    novoConsumivel = null; // Destrói o item para ele não ir pro banco
                }

                if (novoConsumivel != null)
                {
                    _context.Items.Add(novoConsumivel);
                    itemLog = $"Você encontrou: {novoConsumivel.Name}";
                }
            }

            _context.SaveChanges();

            return Ok(new
            {
                mensagem = $"Andar {run.CurrentRound}",
                itemLog = itemLog,
                enemy = new
                {
                    name = run.EnemyName,
                    className = run.EnemyClass, // Garantindo que a classe seja enviada
                    currentHp = run.EnemyCurrentHp,
                    maxHp = run.EnemyMaxHp,
                    bossLore = run.BossLore
                }
            });
        }
    }
}