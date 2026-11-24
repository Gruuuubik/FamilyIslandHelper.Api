using FamilyIslandHelper.Api.Helpers;
using Xunit;

namespace FamilyIslandHelper.Api.UnitTests
{
	public class BaseHelperTests : BaseTest
	{
		private sealed class TestHelper : BaseHelper
		{
			public TestHelper() : base(ApiVersion.v2)
			{
			}

			public string RootNamespace => MainNamespace;

			public MemoryStream ReadPicture(string imagePath) => GetImageStreamByImagePath(imagePath);
		}

		// Paths are built with Path.Combine, so they contain "/" on Linux and macOS and "\" on Windows.
		[Theory]
		[InlineData('\\')]
		[InlineData('/')]
		public void When_GetImageStreamByImagePath_Then_PathSeparatorDoesNotMatter(char separator)
		{
			var helper = new TestHelper();
			var imagePath = $"{helper.RootNamespace}.Pictures_v2{separator}Buildings{separator}Bench.png";

			using (var stream = helper.ReadPicture(imagePath))
			{
				Assert.True(stream.Length > 0);
			}
		}

		[Fact]
		public void When_GetImageStreamByImagePathOfNotExistedPicture_Then_ThrowsFileNotFoundException()
		{
			var helper = new TestHelper();

			var exception = Assert.Throws<FileNotFoundException>(() => helper.ReadPicture($"{helper.RootNamespace}.Pictures_v2/Buildings/NotExisted.png"));

			Assert.Contains("NotExisted.png", exception.Message);
		}
	}
}
