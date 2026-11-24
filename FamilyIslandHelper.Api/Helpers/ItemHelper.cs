using FamilyIslandHelper.Api.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FamilyIslandHelper.Api.Helpers
{
	/// <summary>
	/// Provides items and resources of the game, their recipes, energy costs and pictures.
	/// </summary>
	public class ItemHelper : BaseHelper
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ItemHelper"/> class.
		/// </summary>
		/// <param name="apiVersion">The version of the API.</param>
		/// <exception cref="NotSupportedException">The version is not supported. Only <see cref="ApiVersion.v2"/> is supported.</exception>
		public ItemHelper(ApiVersion apiVersion) : base(apiVersion)
		{
			FolderWithResourcesPictures = Path.Combine(FolderWithPictures, "Resources");
			FolderWithItemsPictures = Path.Combine(FolderWithPictures, "Items");
		}

		/// <summary>
		/// Gets the folder with pictures of resources, for example <c>Pictures_v2\Resources</c>.
		/// </summary>
		public string FolderWithResourcesPictures { get; }

		/// <summary>
		/// Gets the folder with pictures of items, for example <c>Pictures_v2\Items</c>.
		/// </summary>
		public string FolderWithItemsPictures { get; }

		/// <summary>
		/// Gets an item that is produced in a building by its identifier.
		/// </summary>
		/// <param name="itemTypeString">The identifier of the item, for example <c>Rope</c>.</param>
		/// <returns>The item. The same instance is returned on every call.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="itemTypeString"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">There is no such item (resources are not produced items).</exception>
		public ProducibleItem CreateProducibleItem(string itemTypeString)
		{
			if (itemTypeString == null)
			{
				throw new ArgumentNullException(nameof(itemTypeString));
			}

			return Data.FindItem(itemTypeString)
				?? throw new ArgumentException($"Item '{itemTypeString}' was not found.", nameof(itemTypeString));
		}

		/// <summary>
		/// Gets a resource by its identifier.
		/// </summary>
		/// <param name="itemTypeString">The identifier of the resource, for example <c>Stone</c>.</param>
		/// <returns>The resource. The same instance is returned on every call.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="itemTypeString"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">There is no such resource.</exception>
		public ResourceItem CreateResourceItem(string itemTypeString)
		{
			if (itemTypeString == null)
			{
				throw new ArgumentNullException(nameof(itemTypeString));
			}

			return Data.FindResource(itemTypeString)
				?? throw new ArgumentException($"Resource '{itemTypeString}' was not found.", nameof(itemTypeString));
		}

		/// <summary>
		/// Finds an item or a resource by its identifier.
		/// </summary>
		/// <param name="itemName">The identifier, for example <c>Rope</c> or <c>Stone</c>.</param>
		/// <returns>The <see cref="ProducibleItem"/> or the <see cref="ResourceItem"/>, or <see langword="null"/> if nothing is found.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="itemName"/> is <see langword="null"/>.</exception>
		public Item FindItemByName(string itemName)
		{
			if (itemName == null)
			{
				throw new ArgumentNullException(nameof(itemName));
			}

			return (Item)Data.FindItem(itemName) ?? Data.FindResource(itemName);
		}

		/// <summary>
		/// Compares the energy costs of two items.
		/// </summary>
		/// <param name="item1">The first item.</param>
		/// <param name="item2">The second item.</param>
		/// <returns>A sentence (in Russian) that tells which item is cheaper or that they cost the same.</returns>
		public static string CompareItems(Item item1, Item item2)
		{
			return CompareItems(item1, 1, item2, 1);
		}

		/// <summary>
		/// Compares the energy costs of two sets of items.
		/// </summary>
		/// <param name="item1">The first item.</param>
		/// <param name="count1">The number of the first items.</param>
		/// <param name="item2">The second item.</param>
		/// <param name="count2">The number of the second items.</param>
		/// <returns>A sentence (in Russian) that tells which set is cheaper or that they cost the same.</returns>
		public static string CompareItems(Item item1, int count1, Item item2, int count2)
		{
			var cost1 = 0;
			var cost2 = 0;

			if (item1 is ResourceItem resourceItem1)
			{
				cost1 = resourceItem1.EnergyCost;
			}
			else if (item1 is ProducibleItem producibleItem1)
			{
				cost1 = producibleItem1.ProduceEnergyCost;
			}

			if (item2 is ResourceItem resourceItem2)
			{
				cost2 = resourceItem2.EnergyCost;
			}
			else if (item2 is ProducibleItem producibleItem2)
			{
				cost2 = producibleItem2.ProduceEnergyCost;
			}

			cost1 *= count1;
			cost2 *= count2;

			string result;

			if (cost1 < cost2)
			{
				result = $"{count1} '{item1.Name}' выгоднее, чем {count2} '{item2.Name}' по энергии.";
			}
			else if (cost1 > cost2)
			{
				result = $"{count2} '{item2.Name}' выгоднее, чем {count1} '{item1.Name}' по энергии.";
			}
			else
			{
				result = $"{count1} '{item1.Name}' и {count2} '{item2.Name}' равны по энергии.";
			}

			return result;
		}

		/// <summary>
		/// Gets the text description of several items.
		/// </summary>
		/// <param name="itemName">The identifier of the item or the resource.</param>
		/// <param name="itemCount">The number of items.</param>
		/// <param name="showListOfComponents">Whether to add the tree of components (for items that are produced in buildings).</param>
		/// <returns>The lines: the name with the total energy cost and, optionally, the components.</returns>
		public List<string> GetInfoAboutItem(string itemName, int itemCount, bool showListOfComponents)
		{
			var info = new List<string>();

			var item = FindItemByName(itemName);

			info.Add(item.ToString(itemCount));

			if (item is ProducibleItem producibleItem)
			{
				if (showListOfComponents)
				{
					info.Add(string.Empty);
					info.Add("Components:");

					info.AddRange(producibleItem.ComponentsInfo(0, itemCount));
				}
			}

			return info;
		}

		/// <summary>
		/// Gets the relative path of the picture of an item.
		/// </summary>
		/// <param name="buildingName">The identifier of the building that produces the item.</param>
		/// <param name="itemName">The identifier of the item.</param>
		/// <returns>The path, for example <c>Pictures_v2\Items\Bench\Barrel.png</c>.</returns>
		public string GetItemImagePathByName(string buildingName, string itemName)
		{
			return Path.Combine(FolderWithItemsPictures, buildingName, itemName + ImageExtension);
		}

		/// <summary>
		/// Gets the picture of an item.
		/// </summary>
		/// <remarks>
		/// Uses System.Drawing.Common, so on .NET 6 and later it works on Windows only. Use <see cref="GetItemImageStreamByName(string, string)"/> to get the raw PNG.
		/// </remarks>
		/// <param name="buildingName">The identifier of the building that produces the item.</param>
		/// <param name="itemName">The identifier of the item.</param>
		/// <returns>The picture.</returns>
		public Image GetItemImageByName(string buildingName, string itemName)
		{
			return Image.FromStream(GetItemImageStreamByName(buildingName, itemName));
		}

		/// <summary>
		/// Gets the picture of an item as a PNG stream.
		/// </summary>
		/// <param name="buildingName">The identifier of the building that produces the item.</param>
		/// <param name="itemName">The identifier of the item.</param>
		/// <returns>A stream with the picture, positioned at the beginning.</returns>
		public MemoryStream GetItemImageStreamByName(string buildingName, string itemName)
		{
			var imagePath = $"{MainNamespace}.{GetItemImagePathByName(buildingName, itemName)}";

			return GetImageStreamByImagePath(imagePath);
		}

		/// <summary>
		/// Gets the identifiers of all resources.
		/// </summary>
		/// <returns>The identifiers in the order they are declared in the game data.</returns>
		public List<string> GetResourcesNames()
		{
			return Data.Resources.Select(r => r.Id).ToList();
		}

		/// <summary>
		/// Gets the picture of a resource.
		/// </summary>
		/// <remarks>
		/// Uses System.Drawing.Common, so on .NET 6 and later it works on Windows only. Use <see cref="GetResourceImageStreamByName(string)"/> to get the raw PNG.
		/// </remarks>
		/// <param name="resourceName">The identifier of the resource.</param>
		/// <returns>The picture.</returns>
		public Image GetResourceImageByName(string resourceName)
		{
			return Image.FromStream(GetResourceImageStreamByName(resourceName));
		}

		/// <summary>
		/// Gets the picture of a resource as a PNG stream.
		/// </summary>
		/// <param name="resourceName">The identifier of the resource.</param>
		/// <returns>A stream with the picture, positioned at the beginning.</returns>
		public MemoryStream GetResourceImageStreamByName(string resourceName)
		{
			var imagePath = $"{MainNamespace}.{FolderWithResourcesPictures}.{resourceName + ImageExtension}";

			return GetImageStreamByImagePath(imagePath);
		}
	}
}
