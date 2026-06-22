import axios from "axios";

export const apiClient = axios.create({
  // Ajuste a porta conforme a URL real do seu backend .NET
  baseURL: "https://localhost:7011",
  timeout: 10000,
});