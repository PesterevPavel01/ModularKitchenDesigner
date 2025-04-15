namespace ModularKitchenDesigner.Domain.Interfaces.Validators
{
    public interface IEmptyListValidator
    {
        public List<TEntity> Validate<TEntity>(List<TEntity> models, string preffix = "", params string[] suffix)
            where TEntity : class;
    }
}
