# FamilyIslandHelper.Api

Cross-platform `netstandard2.0` library with an API for the game [Family Island](https://play.google.com/store/apps/details?id=com.MelsoftGames.FamilyIslandFarm&pcampaignid=web_share): buildings, craftable items, recipes and energy cost calculations.

## Installation

```bash
dotnet add package FamilyIslandHelper.Api
```

## Quick start

Only `ApiVersion.v2` (game data after the 10.2024 update) is supported.

```csharp
using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Helpers;
using FamilyIslandHelper.Api.Models.Abstract;

var buildingHelper = new BuildingHelper(ApiVersion.v2);
var itemHelper = new ItemHelper(ApiVersion.v2);

// Items of a building (class names)
var forgeItems = buildingHelper.GetItemsOfBuilding("Forge");

// Recipe and total energy cost of an item
if (itemHelper.FindItemByName("Rope") is ProducibleItem rope)
{
    Console.WriteLine($"{rope.Name}: {rope.ProduceEnergyCost} energy");

    foreach (var (component, count) in rope.Components)
    {
        Console.WriteLine($"  {component.Name} x{count}");
    }
}

// Compare two items by energy
Console.WriteLine(ItemHelper.CompareItems(itemHelper.FindItemByName("Wattle"), itemHelper.FindItemByName("Rope")));
```

Items, resources and buildings are looked up by class name. Display names and generated texts are in Russian. Item, resource and building pictures are embedded as PNG (`Get...ImageStreamByName` methods).

## Links

- Source code, contribution guide and supported versions: [GitHub](https://github.com/Gruuuubik/FamilyIslandHelper.Api)
- Release notes: [Releases](https://github.com/Gruuuubik/FamilyIslandHelper.Api/releases)
- Web application built on this library: [familyislandhelper.runasp.net](https://familyislandhelper.runasp.net/)

License: [Apache-2.0](https://github.com/Gruuuubik/FamilyIslandHelper.Api/blob/api/LICENSE)
