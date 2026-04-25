import React, { useState, useEffect } from 'react';
import axios from 'axios';

export default function InventoryModal({ runId, onClose, onItemUsed }) {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Busca os itens assim que a mochila abre
  useEffect(() => {
    const fetchItems = async () => {
      try {
        setLoading(true);
        // Atenção ao 'I' maiúsculo em Inventory!
        const response = await axios.get(`http://localhost:5290/api/Inventory/${runId}`);
        setItems(response.data);
      } catch (err) {
        console.error(err);
        setError("Erro ao carregar a mochila.");
      } finally {
        setLoading(false);
      }
    };

    if (runId) {
      fetchItems();
    }
  }, [runId]);

  // Função disparada ao clicar em "Usar" ou "Equipar"
  const handleAction = async (itemId) => {
    try {
      // 1. Usa o item
      const response = await axios.post("http://localhost:5290/api/Inventory/use", { ItemId: itemId });
      
      // 2. Atualiza a vida e o bônus na tela da Arena
      onItemUsed(response.data);
      
      // 3. CORREÇÃO: Busca os itens novamente direto do banco para atualizar a mochila!
      const reloadResponse = await axios.get(`http://localhost:5290/api/Inventory/${runId}`);
      setItems(reloadResponse.data);
      
    } catch (err) {
      console.error("Erro ao usar item:", err);
      setError("Não foi possível usar este item.");
    }
  };

  return (
    <div style={styles.overlay}>
      <div style={styles.modal}>
        <button onClick={onClose} style={styles.closeBtn}>X</button>
        <h2 style={{ color: '#FFD700', borderBottom: '1px solid #FFD700', paddingBottom: '10px' }}>🎒 Sua Mochila</h2>
        
        {loading && <p>Procurando na mochila...</p>}
        {error && <p style={{ color: '#e63946' }}>{error}</p>}
        
        {!loading && items.length === 0 && (
          <p style={{ color: '#aaa', fontStyle: 'italic' }}>Sua mochila está vazia.</p>
        )}

        <div style={styles.itemList}>
          {items.map(item => (
            <div key={item.id} style={styles.itemCard}>
              <div>
                <strong style={{ color: item.type === "Ring" ? '#00b4d8' : '#2a9d8f', fontSize: '1.1rem' }}>
                  {item.name} {item.isEquipped ? "(Equipado)" : ""}
                </strong>
              </div>
              
              <button 
                onClick={() => handleAction(item.id)} 
                style={{
                  ...styles.actionBtn, 
                  backgroundColor: item.type === "Ring" && item.isEquipped ? '#e63946' : '#2a9d8f'
                }}
              >
                {item.type === "Ring" ? (item.isEquipped ? "Desequipar" : "Equipar") : "Usar"}
              </button>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}

const styles = {
  overlay: { position: 'absolute', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0,0,0,0.85)', display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 50 },
  modal: { backgroundColor: '#1a1a1a', padding: '20px', borderRadius: '10px', color: 'white', border: '2px solid #555', width: '400px', maxWidth: '90%', position: 'relative' },
  closeBtn: { position: 'absolute', top: '15px', right: '15px', backgroundColor: 'transparent', color: 'white', border: 'none', fontSize: '1.2rem', cursor: 'pointer', fontWeight: 'bold' },
  itemList: { maxHeight: '300px', overflowY: 'auto', marginTop: '15px' },
  itemCard: { display: 'flex', justifyContent: 'space-between', alignItems: 'center', backgroundColor: '#333', padding: '10px 15px', borderRadius: '5px', marginBottom: '10px', border: '1px solid #444' },
  actionBtn: { padding: '8px 15px', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' }
};