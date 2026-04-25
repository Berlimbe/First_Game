import React, { useState } from "react";
import axios from "axios";
//Importamos o modal do alert do boss chegando
import BossModal from "../components/BossModal";

export default function StartScreen({ onStartGame }) {
  // 1. Nossos estados (A memória temporária do React)
  const [playerName, setPlayerName] = useState("");
  const [className, setClassName] = useState("Warrior");
  const [loading, setLoading] = useState(false); // Para mostrar um "Carregando..."

  const [showBossModal, setShowBossModal] = useState(false);
  const [bossMessage, setBossMessage] = useState("");
  const [tempIds, setTempIds] = useState(null); // Guarda os IDs para quando ele clicar no botão do Modal

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
        ClassName: className,
      };

      // 4. Enviamos para a porta do nosso GameController
      const response = await axios.post(
        "http://localhost:5290/api/game/start",
        payload,
      );

      console.log("Sucesso do Back-end:", response.data);

      // 3. A LÓGICA DO MODAL SUBSTITUI O ALERT
      if (response.data.mensagem.includes("BOSS")) {
        setBossMessage(response.data.mensagem); // Guarda a mensagem
        setTempIds({
          runId: response.data.runId,
          playerId: response.data.playerId,
        }); // Guarda os IDs
        setShowBossModal(true); // Exibe a tela preta com letras douradas
      } else {
        // Se for jogo normal, vai direto para a arena!
        onStartGame(response.data.runId, response.data.playerId);
      }
    } catch (error) {
      console.error(error);
      alert("Erro de conexão!");
    } finally {
      setLoading(false);
    }
  };

  // 4. FUNÇÃO QUANDO O JOGADOR CLICA NO BOTÃO DO MODAL
  const handleAcknowledgeBoss = () => {
    setShowBossModal(false);
    // Agora sim envia os IDs para o App trocar a tela!
    onStartGame(tempIds.runId, tempIds.playerId);
  };

  return (
    <div
      className="arena-container"
      style={{ justifyContent: "center", alignItems: "center" }}
    >
      {showBossModal && (
        <BossModal
          message={bossMessage}
          onAcknowledge={handleAcknowledgeBoss}
        />
      )}
      <div className="status-box" style={{ width: "400px", padding: "40px" }}>
        <h1 style={{ color: "#C5A059", marginBottom: "30px" }}>
          First Game RPG
        </h1>

        <div style={{ marginBottom: "20px", textAlign: "left" }}>
          <label style={{ display: "block", marginBottom: "5px" }}>
            Nome do Herói:
          </label>
          <input
            type="text"
            value={playerName}
            onChange={(e) => setPlayerName(e.target.value)} // Atualiza o estado ao digitar
            style={{
              width: "100%",
              padding: "10px",
              borderRadius: "5px",
              border: "none",
              boxSizing: "border-box",
            }}
            placeholder="Ex: Aragorn"
          />
        </div>

        <div style={{ marginBottom: "30px", textAlign: "left" }}>
          <label style={{ display: "block", marginBottom: "5px" }}>
            Escolha sua Classe:
          </label>
          <select
            value={className}
            onChange={(e) => setClassName(e.target.value)} // Atualiza o estado ao selecionar
            style={{
              width: "100%",
              padding: "10px",
              borderRadius: "5px",
              border: "none",
              cursor: "pointer",
            }}
          >
            <option value="Warrior">Warrior (Guerreiro)</option>
            <option value="Mage">Mage (Mago)</option>
            <option value="Archer">Archer (Arqueiro)</option>
            <option value="Pirate">Pirate (Pirata)</option>
            <option value="Cleric">Cleric (Clérigo)</option>
            <option value="Viking">Viking (Viking)</option>
            <option value="Hunter">Hunter (Caçador)</option>
            <option value="Marksman">Marksman (Atirador)</option>
            <option value="Necromancer">Necromancer (Necromante)</option>
            <option value="Barbarian">Barbarian (Bárbaro)</option>
          </select>
        </div>

        <button
          className="btn-action"
          onClick={handleStart}
          disabled={loading} // Desativa o botão se estiver carregando
          style={{ width: "100%" }}
        >
          {loading ? "Forjando Herói..." : "Iniciar Jornada"}
        </button>
      </div>
    </div>
  );
}
