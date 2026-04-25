import React, { useState, useEffect } from "react";
import axios from "axios";
import StatusBox from "../components/StatusBox";
import InventoryModal from "../components/InventoryModal";

export default function BattleArena({ runId, playerId, onReset }) {
  const [playerData, setPlayerData] = useState(null);
  const [enemyData, setEnemyData] = useState(null);
  const [loading, setLoading] = useState(true);

  const [showInventory, setShowInventory] = useState(false);
  const [attackBonus, setAttackBonus] = useState(0); // Para a Poção da Força!

  const [battleLog, setBattleLog] = useState(
    "A batalha começou! Escolha sua ação.",
  );
  const [isEnemyTurn, setIsEnemyTurn] = useState(false);
  const [gameOver, setGameOver] = useState(false);
  const [playerXp, setPlayerXp] = useState(0);
  const [showMenu, setShowMenu] = useState(false);

  useEffect(() => {
    const fetchBattleData = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5290/api/game/run/${runId}`,
        );
        setPlayerData(response.data.player);
        setEnemyData(response.data.enemy);
        setPlayerXp(response.data.player.xp || 0);
        setLoading(false);
      } catch (error) {
        console.error("Erro ao buscar a partida:", error);
      }
    };

    if (runId) fetchBattleData();
  }, [runId]);

  const handleAttack = async (skillName) => {
    try {
      setIsEnemyTurn(true);
      const payload = {
        RunId: runId,
        PlayerId: playerId,
        SkillName: skillName,
      };
      const response = await axios.post(
        "http://localhost:5290/api/battle/attack",
        payload,
      );

      setEnemyData((prevData) => ({
        ...prevData,
        currentHp: response.data.enemyHpAtual,
      }));

      if (response.data.inimigoMorreu) {
        setBattleLog(
          `${response.data.playerLog} \n⭐ ${response.data.enemyLog}`,
        );
        setPlayerXp(response.data.playerXpAtual);
        setPlayerData((prev) => ({
          ...prev,
          currentHp: response.data.playerHpAtual,
          maxHp: response.data.playerMaxHpAtual, // Garante que o novo HP Max apareça
          level: response.data.playerLevelAtual,
        }));
        return;
      }

      setBattleLog(response.data.playerLog);

      setTimeout(() => {
        setBattleLog(response.data.enemyLog);
        setPlayerData((prevData) => ({
          ...prevData,
          currentHp: response.data.playerHpAtual,
          maxHp: response.data.playerMaxHpAtual || prevData.maxHp,
        }));

        if (response.data.playerMorreu) setGameOver(true);
        else setIsEnemyTurn(false);
      }, 3000);
    } catch (error) {
      console.error("Erro ao atacar:", error);
      setBattleLog(error.response?.data || "O ataque falhou!");
      setIsEnemyTurn(false);
    }
  };

  const handleNextFloor = async () => {
    try {
      const response = await axios.post(
        `http://localhost:5290/api/battle/next-round/${runId}`,
      );

      // 1. Atualiza os dados do novo inimigo
      setEnemyData(response.data.enemy);

      // 2. MELHORIA VISUAL: Usar Template Literals e um separador " | " para não embolar o texto
      setBattleLog(`${response.data.mensagem} | ${response.data.itemLog}`);

      // 3. Libera os botões de ataque para o novo round
      setIsEnemyTurn(false);
    } catch (error) {
      // 4. TRATAMENTO DE ERRO: Se a internet cair ou o backend falhar, o jogador é avisado
      console.error("Erro ao avançar de andar:", error);
      setBattleLog(
        "A masmorra tremeu... Falha ao carregar o próximo andar. Tente novamente.",
      );
    }
  };

  if (loading || !playerData || !enemyData) {
    return (
      <div style={{ color: "white", textAlign: "center", marginTop: "20%" }}>
        <h1>Entrando na Masmorra...</h1>
      </div>
    );
  }

  const handleItemUsed = (statusAtualizados) => {
    setPlayerData(prev => ({
      ...prev,
      currentHp: statusAtualizados.hp,
      maxHp: statusAtualizados.maxHp
    }));
    setAttackBonus(statusAtualizados.bonus);
    
    // Agora o React escreve exatamente o que o C# mandou dizer!
    setBattleLog(statusAtualizados.mensagem || "Item utilizado.");
  };

  return (
    <div
      className="arena-container"
      style={{
        // COMO COLOCAR A IMAGEM DE FUNDO:
        // Se a sua imagem (ex: fundo.jpg) estiver na pasta "public" do seu projeto, use assim:
        // backgroundImage: "url('/fundo.jpg')",
        backgroundColor: "#111", // Cor de fundo de segurança
        backgroundSize: "cover",
        backgroundPosition: "center",
        minHeight: "100vh",
        position: "relative",
      }}
    >
      <button onClick={() => setShowMenu(true)} style={styles.menuBtn}>
        ⚙️ Menu
      </button>

      {/* Botão da Mochila */}
      <button
        onClick={() => setShowInventory(true)}
        style={{ ...styles.menuBtn, right: "100px" }} // Fica ao lado do Menu
      >
        🎒 Mochila
      </button>

      {/* Renderiza o Modal se o jogador clicar no botão */}
      {showInventory && (
        <InventoryModal
          runId={runId}
          onClose={() => setShowInventory(false)}
          onItemUsed={handleItemUsed}
        />
      )}

      {enemyData.currentHp <= 0 && !gameOver && (
        <div style={styles.victoryBanner}>
          <h2 style={{ margin: "0 0 10px 0", color: "#FFD700" }}>
            Inimigo Derrotado!
          </h2>
          <button onClick={handleNextFloor} style={styles.nextFloorBtn}>
            AVANÇAR PARA O PRÓXIMO ANDAR ➔
          </button>
        </div>
      )}

      {showMenu && (
        <div style={styles.modalOverlay}>
          <div style={styles.modalContent}>
            <h2>Menu da Partida</h2>
            <p>
              <strong>Herói:</strong> {playerData.name} ({playerData.className})
            </p>
            <p>
              <strong>Nível:</strong> {playerData.level || 1}
            </p>
            <p>
              <strong>XP:</strong> {playerXp}/100
            </p>

            <button onClick={() => setShowMenu(false)} style={styles.modalBtn}>
              Voltar para Batalha
            </button>
            <button
              onClick={onReset}
              style={{
                ...styles.modalBtn,
                backgroundColor: "#e63946",
                marginTop: "20px",
              }}
            >
              Abandonar e Criar Novo Herói
            </button>
          </div>
        </div>
      )}

      {enemyData.bossLore && (
        <div
          style={{
            color: "#FFD700",
            padding: "20px",
            textAlign: "center",
            fontStyle: "italic",
            backgroundColor: "rgba(0,0,0,0.6)",
          }}
        >
          <h2>A Lenda:</h2>
          <p>{enemyData.bossLore}</p>
        </div>
      )}

      <div
        className="battlefield"
        style={{ marginTop: enemyData.currentHp <= 0 ? "120px" : "20px" }}
      >
        {/* STATUS DO JOGADOR */}
        <StatusBox
          name={playerData.name}
          level={playerData.level || 1}
          className={playerData.className}
          currentHp={playerData.currentHp}
          maxHp={playerData.maxHp}
        />

        {/* STATUS DO INIMIGO */}
        <StatusBox
          name={enemyData.name}
          // O inimigo não tem nível fixo no nosso banco de dados no momento, então passamos só a classe
          className={enemyData.className}
          currentHp={enemyData.currentHp}
          maxHp={enemyData.maxHp}
        />
      </div>

      <div
        style={{
          textAlign: "center",
          color: "white",
          margin: "20px auto",
          padding: "15px",
          backgroundColor: "rgba(0,0,0,0.8)",
          border: "2px solid #FFD700",
          width: "60%",
          borderRadius: "10px",
        }}
      >
        <h3 style={{ margin: 0 }}>{battleLog}</h3>
      </div>

      <div
        style={{
          width: "60%",
          margin: "0 auto 20px auto",
          textAlign: "left",
          backgroundColor: "#333",
          border: "1px solid white",
          borderRadius: "5px",
          overflow: "hidden",
        }}
      >
        <div
          style={{
            width: `${playerXp}%`,
            backgroundColor: "#00b4d8",
            height: "15px",
            transition: "width 0.5s ease",
          }}
        ></div>
      </div>

      {/* AVISO DO BÔNUS DE FORÇA */}
      {attackBonus > 0 && (
        <div
          style={{
            textAlign: "center",
            color: "#ffb703",
            fontWeight: "bold",
            marginBottom: "15px",
            fontSize: "1.2rem",
            textShadow: "1px 1px 2px black",
          }}
        >
          🔥 Bônus de Força Ativo: +{attackBonus} de Dano no próximo ataque!
        </div>
      )}

      <div
        className="action-menu"
        style={{
          display: "flex",
          gap: "10px",
          justifyContent: "center",
          paddingBottom: "30px",
        }}
      >
        {playerData.skills &&
          playerData.skills.map((skill, index) => (
            <button
              key={index}
              className="btn-action"
              onClick={() => handleAttack(skill)}
              disabled={enemyData.currentHp <= 0 || isEnemyTurn || gameOver}
              style={{
                opacity: isEnemyTurn || enemyData.currentHp <= 0 ? 0.5 : 1,
              }}
            >
              {skill}
            </button>
          ))}
      </div>

      {gameOver && (
        <div style={styles.gameOverOverlay}>
          <h1
            style={{
              color: "black",
              fontSize: "5rem",
              textShadow: "2px 2px white",
            }}
          >
            VOCÊ MORREU
          </h1>
          <button onClick={onReset} style={styles.modalBtn}>
            Tentar Novamente
          </button>
        </div>
      )}
    </div>
  );
}

const styles = {
  menuBtn: {
    position: "absolute",
    top: "15px",
    right: "15px",
    padding: "10px 15px",
    backgroundColor: "rgba(0,0,0,0.7)",
    color: "white",
    border: "1px solid white",
    borderRadius: "5px",
    cursor: "pointer",
    fontSize: "1rem",
    zIndex: 5,
  },
  victoryBanner: {
    position: "absolute",
    top: "20px",
    left: "50%",
    transform: "translateX(-50%)",
    backgroundColor: "rgba(0, 50, 0, 0.9)",
    padding: "20px",
    border: "3px solid #2a9d8f",
    borderRadius: "10px",
    textAlign: "center",
    boxShadow: "0 0 20px rgba(42, 157, 143, 0.8)",
    zIndex: 10,
    width: "400px",
  },
  nextFloorBtn: {
    padding: "12px 20px",
    backgroundColor: "#2a9d8f",
    color: "white",
    border: "none",
    borderRadius: "5px",
    fontSize: "1.1rem",
    fontWeight: "bold",
    cursor: "pointer",
    width: "100%",
  },
  modalOverlay: {
    position: "absolute",
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    backgroundColor: "rgba(0,0,0,0.8)",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    zIndex: 20,
  },
  modalContent: {
    backgroundColor: "#222",
    padding: "30px",
    borderRadius: "10px",
    color: "white",
    border: "2px solid gray",
    textAlign: "center",
    minWidth: "300px",
  },
  modalBtn: {
    display: "block",
    width: "100%",
    padding: "10px",
    marginTop: "10px",
    backgroundColor: "#444",
    color: "white",
    border: "1px solid white",
    borderRadius: "5px",
    cursor: "pointer",
    fontSize: "1rem",
  },
  gameOverOverlay: {
    position: "absolute",
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    backgroundColor: "rgba(150, 0, 0, 0.8)",
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center",
    zIndex: 30,
  },
};
