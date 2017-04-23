using System.Threading.Tasks;
using Orleans;

namespace SLlabs.Player.Interfaces
{
	public interface IPlayerGrain : IGrainWithStringKey
	{

		Task<string> Say(string message);

		Task SetAlias(string alias);

		Task<string> GetAlias();
	}
}
