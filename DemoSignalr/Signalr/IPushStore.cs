using StackExchange.Redis;

namespace DemoSignalr.Signalr
{
	public interface IPushStore
	{
		StoredPush? this[string id] { get; }
		IList<StoredPush> Pushes { get; }
		void Add(StoredPush push);
		void Remove(StoredPush push);
	}
}