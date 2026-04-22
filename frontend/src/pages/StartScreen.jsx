import React, { useState } from 'react';
import axios from 'axios';

export default function StartScreen({ onStartGame }) {
  // 1. Nossos estados (A memória temporária do React)
  const [playerName, setPlayerName] = useState("");
  const [className, setClassName] = useState("Warrior");
  const [loading, setLoading] = useState(false); // Para mostrar um "Carregando..."

  // 2. A função disparada ao clicar no botão
  const handleStart = async () => {
    // Validação básica: Não deixa começar sem nome
    if (playerName.trim() === "") {
      alert("Por favor, digite o nome do seu herói!");
      return;
    }

    setLoading(true);

    try {
      // 3. Montamos o pacote EXATAMENTE com os nomes do DTO do C#
      const payload = {
        PlayerName: playerName,
        ClassName: className
      };

      // 4. Enviamos para a porta do nosso GameController
      const response = await axios.post("http://localhost:5290/api/game/start", payload);
      
      console.log("Sucesso do Back-end:", response.data);
      
      // 5. Avisamos ao App principal que o jogo começou, passando o ID da Run e do Player!
      onStartGame(response.data.runId, response.data.playerId);

    } catch (error) {
      console.error("Erro ao conectar com o servidor:", error);
      alert("Erro ao conectar com o banco de dados. O C# está rodando?");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="arena-container" style={{ justifyContent: 'center', alignItems: 'center' }}>
      <div className="status-box" style={{ width: '400px', padding: '40px' }}>
        <h1 style={{ color: '#C5A059', marginBottom: '30px' }}>First Game RPG</h1>
        
        <div style={{ marginBottom: '20px', textAlign: 'left' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Nome do Herói:</label>
          <input 
            type="text" 
            value={playerName}
            onChange={(e) => setPlayerName(e.target.value)} // Atualiza o estado ao digitar
            style={{ width: '100%', padding: '10px', borderRadius: '5px', border: 'none', boxSizing: 'border-box' }}
            placeholder="Ex: Aragorn"
          />
        </div>

        <div style={{ marginBottom: '30px', textAlign: 'left' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Escolha sua Classe:</label>
          <select 
            value={className}
            onChange={(e) => setClassName(e.target.value)} // Atualiza o estado ao selecionar
            style={{ width: '100%', padding: '10px', borderRadius: '5px', border: 'none', cursor: 'pointer' }}
          >
            <option value="Warrior">Warrior (Guerreiro)</option>
            <option value="Mage">Mage (Mago)</option>
            <option value="Archer">Archer (Arqueiro)</option>
            <option value="Pirate">Pirate (Pirata)</option>
            <option value="Cleric">Cleric (Clérigo)</option>
            <option value="Viking">Viking (Viking)</option>
            <option value="Hunter">Hunter (Caçador)</option>
            <option value="Marksman">Marksman (Atirador)</option>
            <option value="Necromance">Necromance (Necromante)</option>
            <option value="Barbarian">Barbarian (Bárbaro)</option>

          </select>
        </div>

        <button 
          className="btn-action" 
          onClick={handleStart}
          disabled={loading} // Desativa o botão se estiver carregando
          style={{ width: '100%' }}
        >
          {loading ? "Forjando Herói..." : "Iniciar Jornada"}
        </button>
      </div>
    </div>
  );
}