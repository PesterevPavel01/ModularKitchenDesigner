using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.SimpleEntity;
using Repository;

namespace ModularKitchenDesigner.Application.Services.SimpleEntity
{
    internal class SimpleEntityService : ISimpleEntityService
    {
        #region fields

        private Dictionary<Type, object> _creators = null!;
        private Dictionary<Type, object> _updators = null!;
        private Dictionary<Type, object> _loaders = null!;

        #endregion
        public SimpleEntityService(IRepositoryFactory repositoryFactory) 
        {
            _repositoryFactory = repositoryFactory;
            _creators = [];
            _updators = [];
            _loaders = [];
        }

        #region properties

        IRepositoryFactory _repositoryFactory;

        #endregion

        public ISimpleEntityCreator<TEntity, TDto> GetCreator<TEntity, TDto>()
            where TEntity : class, ISimpleEntity, new()
            where TDto : class, ISimpleEntity, new()
        {
            var type = typeof(TEntity);

            if (!_creators.ContainsKey(type))
                _creators[type] = new SimpleEntityCreator<TEntity, TDto>(_repositoryFactory);

            return (ISimpleEntityCreator<TEntity, TDto>)_creators[type];
        }

        public ISimpleEntityUpdater<TEntity, TDto> GetUpdater<TEntity, TDto>()
            where TEntity : class, ISimpleEntity, new()
            where TDto : class, ISimpleEntity, new()
        {
            var type = typeof(TEntity);

            if (!_updators.ContainsKey(type))
                _updators[type] = new SimpleEntityUpdater<TEntity, TDto>(_repositoryFactory);

            return (ISimpleEntityUpdater<TEntity, TDto>)_updators[type];
        }

        ISimpleEntityLoader<TEntity, TDto> ISimpleEntityService.GetLoader<TEntity, TDto>()
        {
            var type = typeof(TEntity);

            if (!_loaders.ContainsKey(type))
                _loaders[type] = new SimpleEntityLoader<TEntity, TDto>(_repositoryFactory);

            return (ISimpleEntityLoader<TEntity, TDto>)_loaders[type];
        }
    }
}
