using System;
using System.IO;
using System.Reflection;

namespace FamilyIslandHelper.Api.Helpers
{
	/// <summary>
	/// Base class of the helpers. A helper works with the game data and pictures of one <see cref="ApiVersion"/>.
	/// </summary>
	public abstract class BaseHelper
	{
		/// <summary>
		/// The root namespace of the assembly. It is the prefix of the names of classes and embedded resources.
		/// </summary>
		protected const string MainNamespace = "FamilyIslandHelper.Api";

		/// <summary>
		/// The extension of the picture files.
		/// </summary>
		public const string ImageExtension = ".png";

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseHelper"/> class.
		/// </summary>
		/// <param name="apiVersion">The version of the API.</param>
		/// <exception cref="NotSupportedException">The version is not supported. Only <see cref="ApiVersion.v2"/> is supported.</exception>
		protected BaseHelper(ApiVersion apiVersion)
		{
			Assembly = Assembly.GetExecutingAssembly();
			Prefix = "_v2";

			FolderWithPictures = $"Pictures{Prefix}";
		}

		/// <summary>
		/// Gets the suffix of the API version that is used in the names of namespaces and picture folders, for example <c>_v2</c>.
		/// </summary>
		protected string Prefix { get; }

		/// <summary>
		/// Gets the folder with pictures of the API version, for example <c>Pictures_v2</c>.
		/// </summary>
		public string FolderWithPictures { get; }

		/// <summary>
		/// Gets the assembly that contains the embedded pictures.
		/// </summary>
		protected Assembly Assembly { get; }

		/// <summary>
		/// Reads an embedded picture.
		/// </summary>
		/// <param name="imagePath">The name of the embedded resource. Backslashes are replaced with dots.</param>
		/// <returns>A stream with the picture, positioned at the beginning.</returns>
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
