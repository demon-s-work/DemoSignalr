namespace DemoSignalr
{
	public class Clients
	{
		public List<Client> ClientsList { get; set; } = new List<Client>();
	}

	public class Client
	{
		public string UserName { get; set; }
		public string ConnectionId { get; set; }
		public string Uid { get; set; }
	}
}