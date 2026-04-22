import React from 'react';

// 1. Ele PRECISA receber as props exatas que você digitou na BattleArena!
export default function StatusBox({ name, currentHp, maxHp, currentMp, maxMp }) {
  
  // 2. Cálculo simples da porcentagem para a barra verde não vazar da caixa
  const hpPercentage = Math.max(0, (currentHp / maxHp) * 100);

  return (
    <div className="status-box">
      {/* 3. Renderizamos o Nome (que agora já vem com a Classe junto) */}
      <h3>{name}</h3>
      
      {/* 4. Textos de HP e MP reais */}
      <p>HP: {currentHp} / {maxHp}</p>
      
      <div className="bar-bg">
        {/* 5. A barra preenche de acordo com a porcentagem calculada */}
        <div className="bar-fill hp" style={{ width: `${hpPercentage}%` }}></div>
      </div>
      
      <p>MP: {currentMp} / {maxMp}</p>
      <div className="bar-bg">
        {/* O MP por enquanto deixamos fixo se quiser, ou fazemos o mesmo cálculo */}
        <div className="bar-fill mp" style={{ width: '100%' }}></div>
      </div>
    </div>
  );
}