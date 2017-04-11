using System;
using Orleans.Runtime.Configuration;

namespace SLlabs.Silo
{
	public class Program
	{
		private static OrleansHostWrapper hostWrapper;

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Console.ReadLine();
		}

		private static int StartSilo(string[] args)
		{
			var config = ClusterConfiguration.LocalhostPrimarySilo();
			config.AddMemoryStorageProvider();
			//config.Defaults.DefaultTraceLevel = Severity.Verbose3;
			
			hostWrapper = new OrleansHostWrapper(config, args);
			return hostWrapper.Run();
		}
	}


}