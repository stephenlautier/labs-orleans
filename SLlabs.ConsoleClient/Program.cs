using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using SLlabs.Player.Interfaces;

namespace SLlabs.ConsoleClient
{
	class Program
	{
		static int Main(string[] args)
		{
			Console.WriteLine("Initializing Console Client...");

			var config = ClientConfiguration.LocalhostSilo();

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
			DoClientWork().Wait();
			Console.WriteLine("Press Enter to terminate...");
			Console.ReadLine();
			return 0;
			
		}

		private static async Task DoClientWork()
		{
			var player = GrainClient.GrainFactory.GetGrain<IPlayerGrain>("chiko");
			var response = await player.Say("Yordle!");
			Console.WriteLine("\n\n{0}\n\n", response);
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