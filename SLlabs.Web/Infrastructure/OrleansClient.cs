using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Threading;
using Microsoft.Extensions.Options;

namespace SLlabs.Web.Infrastructure
{
	public class OrleansClient
	{
		private readonly IOptions<OrleansConfig> _config;

		public OrleansClient(
			IOptions<OrleansConfig> config
		)
		{
			_config = config;
		}

		public int Initialize()
		{
			var config = ClientConfiguration.LocalhostSilo();
			if (_config.Value.DeploymentId != null)
			{
				config.DeploymentId = _config.Value.DeploymentId;
			}
			try
			{
				InitializeWithRetries(config, initializeAttemptsBeforeFailing: 5);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Orleans client initialization failed failed due to {ex}");
				Console.ReadLine();
				return 1;
			}
			return 0;
		}

		private static void InitializeWithRetries(ClientConfiguration config, int initializeAttemptsBeforeFailing)
		{
			int attempt = 0;
			while (true)
			{
				try

				{
					GrainClient.Initialize(config);
					Console.WriteLine("Client successfully connect to silo host");
					break;
				}
				catch (SiloUnavailableException)
				{
					attempt++;
					Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
					if (attempt > initializeAttemptsBeforeFailing) throw;
					Thread.Sleep(TimeSpan.FromSeconds(2));
				}

			}
		}
	}
}
