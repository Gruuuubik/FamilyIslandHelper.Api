using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Items_v2;
using Xunit;

namespace FamilyIslandHelper.Api.UnitTests
{
	public class ProducibleItemTests : BaseTest
	{
		public static IEnumerable<object[]> ComponentsInfo_TestData()
		{
			yield return new object[] { new Rope(), 1, new List<string>
			{
				"\tТрава(2 энергии) - 14 шт.",
				"\tКамень(10 энергии) - 8 шт."
			} };
			yield return new object[] { new Rope(), 3, new List<string>
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
			yield return new object[] { new Rope(), 1, "Верёвка(108 энергии)" };
			yield return new object[] { new Rope(), 3, "Верёвка(324 энергии)" };
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
			yield return new object[] { new Models.Buildings_v2.AlchemistLaboratory().Items };
			yield return new object[] { new Models.Buildings_v2.Bench().Items };
			yield return new object[] { new Models.Buildings_v2.CarpentryWorkshop().Items };
			yield return new object[] { new Models.Buildings_v2.Forge().Items };
			yield return new object[] { new Models.Buildings_v2.GlassWorkshop().Items };
			yield return new object[] { new Models.Buildings_v2.HobbyCorner().Items };
			yield return new object[] { new Models.Buildings_v2.JewelryWorkshop().Items };
			yield return new object[] { new Models.Buildings_v2.Kiln().Items };
			yield return new object[] { new Models.Buildings_v2.Knocker().Items };
			yield return new object[] { new Models.Buildings_v2.Loom().Items };
			yield return new object[] { new Models.Buildings_v2.MeteoriteForge().Items };
			yield return new object[] { new Models.Buildings_v2.Mill().Items };
			yield return new object[] { new Models.Buildings_v2.Mixer().Items };
			yield return new object[] { new Models.Buildings_v2.Pottery().Items };
			yield return new object[] { new Models.Buildings_v2.Sawmill().Items };
			yield return new object[] { new Models.Buildings_v2.SewingWorkshop().Items };
			yield return new object[] { new Models.Buildings_v2.ShamanWorkshop().Items };
			yield return new object[] { new Models.Buildings_v2.Smelter().Items };
			yield return new object[] { new Models.Buildings_v2.Tannery().Items };
			yield return new object[] { new Models.Buildings_v2.Workshop().Items };
		}

		[Theory]
		[MemberData(nameof(GetProperties_TestData))]
		public void Given_ProducibleItem_When_GetProperties_Then_ReturnNotNullValues(List<ProducibleItem> producibleItems)
		{
			Assert.All(producibleItems, (item) => Assert.NotNull(item.Name));
			Assert.All(producibleItems, (item) => Assert.NotNull(item.BuildingToCreate));
			Assert.All(producibleItems, (item) => Assert.NotEmpty(item.Components));
		}
	}
}
