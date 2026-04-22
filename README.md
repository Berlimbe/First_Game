# ⚔️ First Game RPG - Full-Stack Dark Fantasy

Um projeto Full-Stack focado no estudo profundo de **Arquitetura de Software**, construído com C# (.NET 8) e React (Vite). O objetivo principal é aplicar princípios de **POO (Programação Orientada a Objetos)**, **Design Patterns (Strategy, Factory)** e **DDD (Domain-Driven Design)** para gerenciar mecânicas complexas de combate em turnos através de comunicação RESTful (JSON).

## 🎮 Modo de Jogo (Game Flow)
O jogador cria um Herói, escolhe entre 11 classes disponíveis e entra em uma masmorra estilo *Roguelike/Boss Rush*. O herói enfrenta inimigos gerados proceduralmente (com nomes e classes aleatórias que afetam seus status base). Ao chegar no 11º andar, o jogador enfrenta um Boss colossal, cujos atributos e narrativa (Lore) são gerados pela fusão de 3 classes distintas.

---

## 🏗️ Estruturação da Arquitetura Back-end (C# / .NET)
O Back-end foi desenhado utilizando os princípios do **Domain-Driven Design (DDD)**, garantindo que as regras de negócio fiquem isoladas e independentes do banco de dados ou da internet.

* **`Domain` (O Coração do Sistema):**
  * `Entities:` Modelos e comportamentos reais dos personagens (ex: Mago, Guerreiro, Boss).
  * `Interfaces:` Contratos que definem *o que* as entidades fazem (ex: `ITakeDamage`), sem se importar com *como* fazem.
  * `Enums:` Tipagens fortes (ex: `ControlType.AI`) para evitar o uso de "Magic Strings" e prevenir bugs de digitação.
* **`Services`:** Os orquestradores. Eles pegam as entidades do Domínio e aplicam a lógica de combate (Cálculos de dano, turnos).
* **`Controllers`:** Os porteiros da API. Eles recebem os DTOs (Data Transfer Objects) do React via requisição HTTP, acionam os Services e salvam os resultados no banco de dados usando o Entity Framework.

---

## 💻 Estruturação do Front-end (React + Vite)
O Front-end foi organizado para máxima reutilização de código e separação de responsabilidades (UI vs. Lógica de Rede):

```
/frontend
├── /node_modules       <-- Dependências gerenciadas pelo NPM
├── /public             <-- Assets estáticos que não passam pelo build (ex: favicon)
├── /src                <-- Código-fonte principal da aplicação
│   ├── /assets         <-- Imagens, backgrounds e ícones
│   ├── /components     <-- Componentes reutilizáveis (Ex: StatusBox.jsx, ActionButton.jsx)
│   ├── /pages          <-- Telas e rotas (Ex: StartScreen.jsx, BattleArena.jsx)
│   ├── /services       <-- Centralização de chamadas do Axios para a API (C#)
│   ├── /utils          <-- Funções puras e cálculos auxiliares
│   ├── App.jsx         <-- Roteador e gerenciador de estado global
│   ├── main.jsx        <-- Ponto de montagem do React no DOM
│   └── index.css       <-- Estilização global Dark Fantasy
├── index.html          <-- Template base
└── package.json        <-- Configurações e scripts do projeto
```

## Lições Aprendidas e Refatorações Técnicas
Durante o desenvolvimento, algumas decisões arquiteturais foram tomadas para melhorar a qualidade do código:
- No Fall-Through no Switch/Case: O compilador rigoroso do C# exige o uso explícito de break; em cases com lógica. Isso evitou bugs silenciosos onde uma classe poderia herdar status não intencionais de outra (ex: Marksman "caindo" no Necromancer).
- Aplicação da Regra DRY (Don't Repeat Yourself): Inicialmente, a lógica de instanciar classes estava duplicada nas rotas de criação e resgate do banco. O código foi refatorado utilizando o padrão Factory Method (CriarEntidade),centralizando a criação e deixando os Controllers limpos.
- Uso de Tuplas (C# Tuples): Em vez de criar classes temporárias (DTOs) descartáveis apenas para transitar múltiplos retornos (ex: a Lore do Boss que retorna a História, Classe Atual e Classe do Passado), foi adotado o uso de Tuplas, garantindo alta performance e um código mais elegante.

## Roadmap e Bugs Conhecidos (Próximos Passos)
- [ ] UI do Alerta do Boss: Substituir o alert() nativo do navegador por um Modal/Toast estilizado no React, evitando a quebra de imersão.

- [ ] Renderização de Status: Corrigir a passagem de props para o componente StatusBox.jsx para que o HP atual e máximo do Jogador/Inimigo sejam exibidos corretamente na tela.

- [ ] Implementação do Strategy Pattern: Criar a mecânica do botão "Atacar", onde o C# define a resposta do Inimigo baseando-se em estratégias dinâmicas atreladas à porcentagem de HP restante.

##