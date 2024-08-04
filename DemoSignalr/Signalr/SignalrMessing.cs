using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace DemoSignalr.Signalr
{
	public class SignalrMessing<THub, TClient>(IHubContext<THub> hub, IPushStore pushStorage)
		where THub : Hub<TClient>
		where TClient : class
	{
		public async Task SendAsync<TMessage>(string methodName, TMessage data, string connectionId, string uid)
		{
			await hub.Clients.Client(connectionId).SendAsync(methodName, data);

			/*
			if(typeof(TClient).GetMembers(BindingFlags.Public).All(m => m.Name != methodName))
				return;
				*/

			var push = new StoredPush
			{
				Data = JsonSerializer.Serialize(data).Replace(@"\", "").ToLower(),
				MethodType = methodName,
				TimeStamp = DateTime.UtcNow,
				UId = uid
			};

			pushStorage.Add(push);
		}

		public async Task OnConnected(string connectionId, string uid)
		{
			var pushes = pushStorage.Pushes;


			foreach (var p in pushStorage.Pushes.Where(p => p.UId == uid)
			                             .OrderBy(p => p.TimeStamp).ToList())
			{
				var sp = p.Data.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');
				var dict = sp.ToDictionary(s => s.Split(':')[0], s => s.Split(':', 2)[1]);
				await hub.Clients.Client(connectionId).SendAsync(p.MethodType, dict);
			}
		}
	}
}