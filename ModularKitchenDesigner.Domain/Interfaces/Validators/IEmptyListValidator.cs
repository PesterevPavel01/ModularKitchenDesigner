using System.Runtime.CompilerServices;

namespace ModularKitchenDesigner.Domain.Interfaces.Validators
{
    public interface IEmptyListValidator
    {
        public List<TEntity> Validate<TEntity>(List<TEntity> models, string preffix = "", [CallerMemberName] string methodName = null, params string[] suffix)
            where TEntity : class;
    }
}
