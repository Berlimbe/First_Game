import React from 'react';

export default function BossModal({ message, onAcknowledge }) {
  return (
    <div style={modalOverlayStyle}>
      <div style={modalContentStyle}>
        <h1 style={{ color: '#FFD700', textShadow: '0 0 10px #FF8C00' }}>⚠️ BOSS AWAKENED ⚠️</h1>
        <p style={{ fontSize: '1.2rem', margin: '20px 0' }}>{message}</p>
        <button 
          onClick={onAcknowledge} 
          style={buttonStyle}
        >
          Entrar na Arena
        </button>
      </div>
    </div>
  );
}

// Estilos direto no código para não precisarmos de mais CSS
const modalOverlayStyle = {
  position: 'fixed',
  top: 0, left: 0, right: 0, bottom: 0,
  backgroundColor: 'rgba(0,0,0,0.9)', // Quase preto total
  display: 'flex',
  justifyContent: 'center',
  alignItems: 'center',
  zIndex: 9999 // Fica por cima de tudo
};

const modalContentStyle = {
  backgroundColor: '#1a1a1a',
  padding: '40px',
  borderRadius: '10px',
  border: '2px solid #FFD700', // Borda dourada
  textAlign: 'center',
  color: 'white',
  maxWidth: '500px'
};

const buttonStyle = {
  backgroundColor: '#FFD700',
  color: 'black',
  fontWeight: 'bold',
  padding: '10px 20px',
  border: 'none',
  borderRadius: '5px',
  cursor: 'pointer',
  fontSize: '1rem',
  marginTop: '10px'
};