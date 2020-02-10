using Microsoft.Extensions.Caching.Memory;

namespace GithubUsersApi.Tests.Helpers
{
    public class PassiveMemoryCacheFake : IMemoryCache
    {
        public ICacheEntry CreateEntry(object key)
        {
            return new CacheEntryFake { Key = key };
        }

        public void Dispose() {}

        public void Remove(object key) {}

        public bool TryGetValue(object key, out object value)
        {
            value = null;
            return false;
        }
    }
}
