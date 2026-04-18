import React from 'react';

// Um componente reutilizável que recebe "props" (propriedades)
export default function StatusBox({ name, life, lifeMax, mana, manaMax }) {
  return (
    <div className="status-box">
      <h2>{name}</h2>
      
      <div style={{ marginBottom: '10px' }}>
        <p style={{ margin: '0' }}>HP: {life} / {lifeMax}</p>
        {/* Barra visual de vida (Vermelha) */}
        <div style={{ width: '100%', height: '15px', backgroundColor: '#555', borderRadius: '5px' }}>
          <div style={{ width: '100%', height: '100%', backgroundColor: '#E63946', borderRadius: '5px' }}></div>
        </div>
      </div>

      <div>
        <p style={{ margin: '0' }}>MP: {mana} / {manaMax}</p>
        {/* Barra visual de mana (Azul) */}
        <div style={{ width: '100%', height: '15px', backgroundColor: '#555', borderRadius: '5px' }}>
          <div style={{ width: '100%', height: '100%', backgroundColor: '#457B9D', borderRadius: '5px' }}></div>
        </div>
      </div>
    </div>
  );
}