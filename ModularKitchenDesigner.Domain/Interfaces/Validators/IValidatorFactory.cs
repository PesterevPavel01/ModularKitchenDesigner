namespace ModularKitchenDesigner.Domain.Interfaces.Validators
{
    public interface IValidatorFactory
    {
        IObjectNullValidator GetObjectNullValidator();
        ICreateValidator GetCreateValidator();
        IEmptyListValidator GetEmptyListValidator();
    }
}
