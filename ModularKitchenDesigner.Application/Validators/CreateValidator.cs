using ModularKitchenDesigner.Domain.Interfaces.Validators;
using System.Runtime.CompilerServices;
using System.Text;

namespace ModularKitchenDesigner.Application.Validators
{
    public class CreateValidator : ICreateValidator
    {
        public List<TEntity> Validate<TEntity>(List<TEntity> models, string preffix = "", [CallerMemberName] string methodName = null, params string[] suffix)
            where TEntity : class
        {
            if (models.Any())
            {
                StringBuilder stringBuilder = new StringBuilder();

                if (!String.IsNullOrEmpty(preffix))
                    stringBuilder.AppendLine(preffix);

                stringBuilder.Append($"Entity: {typeof(TEntity).Name}");
                stringBuilder.AppendLine("ErrorMessage: Запись уже существует!");
                stringBuilder.AppendLine($"MethodName: {methodName}");

                if (suffix.Count()>0)
                    foreach(var param in suffix)
                         stringBuilder.AppendLine(param);

                throw new Exception(stringBuilder.ToString());
            }

            return models;
        }
    }
}
