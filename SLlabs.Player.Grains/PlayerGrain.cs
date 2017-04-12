﻿using System;
using System.Threading.Tasks;
using Orleans;
using SLlabs.Player.Interfaces;

namespace SLlabs.Player.Grains
{
	public class PlayerGrain : Grain, IPlayerGrain
	{
		public Task Say(string message)
		{
			return Task.FromResult($"You said: '{message}'.");
		}

		public Task SetAlias(string alias)
		{
			throw new NotImplementedException();
			//return Task.FromResult("")
		}
	}
}