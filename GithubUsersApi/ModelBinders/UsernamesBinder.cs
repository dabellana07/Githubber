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

            var value = bindingContext.ValueProvider.GetValue("Usernames");
            if (value.Length == 0)
                return Task.CompletedTask;

            var usernames = new List<string>(ParseCommaSeparatedStringToList(value.FirstValue));
            bindingContext.Result = ModelBindingResult.Success(usernames);

            return Task.CompletedTask;
        }

        private List<string> ParseCommaSeparatedStringToList(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var splitData = value.Split(',');
            var lastIndex = splitData.Length <= 10
                ? splitData.Length
                : 10;
            var usernames = splitData[0..lastIndex];
            return new List<string>(usernames);
        }
    }
}
