using FamilyIslandHelper.Api.Helpers;
using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Items_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using Xunit;

namespace FamilyIslandHelper.Api.UnitTests
{
	public class ItemHelperTests : BaseTest
	{
		private ItemHelper itemHelper;

		public ItemHelperTests()
		{
			itemHelper = new ItemHelper(ApiVersion.v2);
		}

		[Theory]
		[InlineData(ApiVersion.v2)]
		public void When_GetResourcesNames_Then_AllResourcesHavePictures(ApiVersion apiVersion)
		{
			itemHelper = new ItemHelper(apiVersion);

			var actualResourcesNames = itemHelper.GetResourcesNames();

			foreach (var resourceName in actualResourcesNames)
			{
				Assert.NotNull(itemHelper.GetResourceImageByName(resourceName));
			}
		}

		[Theory]
		[InlineData("itemName")]
		public void When_FindItemByNameWithNullParameter_Then_ThrowsException(string expectedParamName)
		{
			var exception = Assert.Throws<ArgumentNullException>(() => itemHelper.FindItemByName(null));

			Assert.Equal(expectedParamName, exception.ParamName);
		}

		[Theory]
		[InlineData(ApiVersion.v2, "Lace", typeof(Lace))]
		[InlineData(ApiVersion.v2, "Stone", typeof(Stone))]
		public void When_FindItemByName_Then_ReturnCorrectItem(ApiVersion apiVersion, string itemName, Type expectedItemType)
		{
			itemHelper = new ItemHelper(apiVersion);

			var actualItem = itemHelper.FindItemByName(itemName);

			Assert.Equal(expectedItemType, actualItem.GetType());
		}

		[Theory]
		[InlineData("Cone123", null)]
		public void When_FindItemByNameForNotExistedItem_Then_ReturnNull(string itemName, Item? expectedItem)
		{
			var actualItem = itemHelper.FindItemByName(itemName);

			Assert.Equal(expectedItem, actualItem);
		}

		[Theory]
		[InlineData(ApiVersion.v2, "Lace", typeof(Models.Items_v2.Lace))]
		public void When_TryToCreateProducibleItem_Then_ReturnCorrectProducibleItem(ApiVersion apiVersion, string itemTypeString, Type expectedItemType)
		{
			itemHelper = new ItemHelper(apiVersion);

			var item = itemHelper.CreateProducibleItem(itemTypeString);

			Assert.Equal(expectedItemType, item.GetType());
		}

		[Theory]
		[InlineData("Stone", typeof(Stone))]
		public void When_TryToCreateResourceItem_Then_ReturnCorrectResourceItem(string itemTypeString, Type expectedItemType)
		{
			var item = itemHelper.CreateResourceItem(itemTypeString);

			Assert.Equal(expectedItemType, item.GetType());
		}

		public static IEnumerable<object[]> CompareItemsWithDifferentCount_TestData()
		{
			yield return new object[] { new Lace(), 2, new Lace(), 2,
				"2 'Шнурок' и 2 'Шнурок' равны по энергии." };

			yield return new object[] { new Rope(), 5, new Wattle(), 3,
				"3 'Плетень' выгоднее, чем 5 'Верёвка' по энергии." };

			yield return new object[] { new Stone(), 5, new Grass(), 3,
				"3 'Трава' выгоднее, чем 5 'Камень' по энергии." };
		}

		[Theory]
		[MemberData(nameof(CompareItemsWithDifferentCount_TestData))]
		public void When_Compare2ItemsWithDifferentCount_Then_ReturnCorrectValue(Item item1, int count1, Item item2, int count2, string expectedMessage)
		{
			var actualMessage = ItemHelper.CompareItems(item1, count1, item2, count2);

			Assert.Equal(expectedMessage, actualMessage);
		}

		public static IEnumerable<object[]> CompareItems_TestData()
		{
			yield return new object[] { new Lace(), new Lace(),
				"1 'Шнурок' и 1 'Шнурок' равны по энергии." };

			yield return new object[] { new Wattle(), new Rope(),
				"1 'Плетень' выгоднее, чем 1 'Верёвка' по энергии." };
		}

		[Theory]
		[MemberData(nameof(CompareItems_TestData))]
		public void When_Compare2Items_Then_ReturnCorrectValue(Item item1, Item item2, string expectedMessage)
		{
			var actualMessage = ItemHelper.CompareItems(item1, item2);

			Assert.Equal(expectedMessage, actualMessage);
		}

		public static IEnumerable<object[]> GetInfoAboutItem_TestData()
		{
			yield return new object[] { "Lace", 1, false,
				new List<string>
				{
					"Шнурок(6 энергии)"
				}
			};

			yield return new object[] { "Lace", 1, true,
				new List<string>
				{
					"Шнурок(6 энергии)",
					"",
					"Components:", "\tТрава(2 энергии) - 3 шт."
				}
			};
		}

		[Theory]
		[MemberData(nameof(GetInfoAboutItem_TestData))]
		public void When_GetInfoAboutItem_Then_ReturnCorrectValue(string itemName, int itemCount, bool showListOfComponents, List<string> expectedInfo)
		{
			var actualInfo = itemHelper.GetInfoAboutItem(itemName, itemCount, showListOfComponents);

			Assert.Equal(expectedInfo, actualInfo);
		}

		[Theory]
		[InlineData("Loom", "Lace", new[] { "Pictures_v2", "Items", "Loom", "Lace.png" })]
		public void When_GetBuildingImagePathByName_Then_ReturnCorrectValue(string buildingName, string itemName, string[] expectedPath)
		{
			var actualPath = itemHelper.GetItemImagePathByName(buildingName, itemName);

			Assert.Equal(Path.Combine(expectedPath), actualPath);
		}

		[Theory]
		[InlineData(16)]
		public void When_GetResourcesNames_Then_ReturnCorrectValue(int expectedNamesCount)
		{
			var actualNames = itemHelper.GetResourcesNames();

			Assert.Equal(expectedNamesCount, actualNames.Count);
		}

		public static IEnumerable<object[]> ApiVersions()
		{
			yield return new object[] { ApiVersion.v2 };
		}

		[Theory]
		[MemberData(nameof(ApiVersions))]
		public void When_GetAllItemsOfAllBuildings_Then_AllItemsHavePictures(ApiVersion apiVersion)
		{
			var buildingHelper = new BuildingHelper(apiVersion);
			var itemHelper = new ItemHelper(apiVersion);
			var missing = new List<string>();

			foreach (var building in buildingHelper.GetBuildingsNames())
			{
				foreach (var item in buildingHelper.GetItemsOfBuilding(building))
				{
					try
					{
						using var stream = itemHelper.GetItemImageStreamByName(building, item);
					}
					catch (NullReferenceException)
					{
						missing.Add($"{building}/{item}");
					}
				}
			}

			Assert.True(missing.Count == 0, "Missing pictures: " + string.Join(", ", missing));
		}
	}
}
