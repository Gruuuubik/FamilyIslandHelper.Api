using FamilyIslandHelper.Api.Models.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FamilyIslandHelper.Api.Data
{
	/// <summary>
	/// Validates the data read from JSON and turns it into model objects.
	/// All problems found are reported at once.
	/// </summary>
	internal static class GameDataBuilder
	{
		internal static GameData Build(IReadOnlyList<ResourceDto> resourceDtos, IReadOnlyList<BuildingDto> buildingDtos)
		{
			var errors = new List<string>();
			var usedIds = new HashSet<string>(StringComparer.Ordinal);

			var resources = BuildResources(resourceDtos, usedIds, errors);
			var resourcesById = resources.ToDictionary(r => r.Id, StringComparer.Ordinal);

			var buildings = new List<DataBuilding>();
			var buildingIds = new HashSet<string>(StringComparer.Ordinal);
			var itemsById = new Dictionary<string, DataProducibleItem>(StringComparer.Ordinal);
			var itemsInfo = new List<(DataProducibleItem item, ItemDto dto, DataBuilding building)>();

			foreach (var buildingDto in buildingDtos)
			{
				if (string.IsNullOrWhiteSpace(buildingDto.Id))
				{
					errors.Add("A building without 'id'.");
					continue;
				}

				if (!buildingIds.Add(buildingDto.Id))
				{
					errors.Add($"Duplicate building id '{buildingDto.Id}'.");
					continue;
				}

				if (string.IsNullOrWhiteSpace(buildingDto.Name))
				{
					errors.Add($"Building '{buildingDto.Id}' has no 'name'.");
				}

				var building = new DataBuilding(buildingDto.Id, buildingDto.Name);
				buildings.Add(building);

				if (buildingDto.Items == null || buildingDto.Items.Count == 0)
				{
					errors.Add($"Building '{buildingDto.Id}' has no items.");
					continue;
				}

				var buildingItems = new List<ProducibleItem>();

				foreach (var itemDto in buildingDto.Items)
				{
					if (string.IsNullOrWhiteSpace(itemDto.Id))
					{
						errors.Add($"Building '{buildingDto.Id}' has an item without 'id'.");
						continue;
					}

					if (!usedIds.Add(itemDto.Id))
					{
						errors.Add($"Duplicate id '{itemDto.Id}' (building '{buildingDto.Id}').");
						continue;
					}

					if (string.IsNullOrWhiteSpace(itemDto.Name))
					{
						errors.Add($"Item '{itemDto.Id}' has no 'name'.");
					}

					var item = new DataProducibleItem(itemDto.Id, itemDto.Name);
					itemsById.Add(item.Id, item);
					buildingItems.Add(item);
					itemsInfo.Add((item, itemDto, building));
				}

				building.SetItems(buildingItems);
			}

			foreach (var (item, dto, building) in itemsInfo)
			{
				item.Initialize(building, ResolveComponents(dto, resourcesById, itemsById, errors));
			}

			DetectCycles(itemsById.Values, errors);

			if (errors.Count > 0)
			{
				throw new InvalidDataException(
					"Invalid game data:" + Environment.NewLine +
					string.Join(Environment.NewLine, errors.Select(e => " - " + e)));
			}

			return new GameData(
				resources,
				buildings.OrderBy(b => b.Id, StringComparer.OrdinalIgnoreCase).Cast<Building>().ToList());
		}

		private static List<ResourceItem> BuildResources(IReadOnlyList<ResourceDto> resourceDtos, HashSet<string> usedIds, List<string> errors)
		{
			var resources = new List<ResourceItem>();

			foreach (var dto in resourceDtos)
			{
				if (string.IsNullOrWhiteSpace(dto.Id))
				{
					errors.Add("A resource without 'id'.");
					continue;
				}

				if (!usedIds.Add(dto.Id))
				{
					errors.Add($"Duplicate id '{dto.Id}' (resource).");
					continue;
				}

				if (string.IsNullOrWhiteSpace(dto.Name))
				{
					errors.Add($"Resource '{dto.Id}' has no 'name'.");
				}

				if (!dto.EnergyCost.HasValue)
				{
					errors.Add($"Resource '{dto.Id}' has no 'energyCost' (use 0 for a resource that costs no energy).");
				}
				else if (dto.EnergyCost.Value < 0)
				{
					errors.Add($"Resource '{dto.Id}' has a negative 'energyCost'.");
				}

				resources.Add(new DataResourceItem(dto.Id, dto.Name, dto.EnergyCost ?? 0));
			}

			return resources;
		}

		private static List<(Item item, int count)> ResolveComponents(
			ItemDto dto,
			Dictionary<string, ResourceItem> resourcesById,
			Dictionary<string, DataProducibleItem> itemsById,
			List<string> errors)
		{
			var components = new List<(Item item, int count)>();

			if (dto.Components == null || dto.Components.Count == 0)
			{
				errors.Add($"Item '{dto.Id}' has no components.");
				return components;
			}

			foreach (var component in dto.Components)
			{
				if (string.IsNullOrWhiteSpace(component.Item))
				{
					errors.Add($"Item '{dto.Id}' has a component without 'item'.");
					continue;
				}

				if (component.Count <= 0)
				{
					errors.Add($"Item '{dto.Id}': component '{component.Item}' must have a positive 'count'.");
				}

				if (resourcesById.TryGetValue(component.Item, out var resource))
				{
					components.Add((resource, component.Count));
				}
				else if (itemsById.TryGetValue(component.Item, out var producibleItem))
				{
					components.Add((producibleItem, component.Count));
				}
				else
				{
					errors.Add($"Item '{dto.Id}': unknown component '{component.Item}'.");
				}
			}

			return components;
		}

		private static void DetectCycles(IEnumerable<DataProducibleItem> items, List<string> errors)
		{
			// 1 - being visited, 2 - done
			var states = new Dictionary<string, int>(StringComparer.Ordinal);
			var path = new List<string>();

			void Visit(DataProducibleItem item)
			{
				if (states.TryGetValue(item.Id, out var state))
				{
					if (state == 1)
					{
						var cycleStart = path.IndexOf(item.Id);
						var cycle = path.Skip(cycleStart).Concat(new[] { item.Id });
						errors.Add("Circular recipe: " + string.Join(" -> ", cycle) + ".");
					}

					return;
				}

				states[item.Id] = 1;
				path.Add(item.Id);

				foreach (var (component, _) in item.RawComponents)
				{
					if (component is DataProducibleItem producibleComponent)
					{
						Visit(producibleComponent);
					}
				}

				path.RemoveAt(path.Count - 1);
				states[item.Id] = 2;
			}

			foreach (var item in items)
			{
				Visit(item);
			}
		}
	}
}
