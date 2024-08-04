namespace DemoSignalr.Signalr
{
	public interface ITestClient
	{
		Task NewMessage(Message m);
		Task Ping(Ping p);
	}
}