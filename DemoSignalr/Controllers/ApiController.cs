using DemoSignalr.Signalr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DemoSignalr.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class ApiController : ControllerBase
	{
		private readonly SignalrMessing<TestHub, ITestClient> _signalr;
		private readonly IHubContext<TestHub, ITestClient> _s;
		private readonly Clients _clients;

		public ApiController(SignalrMessing<TestHub, ITestClient> signalr, Clients clients)
		{
			_signalr = signalr;
			_clients = clients;
		}

		[HttpGet]
		public async Task<string> Connect([FromQuery] string userName, [FromQuery] string connectionId, [FromQuery] string? uid)
		{
			var clientUid = Guid.NewGuid().ToString();
			if (uid is null || _clients.ClientsList.All(c => c.Uid != uid))
			{
				_clients.ClientsList.Add(new Client
				{
					UserName = userName,
					ConnectionId = connectionId,
					Uid = clientUid
				});
				return clientUid;
			}
			else
			{
				var client = _clients.ClientsList.FirstOrDefault(c => c.Uid == uid);
				if (client is not null)
				{
					client.ConnectionId = connectionId;
				}
				return client?.Uid ?? clientUid;
			}

		}

		public async Task sdf(string asd)
		{}

		[HttpGet]
		public async Task Send([FromQuery] string from, [FromQuery] string message, [FromQuery] string to, [FromQuery] string uid)
		{
			var client = _clients.ClientsList.FirstOrDefault(c => c.UserName == to);
			if (client is not null)
			{
				await _signalr.SendAsync(nameof(ITestClient.NewMessage), new Message()
				{
					From = from,
					Text = message,
					Timestamp = DateTime.Now
				}, client.ConnectionId, client.Uid);

			}
		}

		[HttpGet]
		public async Task<string> Test()
		{
			return "Work";
		}
	}
}