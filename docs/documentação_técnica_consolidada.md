# 🌾 Vanguard — Intelligence Center (Documentação Técnica Consolidada)

> Documentação técnica completa e unificada da plataforma Vanguard: Arquitetura do Sistema, Motor Preditivo, Referência de Endpoints REST e Product Roadmap.

---

## 📑 Sumário
1. [🏗️ Arquitetura do Sistema](#1-arquitetura-do-sistema)
2. [🧠 Motor Preditivo & Camada de Heurísticas (Prediction Engine)](#2-motor-preditivo--camada-de-heurísticas-prediction-engine)
3. [🌐 API Endpoints Reference](#3-api-endpoints-reference)
4. [🗺️ Product Roadmap](#4-product-roadmap)

---

## 1. 🏗️ Arquitetura do Sistema

### 📌 Visão Geral da Arquitetura

O **Vanguard** foi arquitetado sob os princípios de **Domain-Driven Design (DDD)** adaptado e **Vertical Slice / Feature-Based**, priorizando baixo acoplamento, alta manutenibilidade e custo zero de infraestrutura. 

A aplicação opera como um **Monólito Modular** composto por uma API RESTful em .NET 10, um Worker Service autônomo para ETL, persistência NoSQL no MongoDB Atlas e uma interface SPA minimalista desenvolvida em React 19.

---

### 📐 Diagrama de Componentes e Fluxo de Dados

```
┌────────────────────────────────────────────────────────────────────────┐
│                        Vanguard.UI (React 19)                          │
│        (HUD Executivo, Single-Fetch, In-Memory Caching)               │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ HTTP / REST (Axios)
┌───────────────────────────────────▼────────────────────────────────────┐
│                       Vanguard.API (.NET 10)                           │
│  ┌──────────────────────────────────────────────────────────────────┐  │
│  │ Domain Layer (Entities, Value Objects, Repository Interfaces)    │  │
│  └────────────────────────────────▲─────────────────────────────────┘  │
│                                   │                                    │
│  ┌────────────────────────────────┴─────────────────────────────────┐  │
│  │ Application Layer (Use Cases, Features, DTOs, PredictionEngine)  │  │
│  └────────────────────────────────▲─────────────────────────────────┘  │
│                                   │                                    │
│  ┌────────────────────────────────┴─────────────────────────────────┐  │
│  │ Infrastructure Layer (MongoDB Atlas, Open-Meteo, Scraping)       │  │
│  └──────────────────────────────────────────────────────────────────┘  │
└───────────────────────────────────▲────────────────────────────────────┘
                                    │
┌───────────────────────────────────┴────────────────────────────────────┐
│                    Vanguard.Worker (Background ETL)                    │
│      (Ingestão Contínua de Commodities e Métricas de Frescor)          │
└────────────────────────────────────────────────────────────────────────┘
```

---

### 🧱 Divisão dos Projetos no Backend (.NET 10)

1. **`Vanguard.Domain`**:
   - Contém a lógica de negócio pura, entidades (`CommodityPrice`, `WeatherLog`, `Prediction`), enums e os contratos dos repositórios (`ICommodityPriceRepository`, `IWeatherRepository`, `IPredictionRepository`).
   - Livre de qualquer dependência externa de frameworks ou bancos de dados.

2. **`Vanguard.Application`**:
   - Casos de uso da aplicação organizados por features (`Commodities`, `Weather`, `Predictions`).
   - Contém os DTOs de entrada/saída, validações e o **`PredictionEngineService`** (motor heurístico preditivo).
   - `InsightTemplateService`: Gerencia o carregamento dinâmico de templates explicativos via arquivo JSON (`agriculture-insights.json`).

3. **`Vanguard.Infrastructure`**:
   - Conexão e contexto NoSQL via **`MongoDbContext`** operando no MongoDB Atlas.
   - Implementação dos repositórios NoSQL.
   - Provedores externos (`OpenMeteoWeatherProvider`, Web Scraping de Commodities).

4. **`Vanguard.Worker`**:
   - Serviço em segundo plano (`BackgroundService`) focado na execução resiliente de tarefas de ETL.
   - Monitora o frescor dos dados (`DataFreshnessService`) e a saúde dos coletores.

5. **`Vanguard.API`**:
   - Camada Web expondo RESTful Controllers com Swashbuckle/Swagger.
   - Registro de Injeção de Dependências e políticas de CORS para o React 19.

---

### 💻 Arquitetura do Frontend (React 19 + TypeScript)

- **Estética HUD (Heads-Up Display):** Interface executiva em Dark Mode com foco em leitura rápida.
- **Single-Fetch & Memory Caching:** Os dados de histórico e cotações são buscados no arranque inicial e filtrados em memória via `useMemo`, garantindo latência zero (`0ms`) ao trocar de commodities ou praças.

---

## 2. 🧠 Motor Preditivo & Camada de Heurísticas (Prediction Engine)

### 📌 Visão Geral

O **Prediction Engine** é o núcleo de inteligência do **Vanguard**. Como o sistema opera em fase inicial acumulando histórico de dados, a inteligência preditiva adota uma **Heurística de Matriz Combinada de Risco (Explainable AI / Bayesian Heuristic)** em vez de modelos pesados de Machine Learning.

O motor cruza variáveis **meteorológicas em tempo real** com a **volatilidade recente do mercado agrícola**, calculando uma probabilidade de impacto ($0\%$ a $100\%$) e gerando diagnósticos em linguagem natural.

---

### 📐 Matriz Combinada de Risco

A pontuação de risco ($\text{Score}$) é calculada através da ponderação de três fatores:

$$\text{Risco Total} = (W_{\text{clima}} \times S_{\text{clima}}) + (W_{\text{volatilidade}} \times S_{\text{volatilidade}}) + (W_{\text{eventos}} \times S_{\text{eventos}})$$

Onde:
- **$W_{\text{clima}} = 40\%$**: Acumulado de chuva de 7 dias e registro de estiagem/seca.
- **$W_{\text{volatilidade}} = 35\%$**: Variação percentual diária/mensal da commodity no mercado.
- **$W_{\text{eventos}} = 25\%$**: Frequência de dias críticos (ex: precipitação $\ge 20\text{mm}$ em 24h).

---

### ⚙️ Regras Heurísticas Implementadas

1. **Estiagem Severa (Drought Risk):**
   - **Gatilho:** Acumulado pluviométrico $< 10\text{mm}$ em 7 dias + Variação da commodity $> +1.5\%$.
   - **Resultado:** Probabilidade de Impacto $\approx 75\% - 85\%$.
   - **Classificação:** Risco Alto / Crítico.

2. **Excesso de Precipitação (Heavy Rain):**
   - **Gatilho:** Precipitação $\ge 30\text{mm}$ em 24h ou $\ge 3$ dias críticos no período.
   - **Resultado:** Alerta logístico e de safra com probabilidade $\ge 70\%$.

3. **Cenário Normal / Estável:**
   - **Gatilho:** Precipitação entre $15\text{mm}$ e $45\text{mm}$ semanal + variação de preços $< 1.0\%$.
   - **Resultado:** Probabilidade $\le 30\%$, tendência `stable`.

---

### 💬 Insight Engine (Explainable AI)

O **`InsightTemplateService`** desacopla as frases explicativas do código C#. Os textos ficam armazenados em `Resources/Insights/agriculture-insights.json` com placeholders dinâmicos:

```json
{
  "droughtRisk": [
    "Aviso de estiagem em {location}: baixo volume pluviométrico ({rain}mm) pressiona a oferta regional de {product}."
  ],
  "heavyRain": [
    "Alerta de tempestade em {location}: precipitação de {rain}mm combinada com flutuação de {variation}% no preço de {product} sinaliza risco logístico."
  ],
  "stable": [
    "Cenário operacional equilibrado em {location}: clima ({rain}mm) e cotação de {product} operam dentro da normalidade estatística."
  ]
}
```

O engine preenche `{product}`, `{location}`, `{rain}` e `{variation}` no momento da requisição, retornando a justificativa ao usuário.

---

## 3. 🌐 API Endpoints Reference

Base URL: `http://localhost:7011/api` ou `https://localhost:7147/api`

---

### 3.1 📊 Collector & Health (`/collector`)

Monitora a saúde e integridade dos coletores de background (Worker Service).

#### `GET /collector/health`
Retorna o status simplificado dos coletores.
- **Response 200 OK**:
  ```json
  [
    {
      "source": "NoticiasAgricolas",
      "status": "Healthy",
      "lastCheckAt": "2026-11-10T14:30:00Z"
    }
  ]
  ```

#### `GET /collector/summary`
Retorna métricas consolidadas de uptime e tempo de resposta.
- **Response 200 OK**:
  ```json
  [
    {
      "source": "NoticiasAgricolas",
      "totalChecks": 100,
      "healthyChecks": 98,
      "warningChecks": 2,
      "criticalChecks": 0,
      "uptimePercent": 98.0,
      "averageResponseTimeMs": 85
    }
  ]
  ```

---

### 3.2 🌾 Commodity Prices (`/commodity-prices`)

Gerencia a leitura e histórico de cotações das commodities agrícolas.

#### `GET /commodity-prices/latest`
Retorna o preço mais recente coletado para cada commodity/praça.

#### `GET /commodity-prices/history`
Retorna o histórico de preços. Aceita filtro opcional por commodity via Query String.
- **Params**: `?commodity=Soja` (Opcional)
- **Response 200 OK**:
  ```json
  [
    {
      "id": "6730cd12a...",
      "source": "NoticiasAgricolas",
      "commodity": "Soja",
      "market": "Esalq/B3 - Paraná",
      "unit": "Saca 60kg",
      "priceBrl": 123.91,
      "dailyVariationPercent": 0.81,
      "monthlyVariationPercent": -1.20,
      "referenceDate": "2026-11-10T00:00:00Z",
      "collectedAt": "2026-11-10T12:00:00Z"
    }
  ]
  ```

---

### 3.3 🧠 Predictions (`/predictions`)

Interface com o **Prediction Engine** e camada preditiva.

#### `POST /predictions/analyze`
Executa uma análise preditiva em tempo real combinando clima da região e cotação da commodity.
- **Request Body**:
  ```json
  {
    "product": "Soja",
    "location": "Joinville - SC",
    "latitude": -26.3044,
    "longitude": -48.8487
  }
  ```
- **Response 200 OK**:
  ```json
  {
    "product": "Soja",
    "location": "Joinville - SC",
    "impactProbability": 30.0,
    "trend": "stable",
    "estimatedVariationPercent": 1.0,
    "insight": "Baixo volume de chuva previsto indica risco climático reduzido para Soja em Joinville - SC.",
    "predictionDate": "2026-11-10T15:20:00Z"
  }
  ```

#### `GET /predictions`
Retorna o histórico de análises preditivas salvas no MongoDB.

#### `GET /predictions/product/{productName}`
Filtra o histórico de predições gravadas por produto específico (ex: `Soja`, `Milho`).

---

## 4. 🗺️ Product Roadmap

### 🎯 Fase 1: Fundação & Core de Mercado (Concluída)
- [x] Estrutura da solução em **.NET 10 (C#)** e **MongoDB Atlas Free Tier (M0)**.
- [x] **Vanguard.Worker**: Serviço autônomo de ETL para *scraping* resiliente do Notícias Agrícolas.
- [x] Endpoints REST de Saúde do Coletor (`/collector/health`, `/collector/summary`).
- [x] Interface HUD em **React 19 + Tailwind CSS** com suporte a filtros locais com **0ms de latência**.

---

### ☀️ Fase 2: Módulo Climático & Cérebro Preditivo (Concluída)
- [x] Conexão com a API **Open-Meteo** para clima atual e forecast de 7 dias.
- [x] Implementação do **`PredictionEngineService`** com a Heurística de Matriz Combinada de Risco.
- [x] **`InsightTemplateService`**: Gerador de análises em linguagem natural (Explainable AI / "JARVIS style").
- [x] Endpoints REST de análise preditiva (`POST /predictions/analyze` e `GET /predictions/product/{productName}`).

---

### ⚡ Fase 3: Aprofundamento do Frontend & Governança (Sprint Atual)
- [x] Integração da aba de análise preditiva no Dashboard React 19.
- [X ] **Horizonte de 7 dias no HUD:** Exibição do mini-gráfico/linha temporal com acumulado de chuvas e dias críticos.
- [X ] **Indicadores de Confiabilidade:** Badges visuais de frescor de dados por fonte.
- [ ] **Estratégia de Agregação / Data Retention:** Job no Worker para consolidar séries temporais antigas em médias diárias, preservando histórico de treino para IA sem estourar o limite de 512MB do MongoDB Atlas.

---

### 🔬 Fase 4: Inteligência Preditiva Avançada & ML (Backlog Futuro)
- [ ] Acúmulo de séries temporais históricas no MongoDB para treinamento de modelos estatísticos.
- [ ] Integração com **ML.NET** ou modelos Python externos para regressão e detecção automática de anomalias climáticas.
- [ ] Módulo de **Auto-Heal (Auto-Cura de Coletores)** com agentes LLM para re-mapeamento automático de seletores CSS quando a fonte alterar a estrutura.
- [ ] Expansão para novos domínios (Energia, Logística, Supply Chain).

---
*Vanguard — Um projeto de estudos focado em transformar dados brutos em decisões inteligentes.*