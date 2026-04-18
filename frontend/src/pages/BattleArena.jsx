import React from 'react';
import bgImagem from '../assets/wallpaper_pc.png'; // Importando a sua imagem!
import StatusBox from '../components/StatusBox';

export default function BattleArena() {
  return (
    <div 
      className="arena-container" 
      style={{ backgroundImage: `url(${bgImagem})` }} // Aplicando no fundo
    >
      
      {/* Área Central: Herói vs Inimigo */}
      <div className="battlefield">
        <StatusBox 
          name="Aragorn" 
          life={150} lifeMax={150} 
          mana={30} manaMax={30} 
        />
        
        <StatusBox 
          name="Esqueleto Sombrio" 
          life={120} lifeMax={120} 
          mana={0} manaMax={0} 
        />
      </div>

      {/* Rodapé: Controles */}
      <div className="action-menu">
        <button className="btn-action">Attack</button>
        <button className="btn-action" style={{ backgroundColor: '#457B9D' }}>Magic</button>
        <button className="btn-action" style={{ backgroundColor: '#D4A373' }}>Potion</button>
      </div>

    </div>
  );
}