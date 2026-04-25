using Microsoft.AspNetCore.Mvc;
using First_Game.backend.Data;
using First_Game.backend.Domain.Models;
using First_Game.backend.Domain.Entities;
using First_Game.backend.Domain.Entities.Classes;
using First_Game.backend.Domain.Enums;
using System.Linq;
using System;

namespace First_Game.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // CORREÇÃO 1: "Route" com 'R' maiúsculo
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context) 
        {
            _context = context;
        }

        // ROTA 1: Busca todos os itens do jogador na partida atual
        [HttpGet("{runId}")]
        public IActionResult GetInventory(int runId)
        {
            var items = _context.Items.Where(i => i.RunId == runId).ToList();
            return Ok(items);
        }

        // ROTA 2: Usa ou Equipa um item
        [HttpPost("use")]
        public IActionResult UseItem([FromBody] UseItemRequest req)
        {
            var item = _context.Items.Find(req.ItemId);
            if (item == null) return NotFound("Item não encontrado.");

            var run = _context.Runs.Find(item.RunId);
            if (run == null) return NotFound("Partida não encontrada.");

            // LÓGICA DE EFEITOS
            string logMensagem = "Item utilizado com sucesso!";

            if (item.Type == "Consumable") 
            {
                if (item.Name.Contains("Cura")) 
                {
                    run.PlayerCurrentHp = Math.Min(run.PlayerMaxHp, run.PlayerCurrentHp + item.Value);
                    logMensagem = $"Você recuperou {item.Value} de HP!";
                } 
                else if (item.Name == "Poção da Força") 
                {
                    run.NextAttackBonus += item.Value; 
                    logMensagem = $"Sua força aumentou! +{item.Value} de dano no próximo ataque.";
                }
                else if (item.Name == "Amuleto Maldito")
                {
                    // Não faz absolutamente nada de bom. Apenas será deletado no final.
                    logMensagem = "O Amuleto Maldito virou pó em suas mãos... Inútil.";
                }
                else if (item.Name == "Amuleto da Sorte")
                {
                    // A MINI-ROLETA DA SORTE!
                    Random rand = new Random();
                    int sorte = rand.Next(1, 101);
                    
                    ItemModel itemSorteado = new ItemModel { RunId = run.Id };

                    if (sorte <= 40) { 
                        itemSorteado.Name = "Poção de Cura Grande"; 
                        itemSorteado.Type = "Consumable"; 
                        itemSorteado.Value = 150; 
                    }
                    else if (sorte <= 80) { 
                        itemSorteado.Name = "Poção da Força"; 
                        itemSorteado.Type = "Consumable"; 
                        itemSorteado.Value = 50; 
                    }
                    else { 
                        // 20% de chance de tirar a "Loteria" do amuleto
                        itemSorteado.Name = "Anel Invencível"; 
                        itemSorteado.Type = "Ring"; 
                        itemSorteado.Value = 50; 
                    }

                    _context.Items.Add(itemSorteado); // Adiciona o prêmio no banco!
                    logMensagem = $"O Amuleto brilhou! Você ganhou: {itemSorteado.Name}!";
                }
                
                // O item usado (inclusive os amuletos) é deletado do inventário
                _context.Items.Remove(item); 
            } 
            else if (item.Type == "Ring") 
            {
                item.IsEquipped = !item.IsEquipped; 
                
                if (item.Name == "Anel da Vida") 
                {
                    if (item.IsEquipped) {
                        run.PlayerMaxHp += item.Value;
                        run.PlayerCurrentHp += item.Value;
                    } else {
                        run.PlayerMaxHp -= item.Value;
                        run.PlayerCurrentHp = Math.Min(run.PlayerCurrentHp, run.PlayerMaxHp);
                    }
                }
                logMensagem = item.IsEquipped ? $"{item.Name} equipado!" : $"{item.Name} desequipado!";
            }

            _context.SaveChanges();
            
            return Ok(new { 
                hp = run.PlayerCurrentHp, 
                maxHp = run.PlayerMaxHp, 
                bonus = run.NextAttackBonus,
                mensagem = logMensagem // Enviando o texto pro React!
            });
        }
    }

    // CORREÇÃO 2: A classe que ensina o C# a ler o JSON do React
    public class UseItemRequest
    {
        public int ItemId { get; set; }
    }
}