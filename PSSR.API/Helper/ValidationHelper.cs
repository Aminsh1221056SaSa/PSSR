using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.API.Helper
{
    public static class ValidationHelper
    {
        public static string CopyErrorsToString(this IStatusGeneric status, ModelStateDictionary modelState)
        {
            string lstErrors ="";
            foreach (var error in status.Errors)
            {
                lstErrors += error.ErrorMessage+ "\n";
            }

            foreach (var state in modelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    lstErrors+= error.ErrorMessage + "\n";
                }
            }

            return lstErrors;
        }
    }
}
