namespace First_Game.backend.Domain.Models
{
    public class PlayerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SelectedClass { get; set; } // Guardamos o nome da classe escolhida
        public int Level { get; set; } = 1;
        public int TotalXp { get; set; }
    }
}