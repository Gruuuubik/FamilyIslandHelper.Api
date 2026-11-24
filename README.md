# FamilyIslandHelper.Api

API for the game [Family Island](https://play.google.com/store/apps/details?id=com.MelsoftGames.FamilyIslandFarm&pcampaignid=web_share): buildings, craftable items, recipes and energy cost calculations, packaged as a cross-platform `netstandard2.0` .NET library.

[![Static Badge](https://img.shields.io/badge/Gruuuubik-FamilyIslandHelper-FamilyIslandHelper)](https://github.com/Gruuuubik/FamilyIslandHelper.Api)
[![NuGet](https://img.shields.io/nuget/v/FamilyIslandHelper.Api.svg)](https://www.nuget.org/packages/FamilyIslandHelper.Api/)
[![Api](https://github.com/Gruuuubik/FamilyIslandHelper.Api/actions/workflows/api.yml/badge.svg?branch=api)](https://github.com/Gruuuubik/FamilyIslandHelper.Api/actions/workflows/api.yml)

## Installation

```bash
dotnet add package FamilyIslandHelper.Api
```

Or search for `FamilyIslandHelper.Api` in the NuGet Package Manager in Visual Studio.

## Quick start

All entry points take an `ApiVersion`. Only `ApiVersion.v2` is supported (see [Supported versions](#supported-versions)).

```csharp
using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Helpers;
using FamilyIslandHelper.Api.Models.Abstract;

var buildingHelper = new BuildingHelper(ApiVersion.v2);
var itemHelper = new ItemHelper(ApiVersion.v2);

// 1. List buildings: Value is the id, Name is the display name
foreach (var building in buildingHelper.GetBuildingsClasses())
{
    Console.WriteLine($"{building.Value} - {building.Name}");
}

// 2. List items a building can produce (ids)
var forgeItems = buildingHelper.GetItemsOfBuilding("Forge"); // Needle, Hammer, Glass, Lock

// 3. Find an item by its id and read its recipe and energy cost
if (itemHelper.FindItemByName("Rope") is ProducibleItem rope)
{
    Console.WriteLine($"{rope.Name}: {rope.ProduceEnergyCost} energy"); // total, including nested components

    foreach (var (component, count) in rope.Components)
    {
        Console.WriteLine($"  {component.Name} x{count}");
    }
}

// 4. Ready-made text output: item, energy and a tree of components
var info = itemHelper.GetInfoAboutItem("Lace", itemCount: 1, showListOfComponents: true);
// Шнурок(6 энергии)
//
// Components:
//     Трава(2 энергии) - 3 шт.

// 5. Compare two items by energy
Console.WriteLine(ItemHelper.CompareItems(itemHelper.FindItemByName("Wattle"), itemHelper.FindItemByName("Rope")));
// 1 'Плетень' выгоднее, чем 1 'Верёвка' по энергии.
```

Notes:

- Items, resources and buildings are looked up by **id** (`"Rope"`, `"Stone"`, `"Forge"`), which is also available as `Item.Id` and `Building.Id`. `FindItemByName` returns `null` if nothing is found, and throws `ArgumentNullException` for `null`. `CreateBuilding`, `CreateProducibleItem` and `CreateResourceItem` throw `ArgumentException` for an unknown id.
- Items and buildings are immutable objects created once from the game data, so the same instance is returned on every call.
- Item and building display names (`Name`) and the generated text are in Russian.
- Resources (`ResourceItem`) have a fixed `EnergyCost`. Items (`ProducibleItem`) calculate `ProduceEnergyCost` recursively from their `Components`.

### Pictures

Pictures are embedded in the assembly as PNG files:

```csharp
using var stream = itemHelper.GetItemImageStreamByName("Forge", "Needle");        // MemoryStream
using var resource = itemHelper.GetResourceImageStreamByName("Stone");            // MemoryStream
using var building = buildingHelper.GetBuildingImageStreamByName("Forge");        // MemoryStream
```

`Get...ImageByName` methods return `System.Drawing.Image`; on non-Windows platforms prefer the `...ImageStreamByName` methods.

## Supported versions

| `ApiVersion` | Data | Status |
|--------------|------|--------|
| `v2`         | Game data after the 10.2024 update | Supported |
| `v1`         | Game data before the 10.2024 update | Not supported (obsolete, throws `NotSupportedException`) |

## Contributing data

The game data (resources, buildings, items and recipes) is stored as JSON files that are embedded into the assembly. No C# code has to be changed to add or fix data:

```text
Data_v2/
  resources.json          resources and their energy costs
  Buildings/<Id>.json     one file per building: its items and their recipes
Pictures_v2/
  Buildings/<BuildingId>.png
  Items/<BuildingId>/<ItemId>.png
  Resources/<ResourceId>.png
```

An `id` is a stable identifier that is used in code (`FindItemByName("Rope")`) and in the name of the picture, so it must be unique among all items and resources and should not be changed. `name` is the text shown to the player.

After any change run:

```bash
dotnet test
```

The data is validated when it is loaded for the first time. A mistake throws `InvalidDataException` that lists **all** problems at once: duplicate ids, unknown components, not positive counts, items without components, circular recipes and so on. The tests also check that every building, resource and item has a picture and that there are no orphan pictures.

### Adding a new item

1. Add an object to `items` of the building that produces it in `Data_v2/Buildings/<BuildingId>.json`:

   ```json
   {
     "id": "Barrel",
     "name": "Бочка",
     "components": [
       { "item": "Wood", "count": 17 },
       { "item": "Nails", "count": 2 },
       { "item": "Clay", "count": 22 }
     ]
   }
   ```

   `item` is the id of a resource or of another item (of any building). The order of `items` is the order the items are returned in.
2. Add the picture `Pictures_v2/Items/<BuildingId>/<ItemId>.png` (for the example: `Pictures_v2/Items/Bench/Barrel.png`).

### Adding a new resource

Add a line to `Data_v2/resources.json` and the picture `Pictures_v2/Resources/<Id>.png`:

```json
{ "id": "Stone", "name": "Камень", "energyCost": 10 },
{ "id": "Egg", "name": "Яйцо", "energyCost": 0 }
```

`energyCost` is required. A resource with `energyCost` 0 costs no energy and is shown without it.

### Adding a new building

1. Create `Data_v2/Buildings/<BuildingId>.json` (it is embedded automatically):

   ```json
   {
     "id": "Bench",
     "name": "Лавка",
     "items": [ ]
   }
   ```

   Fill `items` the same way as for [a new item](#adding-a-new-item). Buildings are returned ordered by id, ignoring case.
2. Add the picture `Pictures_v2/Buildings/<BuildingId>.png`.
3. Update the expected buildings count in `BuildingHelperTests`.

### Adding a new API version

The data and pictures of a version live in `Data_vN` and `Pictures_vN` folders. Versions are not fully data-driven yet: `BaseHelper` hardcodes the `_v2` suffix for pictures and `GameData.Get` has a switch for the data. A new version means a new value in `ApiVersion`, new `Data_vN` and `Pictures_vN` folders, a new case in `GameData.Get` and the suffix mapping in `BaseHelper`, and extending the tests.

## Where used

- Desktop application: [FamilyIslandHelper.Desktop](https://github.com/Gruuuubik/FamilyIslandHelper.Desktop)
- Web application: [FamilyIslandHelper.Web](https://github.com/Gruuuubik/FamilyIslandHelper.Web), deployed at [familyislandhelper.runasp.net](https://familyislandhelper.runasp.net/)

## Development

```bash
git clone https://github.com/Gruuuubik/FamilyIslandHelper.Api.git
cd FamilyIslandHelper.Api
dotnet build
dotnet test
```

The library targets `netstandard2.0` (JSON is read with `System.Text.Json`); the unit tests target .NET 10. CI (GitHub Actions) builds, runs the tests with coverage and packs the NuGet package.

## License

[Apache-2.0](LICENSE)

## Authors

- [@Gruuuubik](https://www.github.com/Gruuuubik)
