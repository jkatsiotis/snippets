using System;
using System.Collections.Generic;
using StackExchange.Profiling;
using StackExchange.Profiling.Storage;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ItsJk.Snippets.MiniProfiler
{
   //This is a Wrapper for MiniProfiler.RedisStorage that logs only if the duration of the profile is more than the threshold set. 
   public class RedisStorageWithThreshold : IAsyncStorage, IDisposable
    {
        private readonly int _threshold;
        private readonly StackExchange.Profiling.Storage.RedisStorage _redisStorage = null;
        public RedisStorageWithThreshold(ConnectionMultiplexer multiplexer, int thresholdInMillis) {
            _redisStorage = new RedisStorage(multiplexer);
            _threshold = thresholdInMillis;
        }
        public void Dispose() {
            _redisStorage?.Dispose();
        }

        public List<Guid> GetUnviewedIds(string user) {
            return _redisStorage.GetUnviewedIds(user);
        }

        public Task<List<Guid>> GetUnviewedIdsAsync(string user) {
            return _redisStorage.GetUnviewedIdsAsync(user);
        }

        public IEnumerable<Guid> List(int maxResults, DateTime? start = null, DateTime? finish = null, ListResultsOrder orderBy = ListResultsOrder.Descending) {
            return _redisStorage.List(maxResults, start, finish, orderBy);
        }

        public Task<IEnumerable<Guid>> ListAsync(int maxResults, DateTime? start = null, DateTime? finish = null, ListResultsOrder orderBy = ListResultsOrder.Descending) {
            return _redisStorage.ListAsync(maxResults, start, finish, orderBy);
        }

        public MiniProfiler Load(Guid id) {
            return _redisStorage.Load(id);
        }

        public Task<MiniProfiler> LoadAsync(Guid id) {
            return _redisStorage.LoadAsync(id);
        }

        public void Save(MiniProfiler profiler) {
            if (profiler.DurationMilliseconds < _threshold) {
                return;
            }
            _redisStorage.Save(profiler);
        }

        public Task SaveAsync(MiniProfiler profiler) {
            if (profiler.DurationMilliseconds < _threshold) {
                return Task.CompletedTask;
            }
            return _redisStorage.SaveAsync(profiler);
        }

        public void SetUnviewed(string user, Guid id) {
            _redisStorage.SetUnviewed(user, id);
        }

        public Task SetUnviewedAsync(string user, Guid id) {
            return _redisStorage.SetUnviewedAsync(user, id);
        }

        public void SetViewed(string user, Guid id) {
            _redisStorage.SetViewed(user, id);
        }

        public Task SetViewedAsync(string user, Guid id) {
            return _redisStorage.SetViewedAsync(user, id);
        }
    }
}
