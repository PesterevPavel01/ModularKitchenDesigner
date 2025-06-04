namespace ModularKitchenDesigner.Domain.Interfaces.Base
{
    public interface IUniqueKeyQueryable<TDto>
        where TDto : class
    {
        bool HasMatchingUniqueKey(IEnumerable<TDto> models);

        //Func<TDto, List<TDto>, bool> IsElementInInputModels { get; set; }
    }
}
