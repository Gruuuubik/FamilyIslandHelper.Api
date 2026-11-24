using FamilyIslandHelper.Api.Helpers;
using FamilyIslandHelper.Api.Models.Abstract;
using Xunit;

namespace FamilyIslandHelper.Api.UnitTests
{
	public class ProducibleItemTests : BaseTest
	{
		private static readonly ItemHelper ItemHelper = new ItemHelper(ApiVersion.v2);

		private static ProducibleItem GetItem(string itemName) => ItemHelper.CreateProducibleItem(itemName);

		public static IEnumerable<object[]> ComponentsInfo_TestData()
		{
			yield return new object[] { GetItem("Rope"), 1, new List<string>
			{
				"\tТрава(2 энергии) - 14 шт.",
				"\tКамень(10 энергии) - 8 шт."
			} };
			yield return new object[] { GetItem("Rope"), 3, new List<string>
			{
				"\tТрава(6 энергии) - 42 шт.",
				"\tКамень(30 энергии) - 24 шт."
			} };
		}

		[Theory]
		[MemberData(nameof(ComponentsInfo_TestData))]
		public void Given_ProducibleItem_When_GetComponentsInfo_Then_ReturnCorrectValue(ProducibleItem producibleItem, int itemsCount, List<string> expectedComponentsInfo)
		{
			var actualTotalProduceTime = producibleItem.ComponentsInfo(0, itemsCount);

			Assert.Equal(expectedComponentsInfo, actualTotalProduceTime);
		}

		public static IEnumerable<object[]> ToString_TestData()
		{
			yield return new object[] { GetItem("Rope"), 1, "Верёвка(108 энергии)" };
			yield return new object[] { GetItem("Rope"), 3, "Верёвка(324 энергии)" };
		}

		[Theory]
		[MemberData(nameof(ToString_TestData))]
		public void Given_ProducibleItem_When_ToString_Then_ReturnCorrectValue(ProducibleItem producibleItem, int itemsCount, string expectedStringResult)
		{
			var actualStringResult = producibleItem.ToString(itemsCount);

			Assert.Equal(expectedStringResult, actualStringResult);
		}

		public static IEnumerable<object[]> GetProperties_TestData()
		{
			foreach (var buildingName in new BuildingHelper(ApiVersion.v2).GetBuildingsNames())
			{
				yield return new object[] { buildingName };
			}
		}

		[Theory]
		[MemberData(nameof(GetProperties_TestData))]
		public void Given_ProducibleItem_When_GetProperties_Then_ReturnNotNullValues(string buildingName)
		{
			var producibleItems = new BuildingHelper(ApiVersion.v2).CreateBuilding(buildingName).Items;

			Assert.NotEmpty(producibleItems);
			Assert.All(producibleItems, (item) => Assert.NotNull(item.Name));
			Assert.All(producibleItems, (item) => Assert.NotEmpty(item.Components));
			Assert.All(producibleItems, (item) => Assert.Equal(buildingName, item.BuildingToCreate.Id));
		}

		[Fact]
		public void Given_ProducibleItem_When_ChangeReturnedComponents_Then_ItemIsNotChanged()
		{
			var item = GetItem("Rope");
			var expectedCount = item.Components.Count;

			item.Components.Clear();

			Assert.Equal(expectedCount, item.Components.Count);
		}
	}
}
