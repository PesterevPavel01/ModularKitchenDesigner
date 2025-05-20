using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class DtoToEntityConverterFactory : IDtoToEntityConverterFactory
    {
        private Dictionary<Type, object> _converters = [];
        private readonly IRepositoryFactory _repositoryFactory = null!;
        private readonly IValidatorFactory _validatorFactory = null!;
        private readonly IDtoToEntityConverterFactory _converterFactory = null!;

        public DtoToEntityConverterFactory(IValidatorFactory validatorFactory, IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _validatorFactory = validatorFactory;
        }

        public IDtoToEntityConverter<TEntity, TDto> GetConverter<TEntity, TDto, TConverter>() 
            where TEntity : class, IConvertibleToDto<TEntity, TDto>, new()
            where TConverter : IDtoToEntityConverter<TEntity, TDto>, new()
        {
            var type = typeof(TEntity);

            if (!_converters.ContainsKey(type))
            {
                _converters[type] = new TConverter()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory);
            }

            return (IDtoToEntityConverter<TEntity, TDto>)_converters[type];
        }
    
    }
}
