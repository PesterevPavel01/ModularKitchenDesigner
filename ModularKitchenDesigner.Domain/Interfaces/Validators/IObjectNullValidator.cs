using System.Runtime.CompilerServices;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.Domain.Interfaces.Validators
{
    public interface IObjectNullValidator
    {
        TEntity Validate<TEntity>(TEntity model, string preffix = "", [CallerMemberName] string methodName = null, params string[] suffix)
             where TEntity : class;
    }
}
