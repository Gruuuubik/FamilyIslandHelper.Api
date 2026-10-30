using FamilyIslandHelper.Api.Helpers;
using Xunit;

namespace FamilyIslandHelper.Api.UnitTests
{
	public class BuildingHelperTests : BaseTest
	{
		private BuildingHelper buildingHelper;

		[Theory]
		[InlineData(ApiVersion.v1, 13)]
		[InlineData(ApiVersion.v2, 20)]
		public void When_GetBuildingsClasses_Then_ReturnCorrectCollection(ApiVersion apiVersion, int expectedCount)
		{
			buildingHelper = new BuildingHelper(apiVersion);

			var actualBuildingsClasses = buildingHelper.GetBuildingsClasses();

			Assert.Equal(expectedCount, actualBuildingsClasses.Count);
			Assert.All(actualBuildingsClasses, (bc) => Assert.NotNull(bc));
		}

		[Theory]
		[InlineData(ApiVersion.v1, "Mill", new[] { "GoatFood", "ChickenFood", "Ocher", "Flour", "SunflowerOil", "Syrup", "CowFood" })]
		[InlineData(ApiVersion.v2, "Forge", new[] { "Needle", "Hammer", "Glass", "Lock" })]
		public void When_TryToGetItemsOfBuilding_Then_ReturnCorrectItems(ApiVersion apiVersion, string buildingName, IEnumerable<string> expectedItems)
		{
			buildingHelper = new BuildingHelper(apiVersion);

			var actualItems = buildingHelper.GetItemsOfBuilding(buildingName);

			Assert.Equal(expectedItems, actualItems);
		}

		[Theory]
		[InlineData("buildingName")]
		public void When_CreateBuildingWithNullParameter_Then_ThrowsException(string expectedParamName)
		{
			buildingHelper = new BuildingHelper(ApiVersion.v1);

			var exception = Assert.Throws<ArgumentNullException>(() => buildingHelper.CreateBuilding(null));

			Assert.Equal(expectedParamName, exception.ParamName);
		}

		[Theory]
		[InlineData(ApiVersion.v1)]
		[InlineData(ApiVersion.v2)]
		public void When_CheckBuildingToCreateForAllItems_Then_ReturnCorrectValue(ApiVersion apiVersion)
		{
			buildingHelper = new BuildingHelper(apiVersion);
			var buildingNames = buildingHelper.GetBuildingsNames();

			foreach (var buildingName in buildingNames)
			{
				var building = buildingHelper.CreateBuilding(buildingName);
				var items = building.Items;

				Assert.Equal(building.Name, items.Select(i => i.BuildingToCreate.Name).Distinct().SingleOrDefault());
			}
		}

		public static IEnumerable<object[]> ApiVersions_TestData()
		{
			yield return new object[] { ApiVersion.v1 };
			yield return new object[] { ApiVersion.v2 };
		}

		[Theory]
		[MemberData(nameof(ApiVersions_TestData))]
		public void When_GetAllBuildingsNames_Then_AllBuildingsHavePictures(ApiVersion apiVersion)
		{
			buildingHelper = new BuildingHelper(apiVersion);
			var buildingsNames = buildingHelper.GetBuildingsNames();

			foreach (var buildingName in buildingsNames)
			{
				Assert.NotNull(buildingHelper.GetBuildingImageByName(buildingName));
			}
		}

		[Theory]
		[MemberData(nameof(ApiVersions_TestData))]
		public void When_GetAllItems_Then_ComponentsBelongToSameApiVersion(ApiVersion apiVersion)
		{
			var suffix = apiVersion == ApiVersion.v1 ? string.Empty : "_v2";
			var allowed = new[]
			{
				$"FamilyIslandHelper.Api.Models.Items{suffix}",
				$"FamilyIslandHelper.Api.Models.Resources{suffix}"
			};
			var buildingHelper = new BuildingHelper(apiVersion);
			var wrong = new List<string>();

			foreach (var buildingName in buildingHelper.GetBuildingsNames())
			{
				foreach (var item in buildingHelper.CreateBuilding(buildingName).Items)
				{
					foreach (var (component, _) in item.Components)
					{
						if (!allowed.Contains(component.GetType().Namespace))
						{
							wrong.Add($"{buildingName}/{item.GetType().Name} -> {component.GetType().FullName}");
						}
					}
				}
			}

			Assert.True(wrong.Count == 0, string.Join(Environment.NewLine, wrong));
		}

		[Theory]
		[MemberData(nameof(ApiVersions_TestData))]
		public void When_CheckEmbeddedPictures_Then_NoOrphanItemPictures(ApiVersion apiVersion)
		{
			var prefix = apiVersion == ApiVersion.v1 ? "" : "_v2";
			var picturesRoot = $"FamilyIslandHelper.Api.Pictures{prefix}.Items.";
			var buildingHelper = new BuildingHelper(apiVersion);

			var expected = buildingHelper.GetBuildingsNames()
				.SelectMany(b => buildingHelper.GetItemsOfBuilding(b).Select(i => $"{picturesRoot}{b}.{i}.png"))
				.ToHashSet();

			var actual = typeof(ItemHelper).Assembly.GetManifestResourceNames()
				.Where(n => n.StartsWith(picturesRoot))
				.ToHashSet();

			Assert.Empty(actual.Except(expected));
		}

#if WINDOWS
		[Theory]
		[InlineData(ApiVersion.v1, "Knocker")]
		[InlineData(ApiVersion.v2, "Knocker")]
		public void When_GetBuildingImageByName_Then_ReturnCorrectValue(ApiVersion apiVersion, string buildingName)
		{
			buildingHelper = new BuildingHelper(apiVersion);

			var actualImage = buildingHelper.GetBuildingImageByName(buildingName);

			Assert.Equal(System.Drawing.Imaging.ImageFormat.Png, actualImage.RawFormat);
		}
#endif
	}
}
