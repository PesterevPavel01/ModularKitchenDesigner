using ModularKitchenDesigner.Domain.Interfaces.Validators;
using System.Runtime.CompilerServices;
using System.Text;

namespace ModularKitchenDesigner.Application.Validators
{
    internal class ObjectNullValidator : IObjectNullValidator
    {
        public TEntity Validate<TEntity>(TEntity model, string preffix = "", [CallerMemberName] string methodName = null, params string[] suffix)
             where TEntity : class
        {
            if (model is null) 
            {
                StringBuilder stringBuilder = new ();
                
                if (!String.IsNullOrEmpty(preffix))
                    stringBuilder.AppendLine(preffix);
                
                stringBuilder.AppendLine($"Entity: {typeof(TEntity).Name}");
                stringBuilder.AppendLine("ErrorMessage: Запись не найдена!");
                stringBuilder.AppendLine($"MethodName: {methodName}");

                if (suffix.Count() > 0)
                    foreach (var param in suffix)
                        stringBuilder.AppendLine(param);

                throw new Exception(stringBuilder.ToString());
            }

            return model;
        }
    }
}
