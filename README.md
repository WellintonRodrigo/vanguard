# 🌾 Vanguard — Intelligence Center

> Plataforma de inteligência de dados, análise de mercado e suporte à tomada de decisão para o agronegócio.  
> *Projeto autoral desenvolvido com foco em Arquitetura de Software Avançada, Engenharia de Dados e Inteligência Artificial.*

---

## 📌 Visão Geral & Propósito do Projeto

O **Vanguard** nasceu de uma provocação pessoal e de estudos: **é possível prever o comportamento do mercado e Antecipar riscos no agronegócio?** No início, a ambição era criar um sistema "onipresente" capaz de prever quase tudo — preços, tendências, clima e safras. 

À medida que o projeto evoluiu, a visão amadureceu para uma abordagem pragmática de produto: para prever o futuro com precisão, primeiro é preciso **dominar o presente com dados confiáveis**.

Hoje, o Vanguard é uma plataforma *end-to-end* em fase piloto focada no monitoramento integrado de **Commodities e Clima**. O sistema combina captura autônoma de dados, alta performance de apresentação e uma arquitetura em **DDD (Domain-Driven Design)** desenhada especificamente para suportar o próximo passo do projeto: a implementação de modelos de **IA para Detecção Preditiva de Anomalias Climáticas**.

---

## 🚀 Principais Funcionalidades

- 📊 **Dashboard Executivo de Alta Performance (O Core de Decisão):**
  - Painel central projetado para leitura visual imediata e análise rápida de cenários.
  - Exibição em tempo real do valor mais recente de cada commodity por mercado/praça, correlacionado com indicadores de variação diária (`+` / `-`) e tendências de curto prazo.
  - Agrupamento inteligente por categorias e métricas vitais (KPIs), permitindo comparar rapidamente **Soja**, **Milho**, **Arroz**, **Boi Gordo** e novos ativos em uma única visão unificada.

- 🔄 **Automação de Dados & Scraping Resiliente (Worker Engine):**
  - Serviço de segundo plano (*Background Worker*) $100\%$ autônomo, responsável pelo pipeline contínuo de extração, limpeza, transformação e carga (ETL).
  - Captura diária e sem intervenção humana de fontes externas de mercado e APIs meteorológicas.
  - Arquitetura resiliente tratada contra falhas de rede, re-tentativas e gravação consistente de séries temporais.

- ⚡ **Filtragem & Navegação Instantânea (0ms de Latência):**
  - Motor de processamento e filtragem em memória no front-end que permite alternar visões, mercados e commodities instantaneamente, sem *loading spinners* adicionais ou chamadas redundantes à API.

- 🌦️ **Módulo de Clima & Análise Ambiental (Piloto Ativo):**
  - Monitoramento de variáveis meteorológicas críticas que impactam diretamente a produtividade no campo e a formação de preços das commodities.

- 🤖 **Detector de Anomalias Climáticas (Laboratório de IA & Roadmap Preditivo):**
  - O próximo grande marco do projeto: aplicação de modelos de análise preditiva/machine learning para identificar desvios padrões em precipitação e temperatura, alertando antecipadamente sobre riscos de seca ou geadas que possam impactar a precificação do mercado.

---

## 🛠️ Arquitetura e Engenharia de Software

Desenvolvido para consolidar boas práticas de mercado, o Vanguard foi arquitetado sobre os princípios do **Domain-Driven Design (DDD)**, garantindo desacoplamento total entre as regras de negócio e os detalhes de infraestrutura.
---

## 🛠️ Arquitetura e Tecnologia

O Vanguard foi arquitetado sobre os princípios de **Domain-Driven Design (DDD)**, mantendo o domínio do negócio completamente isolado de detalhes de infraestrutura e garantindo testabilidade, evolução contínua e alta manutenibilidade.

---

### **Back-end (.NET 10 & C#) — Padrão DDD**
- **Domain Layer:** Regras de negócio puras, entidades e contratos de repositórios, totalmente desacoplados de frameworks.
- **Application Layer (Use Cases):** Casos de uso bem definidos orchestrando o fluxo das informações de commodities e clima.
- **Infrastructure Layer:** Implementações concretas de acesso ao **MongoDB Atlas**, clientes HTTP para scraping e conectores de dados externos.
- **Worker Service:** Ingestão contínua assíncrona operando sob os mesmos conceitos do domínio.

### **Front-end (React, TypeScript & Tailwind CSS)**
- **React 19 & TypeScript:** Tipagem rigorosa compartilhada conceitualmente com os contratos da API.
- **Client-Side Caching & Memory Management:** Estratégia de carregamento em bloco inicial para garantir trocas de estados sem *loading spinners* desnecessários.
---
*Vanguard — Um projeto de estudos focado em transformar dados brutos em decisões inteligentes.*
