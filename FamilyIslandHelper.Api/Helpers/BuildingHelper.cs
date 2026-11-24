using FamilyIslandHelper.Api.Models;
using FamilyIslandHelper.Api.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FamilyIslandHelper.Api.Helpers
{
	/// <summary>
	/// Provides buildings of the game and their pictures.
	/// </summary>
	public class BuildingHelper : BaseHelper
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BuildingHelper"/> class.
		/// </summary>
		/// <param name="apiVersion">The version of the API.</param>
		/// <exception cref="NotSupportedException">The version is not supported. Only <see cref="ApiVersion.v2"/> is supported.</exception>
		public BuildingHelper(ApiVersion apiVersion) : base(apiVersion)
		{
			FolderWithBuildingsPictures = Path.Combine(FolderWithPictures, "Buildings");
		}

		/// <summary>
		/// Gets the folder with pictures of buildings, for example <c>Pictures_v2\Buildings</c>.
		/// </summary>
		public string FolderWithBuildingsPictures { get; }

		/// <summary>
		/// Gets the identifiers and display names of all buildings.
		/// </summary>
		/// <returns>The buildings ordered by identifier: <see cref="BuildingInfo.Value"/> is the identifier, <see cref="BuildingInfo.Name"/> is the display name.</returns>
		public List<BuildingInfo> GetBuildingsClasses()
		{
			return Data.Buildings.Select(b => new BuildingInfo
			{
				Value = b.Id,
				Name = b.Name
			}).ToList();
		}

		/// <summary>
		/// Gets a building by its identifier.
		/// </summary>
		/// <param name="buildingName">The identifier of the building, for example <c>Forge</c>.</param>
		/// <returns>The building. The same instance is returned on every call.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="buildingName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">There is no building with such identifier.</exception>
		public Building CreateBuilding(string buildingName)
		{
			if (buildingName == null)
			{
				throw new ArgumentNullException(nameof(buildingName));
			}

			return Data.FindBuilding(buildingName)
				?? throw new ArgumentException($"Building '{buildingName}' was not found.", nameof(buildingName));
		}

		/// <summary>
		/// Gets the identifiers of all buildings.
		/// </summary>
		/// <returns>The identifiers ordered alphabetically.</returns>
		public List<string> GetBuildingsNames()
		{
			return Data.Buildings.Select(b => b.Id).ToList();
		}

		/// <summary>
		/// Gets the identifiers of the items a building produces.
		/// </summary>
		/// <param name="buildingName">The identifier of the building.</param>
		/// <returns>The identifiers of the items in the order they are declared in the game data.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="buildingName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">There is no building with such identifier.</exception>
		public List<string> GetItemsOfBuilding(string buildingName)
		{
			var building = CreateBuilding(buildingName);

			return building.Items.Select(i => i.Id).ToList();
		}

		/// <summary>
		/// Gets the picture of a building.
		/// </summary>
		/// <remarks>
		/// Uses System.Drawing.Common, so on .NET 6 and later it works on Windows only. Use <see cref="GetBuildingImageStreamByName(string)"/> to get the raw PNG.
		/// </remarks>
		/// <param name="buildingName">The identifier of the building.</param>
		/// <returns>The picture.</returns>
		/// <exception cref="FileNotFoundException">There is no such picture in the assembly.</exception>
		public Image GetBuildingImageByName(string buildingName)
		{
			return Image.FromStream(GetBuildingImageStreamByName(buildingName));
		}

		/// <summary>
		/// Gets the picture of a building as a PNG stream.
		/// </summary>
		/// <param name="buildingName">The identifier of the building.</param>
		/// <returns>A stream with the picture, positioned at the beginning.</returns>
		/// <exception cref="FileNotFoundException">There is no such picture in the assembly.</exception>
		public MemoryStream GetBuildingImageStreamByName(string buildingName)
		{
			var imagePath = $"{MainNamespace}.{FolderWithBuildingsPictures}.{buildingName + ImageExtension}";

			return GetImageStreamByImagePath(imagePath);
		}
	}
}
