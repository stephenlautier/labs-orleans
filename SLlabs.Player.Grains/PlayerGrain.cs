using System;
using System.Threading.Tasks;
using Orleans;
using SLlabs.Player.Interfaces;

namespace SLlabs.Player.Grains
{
	public interface IPlayerGrainState : IGrainState
	{
		string Alias { get; set; }
	}

	public class PlayerGrain : Grain, IPlayerGrain
	{
		private string _alias;

		public override Task OnActivateAsync()
		{
			var id = this.GetPrimaryKeyString();
			Console.WriteLine($"Activated {id}");
			if (!string.IsNullOrWhiteSpace(_alias))
				Console.WriteLine($"Alias was already set as '{id}'.");
			return TaskDone.Done;
		}

		public Task<string> Say(string message)
		{
			var result = $"{_alias ?? "You"} said: '{message}'.";
			return Task.FromResult(result);
		}

		public Task<string> GetAlias()
		{
			return Task.FromResult(_alias);
		}

		public Task SetAlias(string alias)
		{
			_alias = alias;
			Console.WriteLine($"[SetAlias] Alias set! '{alias}'");
			return TaskDone.Done;
		}
	}
}
