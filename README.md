# First_Game
Full-stack First_Game - RPG: A study on Software Architecture using C# (.NET) and React. Focused on POO principles, Design Patterns (Strategy), and Polymorphism to handle complex combat mechanics through JSON-based communication.

# Estruturação de Arquivos
- Usamos o DDD (Domain-Driven-Design)
- Domain: Pasta App com as Definições do Back-end
- Domain/Entities: É a criação dos modelos de como nossas entidades devem ser
- Domain/Interfaces: Define como os personagens FAZEM, sem dizer como, são contratos iniciais para cada entidade
- Domain/Enums: listas de opções (Tipo de elemento), evita o uso de strings
- Services: Osquestrador das entidades e lógica de negócios
- Controllers: Ele quem recebe as ações do player para avisar o Service e agir, nele que ficam as lógicas de um CRUD 

# Definição de Classes
Definimos em torno de 10 classes para o herói escolher e enfrentar todas essas 100 classes.

# Modo de Jogo
O jogador cria um Herói, escolhe uma classe para ele, enfrenta inimigos, sobe de níveis e recolhe itens dropados, enfrenta 10 inimigos e se vencer todos eles ele ganha.

# Estrutura Padrão no Front-end Criado pelo VITE
```
/frontend
├── /node_modules       <-- Onde ficam os códigos de terceiros (nunca mexa aqui)
├── /public             <-- Imagens estáticas, ícones (favicon) que não passam pelo código
├── /src                <-- O SEU REINO. 99% do seu trabalho será aqui.
│   ├── /assets         <-- Imagens e logos importados no código
│   ├── App.jsx         <-- O componente principal, a "casca" da sua tela
│   ├── main.jsx        <-- O ponto de entrada. Ele pega o App.jsx e joga no HTML.
│   └── index.css       <-- CSS global
├── index.html          <-- A única página HTML real do projeto
├── package.json        <-- A "identidade" do projeto e lista de dependências
└── vite.config.js      <-- Configurações do servidor Vite
```

# Estrutura como eu apliquei
```
/src
├── /components         <-- Pedacinhos de tela (Ex: BotaoAtaque.jsx, BarraDeVida.jsx)
├── /pages              <-- Telas inteiras (Ex: TelaBatalha.jsx, TelaSelecaoClasse.jsx)
├── /services           <-- Arquivos que conversam com o C# (Ex: api.js)
├── /utils              <-- Funções úteis e matemáticas (Ex: calcularPorcentagemVida.js)
```
