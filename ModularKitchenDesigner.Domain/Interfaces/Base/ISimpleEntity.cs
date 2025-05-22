using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Interfaces.Base
{
    public interface ISimpleEntity
    {
        string Title { get; set; }
        string Code { get; set; }
    }
}
