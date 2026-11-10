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

// 1. List buildings: Value is the class name, Name is the display name
foreach (var building in buildingHelper.GetBuildingsClasses())
{
    Console.WriteLine($"{building.Value} - {building.Name}");
}

// 2. List items a building can produce (class names)
var forgeItems = buildingHelper.GetItemsOfBuilding("Forge"); // Needle, Hammer, Glass, Lock

// 3. Find an item by its class name and read its recipe and energy cost
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

- Items, resources and buildings are looked up by **class name** (`"Rope"`, `"Stone"`, `"Forge"`). `FindItemByName` returns `null` if nothing is found, and throws `ArgumentNullException` for `null`.
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

Everything is plain C# classes plus PNG pictures, discovered by reflection from namespaces. After any change run:

```bash
dotnet test
```

The tests check that every building, resource and item has a picture, that there are no orphan pictures, that all components belong to the same API version, and that every item of a building points back to that building.

### Adding a new item

1. Add a class to the `<Building>Items.cs` file of the building that produces it (namespace `Models.Items_v2`). Inherit from `ProducibleItem` and set `Name`, `BuildingToCreate` and `Components`:

   ```csharp
   public class Barrel : ProducibleItem
   {
       public override string Name => "Бочка";
       public override Building BuildingToCreate => new Bench();

       public override List<(Item item, int count)> Components => new List<(Item item, int count)>
       {
           (new Wood(), 17),
           (new Nails(), 2),
           (new Clay(), 22)
       };
   }
   ```

2. Add `new Barrel()` to the `Items` list of the building class (`<Building>.cs`, namespace `Models.Buildings_v2`).
3. Add the picture `Pictures_v2/Items/<BuildingClassName>/<ItemClassName>.png` (for the example: `Pictures_v2/Items/Bench/Barrel.png`). `*.png` files in `Pictures_v2` are embedded automatically.

Class names must be unique, because items are found by class name.

### Adding a new resource

1. Add a class to `Resources_v2.cs`: inherit from `ResourceItem` and set `EnergyCost`, or from `ZeroCostResourceItem` for resources that cost no energy.
2. Add the picture `Pictures_v2/Resources/<ClassName>.png`.

### Adding a new building

1. Add a class to `Models.Buildings_v2` (inherit from `Building`, set `Name` and `Items`) and a `<Building>Items.cs` file with its items.
2. Add the picture `Pictures_v2/Buildings/<ClassName>.png`.
3. Update the expected buildings count in `BuildingHelperTests` and add the building to `ProducibleItemTests.GetProperties_TestData`.

### Adding a new API version

Versions are currently not data-driven: `BaseHelper` hardcodes the `_v2` suffix for the namespaces (`Models.Buildings_v2`, `Models.Items_v2`, `Models.Resources_v2`) and the `Pictures_v2` folder. A new version means a new enum value in `ApiVersion`, new `_vN` namespaces and picture folder, mapping the suffix from the version in `BaseHelper`, and extending the tests.

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

The library targets `netstandard2.0`; the unit tests target .NET 10. CI (GitHub Actions) builds, runs the tests with coverage and packs the NuGet package.

## License

[Apache-2.0](LICENSE)

## Authors

- [@Gruuuubik](https://www.github.com/Gruuuubik)
