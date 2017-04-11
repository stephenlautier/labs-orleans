using System.Threading.Tasks;
using Orleans;

namespace SLlabs.Hero.Interfaces
{
	public interface IPlayerGrain : IGrain
	{

		Task Say(string message);

		Task SetAlias(string alias);

	}
}
