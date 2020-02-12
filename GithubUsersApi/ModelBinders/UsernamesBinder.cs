using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GithubUsersApi.ModelBinders
{
    public class UsernamesBinder : ModelBinderAttribute, IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var values = bindingContext.ValueProvider.GetValue("Usernames");
            if (values.Length == 0)
                return Task.CompletedTask;

            var splitData = values.FirstValue.Split(',');
            var usernames = new List<string>(splitData);
            bindingContext.Result = ModelBindingResult.Success(usernames);

            return Task.CompletedTask;
        }
    }
}
