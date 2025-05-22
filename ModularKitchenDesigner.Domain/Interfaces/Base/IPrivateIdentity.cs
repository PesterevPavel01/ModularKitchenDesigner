namespace ModularKitchenDesigner.Domain.Interfaces.Base
{
    public interface IPrivateIdentity
    {
        public Guid GetId();
        public void SetId(Guid id);
    }
}