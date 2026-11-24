using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FamilyIslandHelper.Api.Models.Abstract
{
	/// <summary>
	/// A building of the game that produces items.
	/// </summary>
	public abstract class Building
	{
		/// <summary>
		/// Gets the identifier of the building. It is used to find the building (see <c>BuildingHelper.CreateBuilding</c>)
		/// and it is the name of the picture of the building.
		/// </summary>
		[ExcludeFromCodeCoverage]
		public virtual string Id => GetType().Name;

		/// <summary>
		/// Gets the display name of the building (in Russian).
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Gets the items the building produces, in the order they are declared.
		/// </summary>
		public abstract List<ProducibleItem> Items { get; }

		/// <inheritdoc />
		[ExcludeFromCodeCoverage]
		public override string ToString() => Name;
	}
}
