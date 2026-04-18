using First_Game.backend.Services;

var builder = WebApplication.CreateBuilder(args);

//Configuração do SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. CONFIGURANDO O CORS (O Porteiro)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // A porta padrão do Vite/React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Adicionando suporte a Controllers
builder.Services.AddControllers();

// Injeção de Dependência: Ensina o C# a entregar o BattleService
builder.Services.AddScoped<BattleService>();

var app = builder.Build();

// Configurando o Pipeline (O que acontece quando o servidor roda)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. ATIVANDO O CORS
app.UseCors("PermitirReact");

// Ativando as rotas dos Controllers
app.MapControllers();

//Inicia o servidor para escutar requisições web
app.Run();