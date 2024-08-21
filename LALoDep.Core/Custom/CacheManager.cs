using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace LALoDep.Core.Custom
{
    /// <summary>
    /// Uses System.Runtime.Caching to provide a thread-safe caching class.
    /// Recommended use is to employ the TryGetAndSet method as a wrapper to call
    /// your data-building function.
    /// </summary>
    public static class CacheManager
    {

        /// <summary>
        /// The cache store. A dictionary that stores different memory caches by the type being cached.
        /// </summary>
        private static ConcurrentDictionary<Type, ObjectCache> cacheStore;

        /// <summary>
        /// The default minutes (15)
        /// </summary>
        private const int DefaultMinutes = 15;

        #region constructors

        /// <summary>
        /// Initializes the <see cref="CacheManager"/> class.
        /// </summary>
        static CacheManager()
        {
            cacheStore = new ConcurrentDictionary<Type, ObjectCache>();
        }
        #endregion

        #region Setters

        /// <summary>
        /// Sets the specified cache using the default absolute timeout of 15 minutes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheItem">The data to be cached.</param>
        static internal void Set<T>(string cacheKey, T cacheItem)
        {
            Set<T>(cacheKey, cacheItem, DefaultMinutes);
        }

        /// <summary>
        /// Sets the specified cache using the absolute timeout specified in minutes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheItem">The data to be cached.</param>
        /// <param name="minutes">The absolute expiration (in minutes).</param>
        static internal void Set<T>(string cacheKey, T cacheItem, int minutes)
        {
            var t = typeof(T);
            if (!cacheStore.ContainsKey(t))
            {
                RegisterCache(t);
            }
            var cache = cacheStore[t];
            cache.Set(cacheKey, cacheItem, GetCacheItemPolicy(minutes));

        }

        /// <summary>
        /// Sets the specified cache using the passed function to generate the data. 
        /// Uses default absolute timeout of 15 minutes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="getData">The function to generate the data to be cached.</param>
        static internal void Set<T>(string cacheKey, Func<T> getData)
        {
            Set<T>(cacheKey, getData, DefaultMinutes);
        }

        /// <summary>
        /// Sets the specified cache using the passed function to generate the data. 
        /// Uses the specified absolute timeout (in minutes).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="getData">The function to generate the data to be cached.</param>
        /// <param name="minutes">The absolute expiration (in minutes).</param>
        static internal void Set<T>(string cacheKey, Func<T> getData, int minutes)
        {
            
                Type t = typeof(T);
                if (!cacheStore.ContainsKey(t))
                {
                    RegisterCache(t);
                }
                var cache = cacheStore[t];
                T data = getData();
                cache.Set(cacheKey, data, GetCacheItemPolicy(minutes));
            
        }
        #endregion

        #region Getters
        /// <summary>
        /// Tries to retrieve data from cache first. If the data is not found in cache, the passed function
        /// will be used to generate and store the data in cache. Data is returned via the returnData parameter.
        /// Function returns true if successful.
        /// Uses the default absolute timeout of 15 minutes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="getData">The function to generate the data to be cached.</param>
        /// <param name="returnData">The return data.</param>
        /// <returns>True if successful. False if data is null.</returns>
        public static bool TryGetAndSet<T>(string cacheKey, Func<T> getData, out T returnData)
        {
               Remove<T>(cacheKey);
            
            Type t = typeof(T);
            bool retrievedFromCache = TryGet<T>(cacheKey, out returnData);
            if (retrievedFromCache)
            {
                return true;
            }
            else
            {
                returnData = getData();
                Set<T>(cacheKey, returnData);
                return returnData != null;
            }
        }

        /// <summary>
        /// Tries to retrieve data from cache first. If the data is not found in cache, the passed function
        /// will be used to generate and store the data in cache. Data is returned via the returnData parameter.
        /// Function returns true if successful.
        /// Uses the specified absolute timeout (in minutes).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="getData">The function to generate the data to be cached.</param>
        /// <param name="minutes">The absolute expiration (in minutes).</param>
        /// <param name="returnData">The return data.</param>
        /// <returns>True if successful. False if data is null.</returns>
        public static bool TryGetAndSet<T>(string cacheKey, Func<T> getData, int minutes, out T returnData)
        {
            Type t = typeof(T);
            bool retrievedFromCache = TryGet<T>(cacheKey, out returnData);
            if (retrievedFromCache)
            {
                return true;
            }
            else
            {
                returnData = getData();
                Set<T>(cacheKey, returnData, minutes);
                return returnData != null;
            }
        }

        /// <summary>
        /// Attempts to retrieve data from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="returnItem">The data from cache.</param>
        /// <returns>True if successful. False if data is null or not found.</returns>
        static internal bool TryGet<T>(string cacheKey, out T returnItem)
        {
            Type t = typeof(T);
            if (cacheStore.ContainsKey(t))
            {
                var cache = cacheStore[t];
                object tmp = cache[cacheKey];
                if (tmp != null)
                {
                    returnItem = (T)tmp;
                    return true;
                }
            }
            returnItem = default(T);
            return false;
        }
        #endregion

        /// <summary>
        /// Removes the specified item from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        static internal void Remove<T>(string cacheKey)
        {
            Type t = typeof(T);
            if (cacheStore.ContainsKey(t))
            {
                var cache = cacheStore[t];
                cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Registers the cache in the dictionary.
        /// </summary>
        /// <param name="t">The type used as the key for the MemoryCache that stores this type of data.</param>
        private static void RegisterCache(Type t)
        {
            ObjectCache newCache = new MemoryCache(t.ToString());
            cacheStore.AddOrUpdate(t, newCache, UpdateItem);
        }

        /// <summary>
        /// Updates the item. Required for use of the ConcurrentDictionary type to make this thread-safe.
        /// </summary>
        /// <param name="t">The Type used as the key for the MemoryCache that stores this type of data.</param>
        /// <param name="cache">The cache to be updated.</param>
        /// <returns></returns>
        private static ObjectCache UpdateItem(Type t, ObjectCache cache)
        {
            var newCache = new MemoryCache(cache.Name);
            foreach (var cachedItem in cache)
            {
                newCache.Add(cachedItem.Key, cachedItem.Value, GetCacheItemPolicy(DefaultMinutes));
            }
            return newCache;
        }

        /// <summary>
        /// Gets the cache item policy.
        /// </summary>
        /// <param name="minutes">The absolute expiration, in minutes.</param>
        /// <returns>A standard CacheItemPolicy, varying only in expiration duration, for all items stored in MemoryCache.</returns>
        private static CacheItemPolicy GetCacheItemPolicy(int minutes = 15)
        {
            var policy = new CacheItemPolicy()
            {
                Priority = System.Runtime.Caching.CacheItemPriority.Default,
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutes)
            };
            return policy;
        }
    }

}