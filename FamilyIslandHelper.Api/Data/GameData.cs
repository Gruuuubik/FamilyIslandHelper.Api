using FamilyIslandHelper.Api.Models.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace FamilyIslandHelper.Api.Data
{
	/// <summary>
	/// Game data (resources, buildings, items and recipes) of one API version.
	/// The data is read once from the JSON files embedded into the assembly (<c>Data_v2</c> folder).
	/// </summary>
	internal sealed class GameData
	{
		private const string MainNamespace = "FamilyIslandHelper.Api";

		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			AllowTrailingCommas = true,
			ReadCommentHandling = JsonCommentHandling.Skip
		};

		private static readonly Lazy<GameData> V2 =
			new Lazy<GameData>(() => LoadEmbedded("_v2"), LazyThreadSafetyMode.ExecutionAndPublication);

		private readonly Dictionary<string, ResourceItem> resourcesById;
		private readonly Dictionary<string, ProducibleItem> itemsById;
		private readonly Dictionary<string, Building> buildingsById;

		internal GameData(IReadOnlyList<ResourceItem> resources, IReadOnlyList<Building> buildings)
		{
			Resources = resources;
			Buildings = buildings;

			resourcesById = resources.ToDictionary(r => r.Id, StringComparer.Ordinal);
			buildingsById = buildings.ToDictionary(b => b.Id, StringComparer.Ordinal);
			itemsById = buildings
				.SelectMany(b => b.Items)
				.ToDictionary(i => i.Id, StringComparer.Ordinal);
		}

		/// <summary>
		/// Gets resources in the order they are declared in <c>resources.json</c>.
		/// </summary>
		internal IReadOnlyList<ResourceItem> Resources { get; }

		/// <summary>
		/// Gets buildings ordered by id (case-insensitively, as the files of the buildings are ordered).
		/// </summary>
		internal IReadOnlyList<Building> Buildings { get; }

		internal static GameData Get(ApiVersion apiVersion)
		{
			switch (apiVersion)
			{
				case ApiVersion.v2:
					return V2.Value;
				default:
					throw new NotSupportedException($"API version '{apiVersion}' is not supported. Use API v.2.");
			}
		}

		/// <summary>
		/// Builds game data from JSON texts. Used for the embedded data and in tests.
		/// </summary>
		/// <param name="resourcesJson">Content of <c>resources.json</c>.</param>
		/// <param name="buildingsJson">Source name (for error messages) and content of every building file.</param>
		/// <returns>Validated game data.</returns>
		internal static GameData Parse(string resourcesJson, IEnumerable<(string source, string json)> buildingsJson)
		{
			var resources = Deserialize<List<ResourceDto>>("resources.json", resourcesJson);
			var buildings = buildingsJson
				.Select(b => Deserialize<BuildingDto>(b.source, b.json))
				.ToList();

			return GameDataBuilder.Build(resources, buildings);
		}

		internal ResourceItem FindResource(string id)
		{
			return id != null && resourcesById.TryGetValue(id, out var resource) ? resource : null;
		}

		internal ProducibleItem FindItem(string id)
		{
			return id != null && itemsById.TryGetValue(id, out var item) ? item : null;
		}

		internal Building FindBuilding(string id)
		{
			return id != null && buildingsById.TryGetValue(id, out var building) ? building : null;
		}

		private static GameData LoadEmbedded(string versionSuffix)
		{
			var assembly = typeof(GameData).Assembly;
			var prefix = $"{MainNamespace}.Data{versionSuffix}.";
			var buildingsPrefix = prefix + "Buildings.";

			var resourcesJson = ReadText(assembly, prefix + "resources.json");

			var buildingSources = assembly.GetManifestResourceNames()
				.Where(n => n.StartsWith(buildingsPrefix, StringComparison.Ordinal) && n.EndsWith(".json", StringComparison.Ordinal))
				.OrderBy(n => n, StringComparer.Ordinal)
				.Select(n => (source: n.Substring(prefix.Length), json: ReadText(assembly, n)))
				.ToList();

			return Parse(resourcesJson, buildingSources);
		}

		private static string ReadText(System.Reflection.Assembly assembly, string resourceName)
		{
			using (var stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream == null)
				{
					throw new InvalidDataException($"Embedded resource '{resourceName}' was not found.");
				}

				using (var reader = new StreamReader(stream, Encoding.UTF8, true))
				{
					return reader.ReadToEnd();
				}
			}
		}

		private static T Deserialize<T>(string source, string json)
			where T : class
		{
			try
			{
				var result = JsonSerializer.Deserialize<T>(json, JsonOptions);

				if (result == null)
				{
					throw new InvalidDataException($"{source}: the file is empty.");
				}

				return result;
			}
			catch (JsonException ex)
			{
				throw new InvalidDataException($"{source}: {ex.Message}", ex);
			}
		}
	}
}
