using Interceptors;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors
{
    public sealed class ProcessorFactory<TEntity, TDto> : IProcessorFactory<TEntity, TDto>
        where TEntity : Identity, IAuditable
        where TDto : class
    {
        private Dictionary<Type, object>? _processors = [];
        private readonly IRepositoryFactory _repositoryFactory = null!;
        private readonly IValidatorFactory _validatorFactory = null!;
        private readonly IDtoToEntityConverterFactory _converterFactory = null!;

        public ProcessorFactory(IValidatorFactory validatorFactory, IRepositoryFactory repositoryFactory, IDtoToEntityConverterFactory converterFactory)
        {
            _repositoryFactory = repositoryFactory;
            _validatorFactory = validatorFactory;
            _converterFactory = converterFactory;
        }

        public ICreatorProcessor<TData, TResult> GetCreatorProcessor<TProcessor, TResult, TData>()
            where TProcessor : class, ICreatorProcessor<TData, TResult>, new()
            where TResult: BaseResult
        {
            var type = typeof(TProcessor);

            if (!_processors.ContainsKey(type))
            {
                _processors[type] = new TProcessor()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory);
            }

            return (ICreatorProcessor<TData, TResult>)_processors[type];
        }

        public ILoaderProcessor<TEntity, TDto> GetLoaderProcessor<TProcessor>()
            where TProcessor : class, ILoaderProcessor<TEntity, TDto>, new()
        {
            var type = typeof(TProcessor);

            if (!_processors.ContainsKey(type))
            {
                _processors[type] = new TProcessor()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory);
            }

            return (ILoaderProcessor<TEntity, TDto>)_processors[type];
        }

        public IUpdaterProcessor<List<TDto>, TResult, TEntity> GetUpdaterProcessor<TProcessor, TResult>()
            where TProcessor : class, IUpdaterProcessor<List<TDto>, TResult, TEntity>, new()
            where TResult : BaseResult
        {
            var type = typeof(TProcessor);

            if (!_processors.ContainsKey(type))
            {
                _processors[type] = new TProcessor()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory)
                    .SetDtoToEntityConverterFactory(_converterFactory);
            }

            return (IUpdaterProcessor<List<TDto>, TResult, TEntity>)_processors[type];
        }
    }
}
