using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.DataCollector.Health.Enuns
{
    public enum CollectorHealthStatus
    {
        Healthy = 1,// Sucesso/Saudável
        Warning = 2,// Aviso/Alerta
        Unhealthy = 3// Crítico/Não saudável
    }
}
