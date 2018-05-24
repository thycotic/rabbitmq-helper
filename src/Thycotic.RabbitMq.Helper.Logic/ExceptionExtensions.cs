using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.RabbitMq.Helper.Logic
{
    /// <summary>
    /// Exception extensions
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Gets the combined message of the exception and its inner exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string GetCombinedMessage(this Exception ex)
        {
            var ex2 = ex;
            var sb = new StringBuilder();
            while (ex2 != null)
            {
                sb.Append($"{ex2.Message};");
                ex2 = ex2.InnerException;
            }
            return sb.ToString();
        }
    }
}
