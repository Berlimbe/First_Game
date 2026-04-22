import React, { useState } from 'react';
import BattleArena from './pages/BattleArena';
import StartScreen from './pages/StartScreen';

function App() {
  // Estado para saber em qual tela estamos
  const [currentScreen, setCurrentScreen] = useState("start"); 
  
  // Guardamos os IDs que o banco de dados vai gerar para usarmos na batalha depois!
  const [runId, setRunId] = useState(null);
  const [playerId, setPlayerId] = useState(null);

  // Função que a StartScreen vai chamar quando o C# der o OK
  const handleGameStarted = (newRunId, newPlayerId) => {
    setRunId(newRunId);
    setPlayerId(newPlayerId);
    setCurrentScreen("battle"); // Troca a tela instantaneamente!
  };

  return (
    <div>
      {/* Sistema clássico de roteamento manual */}
      {currentScreen === "start" && <StartScreen onStartGame={handleGameStarted} />}
      
      {/* Depois enviaremos o runId para a Arena para ela saber quem está lutando */}
      {currentScreen === "battle" && <BattleArena runId={runId} playerId={playerId} />}
    </div>
  );
}

export default App;