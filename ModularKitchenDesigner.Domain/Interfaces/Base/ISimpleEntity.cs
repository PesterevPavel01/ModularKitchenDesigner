using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.Domain.Interfaces.Base
{
    public interface ISimpleEntity
    {
        //string Title { get; }
        //string Code { get; }

        static abstract TEntity Create<TEntity>(string title, string code, bool enabled = true, Guid id = default)
            where TEntity : SimpleEntity, new();
        ISimpleEntity Update(string title, string code, bool enabled = true);
    }
}
