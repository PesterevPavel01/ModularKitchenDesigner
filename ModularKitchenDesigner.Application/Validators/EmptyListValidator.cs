using ModularKitchenDesigner.Domain.Interfaces.Validators;
using System.Text;

namespace ModularKitchenDesigner.Application.Validators
{
    internal class EmptyListValidator : IEmptyListValidator
    {
        public List<TEntity> Validate<TEntity>(List<TEntity> models, string preffix = "", params string[] suffix)
            where TEntity : class
        {
            if (!models.Any())
            {
                StringBuilder stringBuilder = new();

                if (!String.IsNullOrEmpty(preffix))
                    stringBuilder.AppendLine(preffix);

                stringBuilder.AppendLine($"Entiry: {typeof(TEntity).Name}");
                stringBuilder.AppendLine("ErrorMessage: Нет ни одной записи, удовлетворяющей установленным условиям!");

                if (suffix.Count() > 0)
                    foreach (var param in suffix)
                        stringBuilder.AppendLine(param);

                throw new Exception(stringBuilder.ToString());
            }

            return models;
        }
    }
}
