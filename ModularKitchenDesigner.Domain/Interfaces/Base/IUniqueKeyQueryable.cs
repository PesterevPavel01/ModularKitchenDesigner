namespace ModularKitchenDesigner.Domain.Interfaces.Base
{
    public interface IUniqueKeyQueryable<TDto>
    {
        Func<TDto, List<TDto>, bool> IsElementInInputModels { get; set; }
    }
}
