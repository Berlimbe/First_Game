# ⚔️ First Game RPG - Full-Stack Dark Fantasy

Um projeto Full-Stack focado no estudo profundo de **Arquitetura de Software**, construído com C# (.NET 8) e React (Vite). O objetivo principal é aplicar princípios de **POO (Programação Orientada a Objetos)**, **Design Patterns (Strategy, Factory)** e **DDD (Domain-Driven Design)** para gerenciar mecânicas complexas de combate em turnos, persistência de dados e progressão de RPG através de comunicação RESTful (JSON).

## 📊 Nível de Desenvolvimento
**Status atual:** 🟢 Alpha / Core Game Loop Funcional
A fundação bruta do jogo (Back-end, Banco de Dados, API e Front-end) está construída e interligada. O ciclo de jogo (Iniciar Partida ➔ Batalhar ➔ Subir de Nível ➔ Coletar Loot ➔ Avançar Andar) funciona perfeitamente de ponta a ponta.

## 🛠️ Ferramentas e Tecnologias
* **Back-end:** C# 12, .NET 8, ASP.NET Core Web API.
* **Banco de Dados:** SQLite e Entity Framework Core (Code-First & Migrations).
* **Front-end:** React 18, Vite, Axios (para requisições HTTP).
* **Arquitetura & Patterns:** Domain-Driven Design (DDD), Strategy Pattern, Factory Method.

## 🎮 Modo de Jogo (O que fizemos até o momento)
O jogador cria um Herói, escolhe entre 11 classes disponíveis e entra em uma masmorra infinita estilo *Roguelike*. As mecânicas atuais incluem:
* **Progressão Infinita e Escalável:** Inimigos normais ficam mais fortes a cada andar concluído. 
* **Boss Rush:** A cada 10 andares, o jogador enfrenta um "Lorde" com atributos massivos e habilidades únicas.
* **Sistema de Level Up:** XP ganho por derrotar inimigos. Ao atingir 100 de XP, o jogador sobe de nível, recuperando HP e ganhando status permanentes (Poder Base e Defesa).
* **Sistema de Inventário (Loot):** Sorteios (RNG) de itens ao vencer batalhas. O jogador pode encontrar poções de cura, elixires de dano, amuletos da sorte (que geram outros itens), amuletos amaldiçoados e anéis equipáveis que concedem buffs passivos percentuais.
* **Persistência de Sessão:** O uso de `localStorage` em conjunto com o Banco de Dados permite que o jogador recarregue a página (F5) ou feche o navegador sem perder a sua "Run" atual.

---

## 🏗️ Estruturação da Arquitetura Back-end (C# / .NET)
O Back-end foi desenhado utilizando os princípios do **Domain-Driven Design (DDD)**, garantindo que as regras de negócio fiquem isoladas.
* **`Domain`:** Entidades (Mago, Guerreiro), Interfaces (`IAttackStrategy`) e Enums.
* **`Models`:** Espelhos das tabelas do banco de dados (ex: `RunModel`, `ItemModel`).
* **`Controllers`:** Os porteiros da API (ex: `BattleController`, `InventoryController`). Recebem os DTOs, aplicam as estratégias de combate, atualizam os itens e salvam no banco.

---

## 💻 Estruturação do Front-end (React + Vite)
O Front-end foi organizado para máxima reutilização de código e separação de responsabilidades (UI vs. Lógica de Rede):

```
/frontend
├── /src
│   ├── /components    <-- Componentes reutilizáveis (StatusBox.jsx, InventoryModal.jsx)
│   ├── /pages         <-- Telas principais (StartScreen.jsx, BattleArena.jsx)
│   ├── App.jsx        <-- Roteador, Lazy Initialization e gerenciamento do localStorage
│   └── index.css      <-- Estilização global Dark Fantasy
```

## Lições Aprendidas e Principais Dificuldades
Construir um ecossistema onde o Front e o Back precisam concordar matematicamente gerou excelentes desafios técnicos:
- Sincronização de Estado (React vs C#): A maior dificuldade inicial foi garantir que a interface reagisse exatamente aos cálculos do servidor. O clássico bug do "jogador curar ao ser atacado" (devido a status de Defesa gerarem dano negativo) e a renderização incorreta de XP exigiram uma arquitetura de resposta (Response Objects) mais rígida e sincronizada.
- O Desafio do useEffect: Lidar com renderizações em cascata e requisições fantasmas. Resolvemos isso utilizando Lazy Initialization nos estados principais (useState) para ler o localStorage de forma otimizada.
- O Poder do Strategy Pattern: Substituir uma teia de If/Else por Interfaces. O combate agora utiliza o IAttackStrategy como um "chip" plugável: o controlador não precisa saber qual é a classe do jogador, ele apenas executa o .CalculateDamage() da estratégia equipada. Isso facilitou enormemente a adição de bônus de inventário (Poções e Anéis).
- Migrações On-The-Fly (Entity Framework): Aprender a alterar a estrutura de um banco de dados relacional em produção (adicionando colunas de Nível, XP e criando a tabela de Itens) sem quebrar o código existente.

## Próximos passos
- [ ] Animações e Game Feel: Adicionar screen shake (tremor de tela), números de dano subindo e transições suaves nas barras de HP usando CSS ou bibliotecas de animação.
- [ ] Sistema de Mana e Habilidades Dinâmicas: Reativar a barra de MP e criar custos reais para as habilidades especiais de cada classe.
- [ ] Feedback Sonoro: Adicionar efeitos de áudio (SFX) para ataques, uso de poções, level up e trilha sonora de tensão contra os Chefões.