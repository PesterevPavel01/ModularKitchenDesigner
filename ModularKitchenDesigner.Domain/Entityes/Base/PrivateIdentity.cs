using ModularKitchenDesigner.Domain.Interfaces.Base;

namespace ModularKitchenDesigner.Domain.Entityes.Base
{
    public class PrivateIdentity : IPrivateIdentity
    {
        private Guid _id { get; set; } = Guid.NewGuid();

        public Guid GetId()
        {
            return _id;
        }

        public void SetId(Guid id)
        {
            _id = id;
        }
    }
}
