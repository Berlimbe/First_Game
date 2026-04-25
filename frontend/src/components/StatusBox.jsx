export default function StatusBox({ name, level, className, currentHp, maxHp }) {
  // Se o maxHp vier errado ou nulo, a barra quebra. Usamos um fallback (|| 1)
  const safeMaxHp = maxHp || currentHp || 1;
  const hpPercent = Math.max(0, Math.min(100, (currentHp / safeMaxHp) * 100));

  return (
    <div className="status-box" style={styles.box}>
      <h3 style={{ margin: '0 0 5px 0' }}>{name}</h3>
      
      {/* Exibindo Nível e Classe juntos e organizados */}
      <div style={{ fontSize: '0.85rem', color: '#aaa', margin: '0 0 10px 0' }}>
        {level && <div style={{ marginBottom: '2px' }}>Nv. {level}</div>}
        {className && <div>{className}</div>}
      </div>
      
      <div style={{ textAlign: 'center', marginBottom: '5px', fontSize: '0.9rem' }}>
        HP: {currentHp} / {safeMaxHp}
      </div>
      
      <div style={styles.barBg}>
        <div style={{ 
          width: `${hpPercent}%`, 
          backgroundColor: '#e63946', 
          height: '100%', 
          transition: 'width 0.5s ease-in-out' 
        }}></div>
      </div>
    </div>
  );
}

const styles = {
  box: { border: '2px solid #444', padding: '15px', borderRadius: '8px', backgroundColor: 'rgba(0,0,0,0.9)', color: 'white', width: '220px', textAlign: 'center' },
  barBg: { width: '100%', backgroundColor: '#333', height: '12px', borderRadius: '10px', overflow: 'hidden', border: '1px solid #555' }
};