using System.Runtime.CompilerServices;
using ModularKitchenDesigner.Application.Exceptions;
using ModularKitchenDesigner.Domain.Interfaces.Validators;

namespace ModularKitchenDesigner.Application.Validators
{
    internal class EmptyListValidator : IEmptyListValidator
    {
        public List<TEntity> Validate<TEntity, TArgument>(List<TEntity> models, TArgument methodArgument, String callerObject = null, [CallerMemberName] string methodName = null)
            where TEntity : class
        {
            if (!models.Any())
            {
                ErrorMessage errorMessage = new()
                {
                    Title = "Ошибка валидации!",
                    MethodName = methodName,
                    Entity = typeof(TEntity).Name,
                    Message = "Записи не найдены!",
                    CallerObject = callerObject,
                    MethodArgument = methodArgument,
                    Code = 1
                };

                throw new ValidationException(errorMessage.ToJson());
            }

            return models;
        }
    }
}
