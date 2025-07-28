namespace ModularKitchenDesigner.Application.Exchange
{
    public class ExchangeRules
    {
        public List<ExchangeModel> Models { get; set; }
        public List<ParametrizeComponent> ParametrizeComponents { get; set; }
    }

    public class ExchangeModel
    {
        public string Title { get; set; }
        public List<Rule> Rule { get; set; } = [];
    }

    public class Rule
    {
        public int Parent { get; set; }
        public string Code { get; set; }
        public int Limit { get; set; }
        public bool Models {  get; set; }
        public bool Folder { get; set; }
    }

    public class ParametrizeComponent
    {
        public string Code { get; set; }
        public Dictionary<string, int> Parameters { get; set; }
    }
}
