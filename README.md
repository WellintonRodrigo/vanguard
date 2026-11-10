# 🌾 Vanguard — Intelligence Center

Plataforma autônoma de inteligência de dados, análise de mercado e suporte à tomada de decisão para o agronegócio. 
*Projeto autoral desenvolvido com foco em Engenharia de Dados, arquitetura enxuta e Inteligência Preditiva.*

---

## 🚀 Visão Geral & Propósito
O **Vanguard** nasceu de uma provocação de estudo: *é possível prever o comportamento do mercado e antecipar riscos no agronegócio combinando dados abertos?*

A plataforma opera de forma **end-to-end** coletando, limpando e cruzando preços de commodities agrícolas com variáveis climáticas em tempo real. O objetivo central não é criar um painel poluído de gráficos, mas seguir a filosofia: **pouca informação, mas informação altamente relevante.**

---

## 🛠️ Stack Tecnológica & Arquitetura

### **Back-end (.NET 10 / C#)**
- **Arquitetura Base:** Separação limpa baseada em features e Domain-Driven Design adaptado (Domain, Application, Infrastructure, API, Worker).
- **Persistência:** **MongoDB Atlas** (Séries temporais e documentos flexíveis com custo zero / M0 Free Tier).
- **Worker Service (ETL Engine):** Serviço autônomo em segundo plano responsável pelo *scraping* resiliente e consumo de APIs externas.
- **APIs RESTful:** Endpoints otimizados.

### **Front-end (React 19, TypeScript & Tailwind CSS)**
- **Arquitetura de UI:** Interface executiva em modo escuro inspirada em HUDs minimalistas.
- **Performance e Caching:** Estratégia de carregamento otimizado (single-fetch no arranque) com gerenciamento de estado em memória para navegação sem latência (`0ms` em filtros por praça/commodity).

---

## ⚙️ Principais Módulos Implementados

1. **Dashboard Executivo (Core de Decisão):** 
   - Leitura visual imediata com variação diária/mensal automática (`+` / `-`).
   - Agrupamento inteligente por categorias (Soja, Milho, etc.).
2. **Worker & Ingestão Resiliente:** 
   - Coleta automatizada de fontes externas (como *Notícias Agrícolas*).
   - Sistema de verificação de frescor de dados e relatórios de integridade de coletores.
3. **Módulo de Clima & Motor Preditivo (Laboratório Atual):** 
   - Integração com a API pública **Open-Meteo** para monitoramento meteorológico da região piloto (Joinville / Sul do Brasil).
   - Camada de heurísticas que cruza pluviosidade e variação de preços para gerar probabilidades de impacto e insights acionáveis.

---

## 📊 Status do Roadmap

- [x] **Arquitetura Base & Conexão MongoDB Atlas**
- [x] **Worker Service de Ingestão de Commodities Operacional**
- [x] **Endpoints RESTful de Preços e Históricos**
- [x] **Dashboard de Commodities em React 19**
- [x] **Integração com API de Clima (Open-Meteo)**
- [x] **Camada de Heurísticas e Motor Preditivo**
- [ ] **Políticas de Retenção de Dados e Expurgo Automático (Backlog)**

---

## 🏁 Como Executar o Projeto

Para rodar o **Vanguard — Intelligence Center** localmente na sua máquina, siga os passos abaixo. O ambiente é dividido entre o **Back-end (.NET 10)** (com seu Worker Service de ingestão autônoma) e o **Front-end (React 19)**.

### 📋 Pré-requisitos
Certifique-se de ter instalado em seu ambiente:
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Node.js](https://nodejs.org/) (v18+ recomendada)
- Uma conta com cluster ativo no [MongoDB Atlas](https://www.mongodb.com/cloud/atlas) (Plano M0 Free Tier)

---

### 1️⃣ Configuração do Banco de Dados (MongoDB Atlas)
1. Crie um cluster gratuito no MongoDB Atlas.
2. Na aba **Network Access**, libere seu IP atual ou habilite acesso temporário para desenvolvimento (`0.0.0.0/0`).
3. Copie a sua *Connection String* de acesso ao banco.

---

### 2️⃣ Configuração de Variáveis de Ambiente
No arquivo `appsettings.json` (ou `appsettings.Development.json`) dos projetos `src/Vanguard.API` e `src/Vanguard.Worker`, configure a string de conexão:

```json
"ConnectionStrings": {
  "MongoDB": "sua-connection-string-do-mongodb-atlas"
}
No appsettings.json do Vanguard.Worker, configure as praças de monitoramento meteorológico (Open-Meteo):

JSON
"TargetRegions": [
  {
    "Location": "Joinville - SC",
    "Latitude": -26.3044,
    "Longitude": -48.8487
  }
]
```
### 3️⃣ Executando o Back-end e o Worker (.NET 10)
Você pode iniciar os projetos pelo Visual Studio (configurando Múltiplos Projetos de Inicialização) ou via terminal:

Iniciando a API RESTful:

Bash
`dotnet run --project src/Vanguard.API/Vanguard.API.csproj`
A API estará acessível via HTTPS e o Swagger estará disponível em https://localhost:7011/swagger.

Iniciando o Worker Service (Ingestão Autônoma):
```
Bash
dotnet run --project src/Vanguard.Worker/Vanguard.Worker.csproj
```
O Worker iniciará os ciclos de coleta contínua em segundo plano (commodities e clima).

### 4️⃣ Executando o Front-end (React 19 + TypeScript)
Navegue até a pasta do front-end:
```
Bash
cd src/Vanguard.Frontend
```
Instale as dependências:
```
Bash
npm install
```
Execute o servidor de desenvolvimento Vite:
```
Bash
npm run dev
```
Acesse a aplicação no seu navegador no endereço indicado pelo Vite `(geralmente http://localhost:5173).`
