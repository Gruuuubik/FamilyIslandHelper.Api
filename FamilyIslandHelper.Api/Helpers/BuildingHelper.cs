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
		private readonly string buildingsNamespace;

		/// <summary>
		/// Initializes a new instance of the <see cref="BuildingHelper"/> class.
		/// </summary>
		/// <param name="apiVersion">The version of the API.</param>
		/// <exception cref="NotSupportedException">The version is not supported. Only <see cref="ApiVersion.v2"/> is supported.</exception>
		public BuildingHelper(ApiVersion apiVersion) : base(apiVersion)
		{
			buildingsNamespace = $"{MainNamespace}.Models.Buildings{Prefix}";
			FolderWithBuildingsPictures = Path.Combine(FolderWithPictures, "Buildings");
		}

		/// <summary>
		/// Gets the folder with pictures of buildings, for example <c>Pictures_v2\Buildings</c>.
		/// </summary>
		public string FolderWithBuildingsPictures { get; }

		/// <summary>
		/// Gets the class names and display names of all buildings.
		/// </summary>
		/// <returns>The buildings: <see cref="BuildingInfo.Value"/> is the name of the class, <see cref="BuildingInfo.Name"/> is the display name.</returns>
		public List<BuildingInfo> GetBuildingsClasses()
		{
			var buildingsClasses = ClassHelper.GetClasses(buildingsNamespace);

			return buildingsClasses.Select(bc => new BuildingInfo
			{
				Value = bc.Name,
				Name = CreateBuilding(bc.Name).Name
			}).ToList();
		}

		/// <summary>
		/// Creates a building by the name of its class.
		/// </summary>
		/// <param name="buildingName">The name of the class of the building, for example <c>Forge</c>.</param>
		/// <returns>A new instance of the building.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="buildingName"/> is <see langword="null"/>.</exception>
		/// <exception cref="TypeLoadException">There is no building with such name.</exception>
		public Building CreateBuilding(string buildingName)
		{
			if (buildingName == null)
			{
				throw new ArgumentNullException(nameof(buildingName));
			}

			var buildingType = Type.GetType($"{buildingsNamespace}.{buildingName}", true);
			return Activator.CreateInstance(buildingType) as Building;
		}

		/// <summary>
		/// Gets the names of the classes of all buildings.
		/// </summary>
		/// <returns>The names of the classes.</returns>
		public List<string> GetBuildingsNames()
		{
			return ClassHelper.GetClassesNames(buildingsNamespace).ToList();
		}

		/// <summary>
		/// Gets the names of the classes of the items a building produces.
		/// </summary>
		/// <param name="buildingName">The name of the class of the building.</param>
		/// <returns>The names of the classes of the items in the order they are declared in the building.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="buildingName"/> is <see langword="null"/>.</exception>
		/// <exception cref="TypeLoadException">There is no building with such name.</exception>
		public List<string> GetItemsOfBuilding(string buildingName)
		{
			var building = CreateBuilding(buildingName);

			return building.Items.Select(i => i.GetType().Name).ToList();
		}

		/// <summary>
		/// Gets the picture of a building.
		/// </summary>
		/// <remarks>
		/// Uses System.Drawing.Common, so on .NET 6 and later it works on Windows only. Use <see cref="GetBuildingImageStreamByName(string)"/> to get the raw PNG.
		/// </remarks>
		/// <param name="buildingName">The name of the class of the building.</param>
		/// <returns>The picture.</returns>
		public Image GetBuildingImageByName(string buildingName)
		{
			return Image.FromStream(GetBuildingImageStreamByName(buildingName));
		}

		/// <summary>
		/// Gets the picture of a building as a PNG stream.
		/// </summary>
		/// <param name="buildingName">The name of the class of the building.</param>
		/// <returns>A stream with the picture, positioned at the beginning.</returns>
		public MemoryStream GetBuildingImageStreamByName(string buildingName)
		{
			var imagePath = $"{MainNamespace}.{FolderWithBuildingsPictures}.{buildingName + ImageExtension}";

			return GetImageStreamByImagePath(imagePath);
		}
	}
}
