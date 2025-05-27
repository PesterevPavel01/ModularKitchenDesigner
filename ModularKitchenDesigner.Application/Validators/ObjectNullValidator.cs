using System.Runtime.CompilerServices;
using ModularKitchenDesigner.Application.Errors;
using ModularKitchenDesigner.Application.Exceptions;
using ModularKitchenDesigner.Domain.Interfaces.Validators;

namespace ModularKitchenDesigner.Application.Validators
{
    internal class ObjectNullValidator : IObjectNullValidator
    {
        public TEntity Validate<TEntity, TArgument>(TEntity model, TArgument methodArgument, String callerObject = null, [CallerMemberName] string methodName = null)
             where TEntity : class
        {
            if (model is null) 
            {
                ErrorMessage errorMessage = new()
                {
                    Title = "Ошибка валидации",
                    Entity = typeof(TEntity).Name,
                    Message = "Оъект не найден!",
                    CallerObject = callerObject,
                    MethodName = methodName,
                    MethodArgument = methodArgument,
                    Code = 0
                };

                throw new ValidationException(errorMessage.ToJson());
            }

            return model;
        }
    }
}
