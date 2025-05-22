using System.Data.SqlTypes;
using System.Runtime.CompilerServices;

namespace ModularKitchenDesigner.Domain.Interfaces.Validators
{
    public interface ICreateValidator
    {
        List<TEntity> Validate<TEntity, TArgument>(List<TEntity> models, TArgument methodArgument, String callerObject = null, [CallerMemberName] string methodName = null)
            where TEntity : class;
    }
}
