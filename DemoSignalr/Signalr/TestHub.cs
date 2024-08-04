using Microsoft.AspNetCore.SignalR;

namespace DemoSignalr.Signalr
{
	public class TestHub : Hub<ITestClient>
	{
		private SignalrMessing<TestHub, ITestClient> _signalrMessing;

		public TestHub(SignalrMessing<TestHub, ITestClient> signalrMessing)
		{
			_signalrMessing = signalrMessing;
		}

		public override async Task OnConnectedAsync()
		{
			var ctx = Context.GetHttpContext();
			if (ctx != null && ctx.Request.Cookies.TryGetValue("uid", out var uid))
				await _signalrMessing.OnConnected(Context.ConnectionId, uid);
			await base.OnConnectedAsync();
		}
	}
}