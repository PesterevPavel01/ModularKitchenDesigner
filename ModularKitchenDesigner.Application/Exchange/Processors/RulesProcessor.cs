using Microsoft.Extensions.Options;
using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.Application.Exchange.Processors
{
    public class RulesProcessor
    {
        private readonly IOptions<ExchangeRules> _exchangeRules;

        public RulesProcessor(IOptions<ExchangeRules> exchangeRules)
        {
            _exchangeRules = exchangeRules;
        }

        public List<Rule> GetModelRules<TEntity>()
            where TEntity : BaseEntity
        {
            var exchangeRules = _exchangeRules.Value;

            var modelRules = exchangeRules.Models
                .Find(x => x.Title == typeof(TEntity).Name)?.Rule.ToList();

            if (modelRules == null || !modelRules.Any())
            {
                throw new InvalidOperationException($"{typeof(TEntity).Name} rules not found in configuration");
            }

            return modelRules;
        }

        public Dictionary<string,int> GetParametrizeComponentRules(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return new();

            var exchangeRules = _exchangeRules.Value.ParametrizeComponents;

            var paramters = _exchangeRules.Value.ParametrizeComponents
               .Find(x =>
                   x.Code == code)?.Parameters;

            return paramters;
        }
    }
}
