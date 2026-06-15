using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.DataCollector.Health.Enuns
{
    public enum CollectorHealthStatus
    {
        Unknown = 0, // default
        Healthy = 1, //Sucesso/SaudávelSucesso/Saudável
        Warning = 2,// Aviso/Alerta
        Critical = 3 // Crítico/Não saudável
    }
}
