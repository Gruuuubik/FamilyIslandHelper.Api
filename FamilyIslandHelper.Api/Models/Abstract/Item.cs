using System.Diagnostics.CodeAnalysis;

namespace FamilyIslandHelper.Api.Models.Abstract
{
	/// <summary>
	/// Base class of everything that can be a component of a recipe: resources and items produced in buildings.
	/// </summary>
	public abstract class Item
	{
		/// <summary>
		/// Gets the identifier of the item. It is used to find the item (see <c>ItemHelper.FindItemByName</c>)
		/// and it is a part of the name of the picture of the item.
		/// </summary>
		[ExcludeFromCodeCoverage]
		public virtual string Id => GetType().Name;

		/// <summary>
		/// Gets the display name of the item (in Russian).
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Gets the text description of one item.
		/// </summary>
		/// <returns>The name of the item and, when the item has an energy cost, the cost.</returns>
		[ExcludeFromCodeCoverage]
		public override string ToString() => Name;

		/// <summary>
		/// Gets the text description of several items.
		/// </summary>
		/// <param name="itemCount">The number of items.</param>
		/// <returns>The name of the item and, when the item has an energy cost, the total cost of <paramref name="itemCount"/> items.</returns>
		[ExcludeFromCodeCoverage]
		public virtual string ToString(int itemCount) => Name;
	}
}
