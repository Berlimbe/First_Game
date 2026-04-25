import React, { useState } from 'react'; // Apague o useEffect daqui
import StartScreen from './pages/StartScreen';
import BattleArena from './pages/BattleArena';
import './index.css';

export default function App() {
  const [runId, setRunId] = useState(() => {
    const saved = localStorage.getItem("runId");
    return saved ? parseInt(saved) : null;
  });

  const [playerId, setPlayerId] = useState(() => {
    const saved = localStorage.getItem("playerId");
    return saved ? parseInt(saved) : null;
  });

  const [gameState, setGameState] = useState(() => {
    return localStorage.getItem("runId") ? "BATTLE" : "START";
  });

  // FUNÇÃO PARA RESETAR TUDO
  const handleReset = () => {
    localStorage.clear();
    setRunId(null);
    setPlayerId(null);
    setGameState("START");
  };

  const handleStartGame = (id, pId) => {
    localStorage.setItem("runId", id);
    localStorage.setItem("playerId", pId);
    setRunId(id);
    setPlayerId(pId);
    setGameState("BATTLE");
  };

  return (
    <div>
      {gameState === "START" && <StartScreen onStartGame={handleStartGame} />}
      {gameState === "BATTLE" && (
        <BattleArena 
          runId={runId} 
          playerId={playerId} 
          onReset={handleReset} // Passamos a função de reset para a Arena
        />
      )}
    </div>
  );
}