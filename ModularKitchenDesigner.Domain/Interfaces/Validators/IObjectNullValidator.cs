using System.Runtime.CompilerServices;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.Domain.Interfaces.Validators
{
    public interface IObjectNullValidator
    {
        TEntity Validate<TEntity, TArgument>(TEntity model, TArgument methodArgument, String callerObject = null, [CallerMemberName] string methodName = null)
             where TEntity : class;
    }
}
