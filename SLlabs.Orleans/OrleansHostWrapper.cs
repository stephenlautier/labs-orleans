using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace SLlabs.Silo
{
	class OrleansHostWrapper
	{
		private readonly SiloHost siloHost;

		public OrleansHostWrapper(ClusterConfiguration config, string[] args)
		{
			var siloArgs = SiloArgs.ParseArguments(args);
			if (siloArgs == null) return;

			if (siloArgs.DeploymentId != null)
				config.Globals.DeploymentId = siloArgs.DeploymentId;
			
			siloHost = new SiloHost(siloArgs.SiloName, config);
			siloHost.LoadOrleansConfig();
		}

		// todo: implement others

		class SiloArgs
		{

			public SiloArgs(string siloName, string deploymentId)
			{
				this.DeploymentId = deploymentId;
				this.SiloName = siloName;
			}

			public string SiloName { get; set; }

			public string DeploymentId { get; set; }

			public static SiloArgs ParseArguments(string[] args)
			{
				throw new System.NotImplementedException();
			}
	
		}
	}


}