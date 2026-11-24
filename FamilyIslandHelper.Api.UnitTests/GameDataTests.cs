using FamilyIslandHelper.Api.Data;
using FamilyIslandHelper.Api.Helpers;
using Xunit;

namespace FamilyIslandHelper.Api.UnitTests
{
	/// <summary>
	/// Tests of the game data (JSON files in the Data_v2 folder) and of the rules the data is validated by.
	/// </summary>
	public class GameDataTests : BaseTest
	{
		private const string ValidResources = @"[
			{ ""id"": ""Wood"", ""name"": ""Дерево"", ""energyCost"": 4 },
			{ ""id"": ""Egg"", ""name"": ""Яйцо"", ""energyCost"": 0 }
		]";

		private const string ValidBuilding = @"{
			""id"": ""Bench"",
			""name"": ""Лавка"",
			""items"": [
				{ ""id"": ""Plank"", ""name"": ""Доска"", ""components"": [ { ""item"": ""Wood"", ""count"": 3 } ] },
				{ ""id"": ""Cake"", ""name"": ""Пирог"", ""components"": [ { ""item"": ""Plank"", ""count"": 2 }, { ""item"": ""Egg"", ""count"": 1 } ] }
			]
		}";

		private static GameData Parse(string resourcesJson, params string[] buildingsJson)
		{
			return GameData.Parse(resourcesJson, buildingsJson.Select((json, i) => ($"Buildings/{i}.json", json)));
		}

		[Fact]
		public void When_LoadEmbeddedData_Then_AllComponentsAreResolved()
		{
			var buildingHelper = new BuildingHelper(ApiVersion.v2);
			var itemHelper = new ItemHelper(ApiVersion.v2);

			foreach (var buildingName in buildingHelper.GetBuildingsNames())
			{
				foreach (var item in buildingHelper.CreateBuilding(buildingName).Items)
				{
					Assert.NotEmpty(item.Components);

					foreach (var (component, count) in item.Components)
					{
						Assert.True(count > 0, $"{buildingName}/{item.Id}: '{component.Id}' has not positive count.");
						Assert.NotNull(itemHelper.FindItemByName(component.Id));
					}
				}
			}
		}

		[Fact]
		public void When_LoadEmbeddedData_Then_IdsOfItemsAndResourcesAreUnique()
		{
			var buildingHelper = new BuildingHelper(ApiVersion.v2);
			var itemHelper = new ItemHelper(ApiVersion.v2);

			var ids = buildingHelper.GetBuildingsNames()
				.SelectMany(b => buildingHelper.GetItemsOfBuilding(b))
				.Concat(itemHelper.GetResourcesNames())
				.ToList();

			Assert.Equal(ids.Count, ids.Distinct().Count());
		}

		[Fact]
		public void When_LoadEmbeddedData_Then_IdsContainOnlyLatinLettersAndDigits()
		{
			var buildingHelper = new BuildingHelper(ApiVersion.v2);
			var itemHelper = new ItemHelper(ApiVersion.v2);

			var ids = buildingHelper.GetBuildingsNames()
				.Concat(buildingHelper.GetBuildingsNames().SelectMany(b => buildingHelper.GetItemsOfBuilding(b)))
				.Concat(itemHelper.GetResourcesNames());

			// Ids are typed in code and are names of the picture files, so look-alike letters (for example Cyrillic "С") are not allowed.
			Assert.All(ids, id => Assert.Matches("^[A-Za-z0-9]+$", id));
		}

		[Fact]
		public void When_GetNotSupportedVersion_Then_ThrowsException()
		{
			Assert.Throws<NotSupportedException>(() => GameData.Get((ApiVersion)42));
		}

		[Fact]
		public void When_ParseValidData_Then_ReturnCorrectData()
		{
			var data = Parse(ValidResources, ValidBuilding);

			Assert.Equal(["Wood", "Egg"], data.Resources.Select(r => r.Id));
			Assert.Equal(4, data.FindResource("Wood")!.EnergyCost);
			// a resource that costs no energy is shown by its name only
			Assert.Equal("Яйцо", data.FindResource("Egg")!.ToString(3));
			Assert.Equal("Дерево(12 энергии)", data.FindResource("Wood")!.ToString(3));

			var building = data.FindBuilding("Bench")!;
			Assert.Equal(["Plank", "Cake"], building.Items.Select(i => i.Id));

			// 2 planks (3 wood each, 4 energy each) and a free egg
			Assert.Equal(24, data.FindItem("Cake")!.ProduceEnergyCost);
		}

		[Fact]
		public void When_ParseDataWithCommentsAndTrailingCommas_Then_ReturnData()
		{
			var resources = @"[
				// a comment
				{ ""id"": ""Wood"", ""name"": ""Дерево"", ""energyCost"": 4, },
			]";
			var building = @"{ ""id"": ""Bench"", ""name"": ""Лавка"", ""items"": [
				{ ""id"": ""Plank"", ""name"": ""Доска"", ""components"": [ { ""item"": ""Wood"", ""count"": 3 }, ] },
			] }";

			var data = Parse(resources, building);

			Assert.Equal(12, data.FindItem("Plank")!.ProduceEnergyCost);
		}

		[Fact]
		public void When_ParseBuildingsInRandomOrder_Then_BuildingsAreOrderedById()
		{
			var second = ValidBuilding.Replace("Bench", "Anvil").Replace("Plank", "Plank2").Replace("Cake", "Cake2");

			var data = Parse(ValidResources, ValidBuilding, second);

			Assert.Equal(["Anvil", "Bench"], data.Buildings.Select(b => b.Id));
		}

		[Fact]
		public void When_FindNotExistedOrNullId_Then_ReturnNull()
		{
			var data = Parse(ValidResources, ValidBuilding);

			Assert.Null(data.FindItem("Cone123"));
			Assert.Null(data.FindItem(null));
			Assert.Null(data.FindResource("Plank"));
			Assert.Null(data.FindBuilding("Cone123"));
		}

		public static IEnumerable<object[]> InvalidData_TestData()
		{
			yield return new object[]
			{
				"duplicated resource id",
				@"[ { ""id"": ""Wood"", ""name"": ""A"", ""energyCost"": 1 }, { ""id"": ""Wood"", ""name"": ""B"", ""energyCost"": 2 } ]",
				ValidBuilding,
				"Duplicate id 'Wood'"
			};

			yield return new object[]
			{
				"item id equals to resource id",
				ValidResources,
				ValidBuilding.Replace("Plank", "Wood"),
				"Duplicate id 'Wood'"
			};

			yield return new object[]
			{
				"unknown component",
				ValidResources,
				ValidBuilding.Replace(@"""item"": ""Egg""", @"""item"": ""Milk"""),
				"unknown component 'Milk'"
			};

			yield return new object[]
			{
				"not positive count",
				ValidResources,
				ValidBuilding.Replace(@"""count"": 3", @"""count"": 0"),
				"must have a positive 'count'"
			};

			yield return new object[]
			{
				"item without components",
				ValidResources,
				@"{ ""id"": ""Bench"", ""name"": ""A"", ""items"": [ { ""id"": ""Plank"", ""name"": ""B"", ""components"": [] } ] }",
				"Item 'Plank' has no components"
			};

			yield return new object[]
			{
				"building without items",
				ValidResources,
				@"{ ""id"": ""Bench"", ""name"": ""A"", ""items"": [] }",
				"Building 'Bench' has no items"
			};

			yield return new object[]
			{
				"building without name",
				ValidResources,
				ValidBuilding.Replace(@"""name"": ""Лавка"",", string.Empty),
				"Building 'Bench' has no 'name'"
			};

			yield return new object[]
			{
				"resource without energy cost",
				@"[ { ""id"": ""Wood"", ""name"": ""A"" } ]",
				ValidBuilding.Replace("Egg", "Wood"),
				"has no 'energyCost'"
			};

			yield return new object[]
			{
				"negative energy cost",
				@"[ { ""id"": ""Wood"", ""name"": ""A"", ""energyCost"": -1 } ]",
				ValidBuilding.Replace("Egg", "Wood"),
				"negative 'energyCost'"
			};

			yield return new object[]
			{
				"circular recipe",
				ValidResources,
				@"{ ""id"": ""Bench"", ""name"": ""A"", ""items"": [
					{ ""id"": ""A"", ""name"": ""A"", ""components"": [ { ""item"": ""B"", ""count"": 1 } ] },
					{ ""id"": ""B"", ""name"": ""B"", ""components"": [ { ""item"": ""A"", ""count"": 1 } ] }
				] }",
				"Circular recipe: A -> B -> A"
			};

			yield return new object[]
			{
				"item which contains itself",
				ValidResources,
				@"{ ""id"": ""Bench"", ""name"": ""A"", ""items"": [
					{ ""id"": ""A"", ""name"": ""A"", ""components"": [ { ""item"": ""A"", ""count"": 1 } ] }
				] }",
				"Circular recipe: A -> A"
			};
		}

		[Theory]
		[MemberData(nameof(InvalidData_TestData))]
		public void When_ParseInvalidData_Then_ThrowsExceptionWithDescription(string caseName, string resourcesJson, string buildingJson, string expectedMessagePart)
		{
			var exception = Assert.Throws<InvalidDataException>(() => Parse(resourcesJson, buildingJson));

			Assert.True(
				exception.Message.Contains(expectedMessagePart),
				$"Case '{caseName}': message '{exception.Message}' does not contain '{expectedMessagePart}'.");
		}

		[Fact]
		public void When_ParseDataWithSeveralProblems_Then_ReportAllOfThem()
		{
			var building = ValidBuilding
				.Replace(@"""item"": ""Egg""", @"""item"": ""Milk""")
				.Replace(@"""count"": 3", @"""count"": 0");

			var exception = Assert.Throws<InvalidDataException>(() => Parse(ValidResources, building));

			Assert.Contains("unknown component 'Milk'", exception.Message);
			Assert.Contains("must have a positive 'count'", exception.Message);
		}

		[Fact]
		public void When_ParseBrokenJson_Then_ExceptionContainsSourceName()
		{
			var exception = Assert.Throws<InvalidDataException>(() =>
				GameData.Parse(ValidResources, [("Buildings/Broken.json", "{ \"id\": ")]));

			Assert.StartsWith("Buildings/Broken.json:", exception.Message);
		}
	}
}
