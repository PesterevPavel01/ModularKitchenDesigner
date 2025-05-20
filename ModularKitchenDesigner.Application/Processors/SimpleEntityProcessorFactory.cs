using Interceptors;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.SimpleEntityProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntity;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors
{
    public class SimpleEntityProcessorFactory : ISimpleEntityProcessorFactory
    {
        #region fields

        private Dictionary<Type, object> _creators = null!;
        private Dictionary<Type, object> _updators = null!;
        private Dictionary<Type, object> _loaders = null!;
        private Dictionary<Type, object> _removeProcessors = null!;

        #endregion
        public SimpleEntityProcessorFactory(IRepositoryFactory repositoryFactory, IValidatorFactory validatorFactory, IDtoToEntityConverterFactory converterFactory) 
        {
            _validatorFactory = validatorFactory;
            _repositoryFactory = repositoryFactory;
            _converterFactory = converterFactory;
            _creators = [];
            _updators = [];
            _loaders = [];
            _removeProcessors = [];
        }

        #region properties

        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IValidatorFactory _validatorFactory;
        private readonly IDtoToEntityConverterFactory _converterFactory;

        #endregion

        public ICreatorProcessor<TData, TResult> GetCreatorProcessor<TProcessor, TResult, TData>()
            where TProcessor : class, ICreatorProcessor<TData, TResult>, new()
            where TResult : BaseResult
        {
            var type = typeof(TProcessor);

            if (!_creators.ContainsKey(type))
            {
                _creators[type] = new TProcessor()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory);
            }

            return (ICreatorProcessor<TData, TResult>)_creators[type];
        }
        public ILoaderProcessor<TEntity, SimpleDto> GetLoaderProcessor<TProcessor, TEntity>()
            where TEntity : Identity, ISimpleEntity, IAuditable
            where TProcessor : class, ILoaderProcessor<TEntity, SimpleDto>, new()
        {
            var type = typeof(TEntity);

            if (!_loaders.ContainsKey(type))
                _loaders[type] = new TProcessor()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory);

            return (ILoaderProcessor<TEntity, SimpleDto>)_loaders[type];
        }

        public IUpdaterProcessor<SimpleDto,BaseResult<SimpleDto>, TEntity> GetUpdaterProcessor<TEntity>()
            where TEntity : class, ISimpleEntity, IConvertibleToDto<TEntity,SimpleDto>, new()
        {
            var type = typeof(TEntity);

            if (!_updators.ContainsKey(type))
                _updators[type] = new SimpleEntitySingleUpdaterProcessor<TEntity,SimpleEntityConverter<TEntity>>()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory)
                    .SetDtoToEntityConverterFactory(_converterFactory);

            return (IUpdaterProcessor<SimpleDto, BaseResult<SimpleDto>, TEntity>)_updators[type];
        }



        public ISimpleEntityRemoveProcessor GetRemoveProcessor<TEntity>()
            where TEntity : class, ISimpleEntity, new()
        {
            var type = typeof(TEntity);

            if (!_removeProcessors.ContainsKey(type))
                _removeProcessors[type] = new SimpleEntityRemoveProcessor<TEntity>(_repositoryFactory, _validatorFactory);

            return (ISimpleEntityRemoveProcessor)_removeProcessors[type];
        }
    }
}
