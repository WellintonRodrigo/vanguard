using Vanguard.Application.Features.Commodities.DTOs;
using Vanguard.Application.Features.Insights;
using Vanguard.Domain.Enums;

namespace Vanguard.Application.Features.Commodities.Services
{
    public class DataFreshnessService
    {
        private readonly DataFreshnessInsightTemplateService _insightTemplatService;

        public DataFreshnessService(
            DataFreshnessInsightTemplateService insightTemplatService)
        {
            _insightTemplatService = insightTemplatService;
        }

        public DataFreshnessDto Calculate(DateTime referenceDate)
        {
            var today = DateTime.UtcNow.Date;
            var reference = referenceDate.Date;
            var daysWithoutUpdate = (today - referenceDate).Days;

            var status = daysWithoutUpdate switch
            {
                <= 1 => DataFreshnessStatus.Healthy,
                <= 7 => DataFreshnessStatus.Warning,
                _ => DataFreshnessStatus.Critical
            };
            return new DataFreshnessDto
            {
                Status = status,
                DaysWithoutUpdate = daysWithoutUpdate,
                Message = _insightTemplatService.Generate(status, daysWithoutUpdate)
            };

        }
    }
}
