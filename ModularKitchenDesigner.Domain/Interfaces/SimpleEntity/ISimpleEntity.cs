using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Interfaces
{
    public interface ISimpleEntity
    {
        string Title { get; set; }
        string Code { get; set; }
    }
}
