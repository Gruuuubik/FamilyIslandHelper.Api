using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Data
{
	/// <summary>
	/// A resource as it is described in <c>resources.json</c>.
	/// </summary>
	internal sealed class ResourceDto
	{
		public string Id { get; set; }

		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the energy cost of the resource. 0 means a free resource (it is shown without energy).
		/// Nullable to report a missing value instead of silently treating it as 0.
		/// </summary>
		public int? EnergyCost { get; set; }
	}

	/// <summary>
	/// A building as it is described in <c>Buildings/&lt;Id&gt;.json</c>.
	/// </summary>
	internal sealed class BuildingDto
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public List<ItemDto> Items { get; set; }
	}

	internal sealed class ItemDto
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public List<ComponentDto> Components { get; set; }
	}

	internal sealed class ComponentDto
	{
		/// <summary>
		/// Gets or sets the id of a resource or an item.
		/// </summary>
		public string Item { get; set; }

		public int Count { get; set; }
	}
}
