using System.Runtime.CompilerServices;

namespace ModularKitchenDesigner.Domain.Interfaces.Validators
{
    public interface IEmptyListValidator
    {
        public List<TEntity> Validate<TEntity, TArgument>(List<TEntity> models, TArgument methodArgument, String callerObject = null, [CallerMemberName] string methodName = null)
            where TEntity : class;
    }
}
