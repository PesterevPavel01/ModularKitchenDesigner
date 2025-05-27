using System.Runtime.CompilerServices;
using ModularKitchenDesigner.Application.Errors;
using ModularKitchenDesigner.Application.Exceptions;
using ModularKitchenDesigner.Domain.Interfaces.Validators;

namespace ModularKitchenDesigner.Application.Validators
{
    public class CreateValidator : ICreateValidator
    {
        public List<TEntity> Validate<TEntity, TArgument>(List<TEntity> models, TArgument methodArgument, string? callerObject = null, [CallerMemberName] string? methodName = null)
            where TEntity : class
        {
            if (models.Any())
            {
                ErrorMessage errorMessage = new()
                {
                    Title = "Ошибка валидации!",
                    Entity = typeof(TEntity).Name,
                    Message = "Запись уже существует!",
                    CallerObject = callerObject,
                    MethodName = methodName,
                    MethodArgument = methodArgument,
                    Code = 0
                };

                throw new ValidationException(errorMessage.ToJson());
            }

            return models;
        }
    }
}
