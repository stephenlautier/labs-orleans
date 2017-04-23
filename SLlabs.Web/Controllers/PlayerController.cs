using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using SLlabs.Player.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SLlabs.Web.Controllers
{
	[Route("api/[controller]")]
	public class PlayerController : Controller
	{
		// GET: api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/player/chiko
		[HttpGet("{id}")]
		public async Task<string> Get(string id)
		{
			var player = GrainClient.GrainFactory.GetGrain<IPlayerGrain>(id);
			var result = await player.GetAlias();
			return result;
		}

		// POST api/player/chiko/say
		[HttpPost("{id}/say")]
		public async Task<ActionResult> Say(string id, [FromBody]string value)
		{
			var player = GrainClient.GrainFactory.GetGrain<IPlayerGrain>(id);
			await player.Say(value);
			return Ok();
		}
		// POST api/player/chiko/alias
		[HttpPost("{id}/alias")]
		public async Task<ActionResult> SetAlias(string id, [FromBody]string value)
		{
			var player = GrainClient.GrainFactory.GetGrain<IPlayerGrain>(id);
			await player.SetAlias(value);
			return Ok();
		}

	}
}