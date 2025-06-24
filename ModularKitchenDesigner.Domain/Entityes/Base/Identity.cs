namespace ModularKitchenDesigner.Domain.Entityes.Base
{
    public abstract class Identity
    {
        public Identity(){}
        
        protected Identity(Guid id)
            => Id = id;

        public Guid Id { get; protected set; }



    }
}
