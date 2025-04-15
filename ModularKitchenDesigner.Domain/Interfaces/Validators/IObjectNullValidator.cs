using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.Domain.Interfaces.Validators
{
    public interface IObjectNullValidator
    {
        TEntity Validate<TEntity>(TEntity model, string preffix = "", params string[] suffix)
             where TEntity : class;
    }
}
