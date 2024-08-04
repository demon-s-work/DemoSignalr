using CachingFramework.Redis;
using CachingFramework.Redis.Contracts.RedisObjects;

namespace DemoSignalr.Signalr
{
	public class RedisPush : IPushStore
	{
        private readonly RedisContext _redisContext;
        private string Key = "Pushes";
        private readonly IRedisList<StoredPush> _pushes;

        public RedisPush(RedisContext redisContext)
        {
	        _redisContext = redisContext;
	        _pushes = redisContext.Collections.GetRedisList<StoredPush>(Key);
        }

        public IList<StoredPush?> Pushes => _pushes.ToList();
        public StoredPush? this[string id] => _pushes.FirstOrDefault(p => p.UId == id);

        public void Add(StoredPush push)
        {
	        _pushes.Add(push);
        }

        public void Remove(StoredPush push)
        {
	        _pushes.Remove(push);
        }
	}
}