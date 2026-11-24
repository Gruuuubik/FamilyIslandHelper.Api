using FamilyIslandHelper.Api.Helpers;
using FluentAssertions;
using Xunit;

namespace FamilyIslandHelper.Api.UnitTests
{
	public class BuildingHelperTests : BaseTest
	{
		private BuildingHelper buildingHelper;

		[Theory]
		[InlineData(ApiVersion.v2, 34)]
		public void When_GetBuildingsClasses_Then_ReturnCorrectCollection(ApiVersion apiVersion, int expectedCount)
		{
			buildingHelper = new BuildingHelper(apiVersion);

			var actualBuildingsClasses = buildingHelper.GetBuildingsClasses();

			Assert.Equal(expectedCount, actualBuildingsClasses.Count);
			Assert.All(actualBuildingsClasses, (bc) => Assert.NotNull(bc));
		}

		[Theory]
		[InlineData(ApiVersion.v2, "Forge", new[] { "Needle", "Hammer", "Glass", "Lock" })]
		public void When_TryToGetItemsOfBuilding_Then_ReturnCorrectItems(ApiVersion apiVersion, string buildingName, IEnumerable<string> expectedItems)
		{
			buildingHelper = new BuildingHelper(apiVersion);

			var actualItems = buildingHelper.GetItemsOfBuilding(buildingName);

			Assert.Equal(expectedItems, actualItems);
		}

		[Fact]
		public void When_GetBuildingsNames_Then_ReturnBuildingsOrderedByName()
		{
			buildingHelper = new BuildingHelper(ApiVersion.v2);

			var actualNames = buildingHelper.GetBuildingsNames();

			Assert.Equal(actualNames.OrderBy(n => n, StringComparer.OrdinalIgnoreCase), actualNames);
		}

		[Fact]
		public void When_CreateNotExistedBuilding_Then_ThrowsException()
		{
			buildingHelper = new BuildingHelper(ApiVersion.v2);

			var exception = Assert.Throws<ArgumentException>(() => buildingHelper.CreateBuilding("Building123"));

			Assert.Equal("buildingName", exception.ParamName);
		}

		[Theory]
		[InlineData("buildingName")]
		public void When_CreateBuildingWithNullParameter_Then_ThrowsException(string expectedParamName)
		{
			buildingHelper = new BuildingHelper(ApiVersion.v2);

			var exception = Assert.Throws<ArgumentNullException>(() => buildingHelper.CreateBuilding(null));

			Assert.Equal(expectedParamName, exception.ParamName);
		}

		[Fact]
		public void When_GetAllBuildingsNames_Then_AllBuildingsHavePictures()
		{
			buildingHelper = new BuildingHelper(ApiVersion.v2);
			var buildingsNames = buildingHelper.GetBuildingsNames();

			foreach (var buildingName in buildingsNames)
			{
				Console.WriteLine($"Checking picture for building: {buildingName}");
				buildingHelper.GetBuildingImageByName(buildingName).Should().NotBeNull();
			}
		}

		[Fact]
		public void When_CheckEmbeddedPictures_Then_NoOrphanItemPictures()
		{
			var prefix = "_v2";
			var picturesRoot = $"FamilyIslandHelper.Api.Pictures{prefix}.Items.";
			var buildingHelper = new BuildingHelper(ApiVersion.v2);

			var expected = buildingHelper.GetBuildingsNames()
				.SelectMany(b => buildingHelper.GetItemsOfBuilding(b).Select(i => $"{picturesRoot}{b}.{i}.png"))
				.ToHashSet();

			var actual = typeof(ItemHelper).Assembly.GetManifestResourceNames()
				.Where(n => n.StartsWith(picturesRoot))
				.ToHashSet();

			actual.Should().BeEquivalentTo(expected);
		}

#if WINDOWS
		[Theory]
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
