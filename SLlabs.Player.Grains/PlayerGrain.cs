using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using SLlabs.Player.Interfaces;

namespace SLlabs.Player.Grains
{
	public class PlayerGrainState
	{
		public string Alias { get; set; }
	}

	[StorageProvider(ProviderName = "AzureTableStore")]
	public class PlayerGrain : Grain<PlayerGrainState>, IPlayerGrain
	{
		public override Task OnActivateAsync()
		{
			var id = this.GetPrimaryKeyString();
			Console.WriteLine($"Activated {id}");
			if (!string.IsNullOrWhiteSpace(State.Alias))
				Console.WriteLine($"Alias was already set as '{id}'.");
			return TaskDone.Done;
		}

		public Task<string> Say(string message)
		{
			var result = $"{State.Alias ?? "You"} said: '{message}'.";
			return Task.FromResult(result);
		}

		public Task<string> GetAlias()
		{
			return Task.FromResult(State.Alias);
		}

		public async Task SetAlias(string alias)
		{
			if (State.Alias != alias)
			{
				State.Alias = alias;
				Console.WriteLine($"[SetAlias] Alias changed! '{alias}'");
				await WriteStateAsync();
			}

			Console.WriteLine($"[SetAlias] Alias set! '{alias}'");
		}
	}
}
