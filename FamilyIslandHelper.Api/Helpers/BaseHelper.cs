using System;
using System.IO;
using System.Reflection;

namespace FamilyIslandHelper.Api.Helpers
{
	public abstract class BaseHelper
	{
		protected const string MainNamespace = "FamilyIslandHelper.Api";
		public const string ImageExtension = ".png";

		protected BaseHelper(ApiVersion apiVersion)
		{
			if (apiVersion == ApiVersion.v1)
			{
				throw new NotSupportedException("API v.1 is not supported. Use API v.2.");
			}

			Assembly = Assembly.GetExecutingAssembly();
			Prefix = "_v2";

			FolderWithPictures = $"Pictures{Prefix}";
		}

		protected string Prefix { get; }

		public string FolderWithPictures { get; }

		protected Assembly Assembly { get; }

		protected MemoryStream GetImageStreamByImagePath(string imagePath)
		{
			using (var stream = Assembly.GetManifestResourceStream(imagePath.Replace("\\", ".")))
			{
				var memoryStream = new MemoryStream();
				stream.CopyTo(memoryStream);
				memoryStream.Position = 0;

				return memoryStream;
			}
		}
	}
}
