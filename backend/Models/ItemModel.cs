namespace First_Game.backend.Domain.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public int RunId { get; set; } // O item pertence a uma partida específica
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Heal", "Strength", "Crit", "Mana"
        public int Value { get; set; } // Ex: 50 (cura 50) ou 10 (mais 10 de dano)
        public int Quantity { get; set; } = 1;
        public bool IsEquipped { get; set; } = false;
    }
}