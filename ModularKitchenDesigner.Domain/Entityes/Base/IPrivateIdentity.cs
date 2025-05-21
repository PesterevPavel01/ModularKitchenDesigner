namespace ModularKitchenDesigner.Domain.Entityes.Base
{
    public interface IPrivateIdentity
    {
        public Guid GetId();
        public void SetId(Guid id);
    }
}