import React, { useState, useEffect } from "react";
import axios from "axios";
import bgImagem from "../assets/bg-arena.jpg";
import StatusBox from "../components/StatusBox";

export default function BattleArena({ runId, playerId }) {
  //o playerId estava retornando erro pq ele n estava sendo usado, então usamos ele no console, enquanto n usamos ele no Attack
  console.log("ID do Jogador pronto para a batalha:", playerId);
  // 1. Criamos os "Estados" vazios esperando os dados chegarem
  const [playerData, setPlayerData] = useState(null);
  const [enemyData, setEnemyData] = useState(null);
  const [loading, setLoading] = useState(true); // Controla a tela de carregamento

  // 2. O useEffect roda UMA ÚNICA VEZ assim que a Arena é aberta
  useEffect(() => {
    const fetchBattleData = async () => {
      try {
        // Bate na rota GET que acabamos de criar no C#
        const response = await axios.get(
          `http://localhost:5290/api/game/run/${runId}`,
        );

        // Guarda os dados nas variáveis do React
        setPlayerData(response.data.player);
        setEnemyData(response.data.enemy);
        setLoading(false); // Desliga o carregamento
      } catch (error) {
        console.error("Erro ao buscar a partida:", error);
      }
    };

    // Só busca se o runId existir
    if (runId) {
      fetchBattleData();
    }
  }, [runId]); // Esse array diz: "Se o runId mudar, rode de novo"

  // 3. Enquanto o C# não responde, mostramos uma tela de loading
  if (loading) {
    return (
      <div style={{ color: "white", textAlign: "center", marginTop: "20%" }}>
        <h1>Entrando na Masmorra...</h1>
      </div>
    );
  }

  return (
    <div
      className="arena-container"
      style={{ backgroundImage: `url(${bgImagem})` }}
    >
      {/* 4. Usamos os dados REAIS do banco para montar a tela! */}
      <div className="battlefield">
       <StatusBox 
          name={`${playerData.name} (${playerData.className})`} 
          currentHp={playerData.currentHp} maxHp={playerData.maxHp} 
          currentMp={30} maxMp={30} 
        />
        
        <StatusBox 
          name={`${enemyData.name} (${enemyData.className})`} 
          currentHp={enemyData.currentHp} maxHp={enemyData.maxHp} 
          currentMp={0} maxMp={0} 
        />
      </div>

      <div className="action-menu">
        <button className="btn-action">Atacar</button>
        <button className="btn-action" style={{ backgroundColor: "#457B9D" }}>
          Magia
        </button>
        <button className="btn-action" style={{ backgroundColor: "#D4A373" }}>
          Pocão
        </button>
      </div>
    </div>
  );
}
