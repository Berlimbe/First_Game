namespace First_Game.backend.Domain.Entities
{
    public class Mage : Hero
    {
        public Mage(string name)
        {
            Nome = name;
            Classe = ClassType.Mago; // Vinculando ao seu Enum
            
            // Status Iniciais do Mago
            VidaMax = 80;
            VidaAtual = 80;
            ManaMax = 200;
            ManaAtual = 200;
            Ataque = 5; // Ataque físico base é bem fraco
            Defesa = 4;
        }

        public override void ExecutarTurno(Entity alvo)
        {
            // O Mago toma decisões no turno baseadas em recursos
            if (ManaAtual >= 20)
            {
                Console.WriteLine($"{Nome} conjura uma Bola de Fogo massiva!");
                ManaAtual -= 20;
                alvo.ReceberDano(Ataque * 4); // Multiplicador de magia
            }
            else
            {
                Console.WriteLine($"{Nome} está sem mana e desfere um soco fraco...");
                alvo.ReceberDano(Ataque); // Dano base fraco
            }
        }
    }
}