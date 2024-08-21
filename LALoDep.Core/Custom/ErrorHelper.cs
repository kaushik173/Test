using System.Linq;

namespace LALoDep.Core.Custom
{
    public class ErrorHelper : IErrorHelper
    {
        public string GetErrors<T>(System.Web.Mvc.ModelStateDictionary modelState)
        {
            return string.Join("<br />", modelState.SelectMany(o => o.Value.Errors.Select(o1 => o1.ErrorMessage)));
        }
    }

    public interface IErrorHelper
    {
        string GetErrors<T>(System.Web.Mvc.ModelStateDictionary modelState);
    }
}