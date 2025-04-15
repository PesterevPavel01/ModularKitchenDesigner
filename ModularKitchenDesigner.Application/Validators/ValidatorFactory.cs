using ModularKitchenDesigner.Domain.Interfaces.Validators;

namespace ModularKitchenDesigner.Application.Validators
{
    internal class ValidatorFactory : IValidatorFactory
    {
        private Dictionary<Type, object>? _validators = [];

        public ICreateValidator GetCreateValidator()
        {
            var type = typeof(CreateValidator);

            if (!_validators.ContainsKey(type))
            {
                _validators[type] = new CreateValidator();
            }

            return (ICreateValidator)_validators[type];
        }

        public IEmptyListValidator GetEmptyListValidator()
        {
            var type = typeof(EmptyListValidator);

            if (!_validators.ContainsKey(type))
            {
                _validators[type] = new EmptyListValidator();
            }

            return (IEmptyListValidator)_validators[type];
        }

        public IObjectNullValidator GetObjectNullValidator()
        {
            var type = typeof(ObjectNullValidator);

            if (!_validators.ContainsKey(type))
            {
                _validators[type] = new ObjectNullValidator();
            }

            return (IObjectNullValidator)_validators[type];
        }
    }
}
