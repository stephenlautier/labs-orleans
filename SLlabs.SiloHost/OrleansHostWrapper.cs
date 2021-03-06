﻿using System;
using System.Net;
using System.Reflection;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace SLlabs.Silo
{
	internal class OrleansHostWrapper
	{
		private readonly SiloHost _siloHost;

		public OrleansHostWrapper(ClusterConfiguration config, string[] args)
		{
			var siloArgs = SiloArgs.ParseArguments(args);
			if (siloArgs == null) return;

			if (siloArgs.DeploymentId != null)
				config.Globals.DeploymentId = siloArgs.DeploymentId;

			_siloHost = new SiloHost(siloArgs.SiloName, config);
			_siloHost.LoadOrleansConfig();
		}
		
		public int Run()
		{
			if (_siloHost == null)
			{
				SiloArgs.PrintUsage();
				return 1;
			}

			try
			{
				_siloHost.InitializeOrleansSilo();
				if (!_siloHost.StartOrleansSilo())
					throw new OrleansException($"Failed to start Orleans silo '{_siloHost.Name}' as a {_siloHost.Type} node.");
				Console.WriteLine($"Successfully started Orleans silo '{_siloHost.Name}' as a {_siloHost.Type} node.");
				Console.WriteLine("OrleansHost is running. Press [Ctrl]-C to stop...");
				_siloHost.WaitForOrleansSiloShutdown();
				Console.WriteLine("OrleansHost stopped waiting, time to shut down!");
				return 0;
			}
			catch (Exception ex)
			{
				_siloHost.ReportStartupError(ex);
				Console.Error.WriteLine(ex);
				return 1;
			}
		}

		public int Stop()
		{
			if (_siloHost == null) return 0;

			try
			{
				_siloHost.StopOrleansSilo();
				_siloHost.Dispose();
				Console.WriteLine($"Orleans silo '{_siloHost.Name}' shutdown.");
			}
			catch (Exception ex)
			{
				_siloHost.ReportStartupError(ex);
				Console.Error.WriteLine(ex);
				return 1;
			}
			return 0;
		}

		class SiloArgs
		{

			public SiloArgs(string siloName, string deploymentId)
			{
				this.DeploymentId = deploymentId;
				this.SiloName = siloName;
			}

			public string SiloName { get; }

			public string DeploymentId { get; }


			public static SiloArgs ParseArguments(string[] args)
			{
				string deploymentId = null;
				string siloName = null;

				foreach (string arg in args)
				{
					if (arg.StartsWith("-") || arg.StartsWith("/"))
					{
						switch (arg.ToLowerInvariant())
						{
							case "/?":
							case "/help":
							case "-?":
							case "-help":
								// Query usage help. Return null so that usage is printed
								return null;
							default:
								Console.WriteLine($"Bad command line arguments supplied: {arg}");
								return null;
						}
					}
					if (arg.Contains("="))
					{
						string[] parameters = arg.Split('=');
						if (String.IsNullOrEmpty(parameters[1]))
						{
							Console.WriteLine($"Bad command line arguments supplied: {arg}");
							return null;
						}
						switch (parameters[0].ToLowerInvariant())
						{
							case "deploymentid":
								deploymentId = parameters[1];
								break;
							case "name":
								siloName = parameters[1];
								break;
							default:
								Console.WriteLine($"Bad command line arguments supplied: {arg}");
								return null;
						}
					}
					else
					{
						Console.WriteLine($"Bad command line arguments supplied: {arg}");
						return null;
					}
				}
				// Default to machine name
				siloName = siloName ?? Dns.GetHostName();
				return new SiloArgs(siloName, deploymentId);
			}

			public static void PrintUsage()
			{
				string consoleAppName = typeof(SiloArgs).GetTypeInfo().Assembly.GetName().Name;
				Console.WriteLine(
					$@"USAGE: {consoleAppName} [name=<siloName>] [deploymentId=<idString>] [/debug]
                Where:
                name=<siloName> - Name of this silo (optional)
                deploymentId=<idString> - Optionally override the deployment group this host instance should run in 
                (otherwise will use the one in the configuration");
			}

		}
	}


}