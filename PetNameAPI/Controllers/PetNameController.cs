using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class PetNameController : ControllerBase
{
	private static readonly Dictionary<string, List<string>> names = new()
	{
		{ "dog", new List<string> { "Buddy", "Max", "Charlie", "Rocky", "Rex" } },
		{ "cat", new List<string> { "Whiskers", "Mittens", "Luna", "Simba", "Tiger" } },
		{ "bird", new List<string> { "Tweety", "Sky", "Chirpy", "Raven", "Sunny" } }
	};

	[HttpPost("generate")]
	public IActionResult GeneratePetName([FromBody] PetNameRequest request)
	{
		if (request.AnimalType == null)
			return BadRequest(new { error = "The 'animalType' field is required." });

		if (!names.ContainsKey(request.AnimalType.ToLower()))
			return BadRequest(new { error = "Invalid animal type. Allowed values: dog, cat, bird." });

		if (request.TwoPart != null && request.TwoPart is not bool)
			return BadRequest(new { error = "The 'twoPart' field must be a boolean (true or false)." });

		var animalNames = names[request.AnimalType.ToLower()];
		var random = new Random();
		string generatedName = request.TwoPart == true
			? animalNames[random.Next(animalNames.Count)] + animalNames[random.Next(animalNames.Count)]
			: animalNames[random.Next(animalNames.Count)];

		return Ok(new { name = generatedName });
	}
}

public class PetNameRequest
{
	public string AnimalType { get; set; }
	public bool? TwoPart { get; set; }
}
