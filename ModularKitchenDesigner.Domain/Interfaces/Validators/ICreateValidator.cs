namespace ModularKitchenDesigner.Domain.Interfaces.Validators
{
    public interface ICreateValidator
    {
        List<TEntity> Validate<TEntity>(List<TEntity> models, string preffix = "", params string[] suffix)
            where TEntity : class;
    }
}
