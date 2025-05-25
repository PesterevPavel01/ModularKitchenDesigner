using Interceptors;
using ModularKitchenDesigner.Application.Processors.SimpleEntityProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntity;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;

namespace ModularKitchenDesigner.Application.Processors
{
    public class SimpleEntityProcessorFactory : ISimpleEntityProcessorFactory
    {
        #region fields

        private Dictionary<Type, object> _loaders = null!;
        private Dictionary<Type, object> _removeProcessors = null!;

        #endregion
        public SimpleEntityProcessorFactory(IRepositoryFactory repositoryFactory, IValidatorFactory validatorFactory, IDtoToEntityConverterFactory converterFactory) 
        {
            _validatorFactory = validatorFactory;
            _repositoryFactory = repositoryFactory;
            _converterFactory = converterFactory;
            _loaders = [];
            _removeProcessors = [];
        }

        #region properties

        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IValidatorFactory _validatorFactory;
        private readonly IDtoToEntityConverterFactory _converterFactory;

        #endregion

        public ILoaderProcessor<TEntity, SimpleDto> GetLoaderProcessor<TProcessor, TEntity>()
            where TEntity : Identity, ISimpleEntity, IDtoConvertible<TEntity, SimpleDto>
            where TProcessor : class, ILoaderProcessor<TEntity, SimpleDto>, new()
        {
            var type = typeof(TEntity);

            if (!_loaders.ContainsKey(type))
                _loaders[type] = new TProcessor()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory);

            return (ILoaderProcessor<TEntity, SimpleDto>)_loaders[type];
        }

        public ISimpleEntityRemoveProcessor GetRemoveProcessor<TEntity>()
            where TEntity : class, ISimpleEntity, IDtoConvertible<TEntity, SimpleDto>, new ()
        {
            var type = typeof(TEntity);

            if (!_removeProcessors.ContainsKey(type))
                _removeProcessors[type] = new SimpleEntityRemoveProcessor<TEntity>(_repositoryFactory, _validatorFactory);

            return (ISimpleEntityRemoveProcessor)_removeProcessors[type];
        }
    }
}
