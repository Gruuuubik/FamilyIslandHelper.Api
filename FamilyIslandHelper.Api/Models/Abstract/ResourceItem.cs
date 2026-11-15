using System.Diagnostics.CodeAnalysis;

namespace FamilyIslandHelper.Api.Models.Abstract
{
	/// <summary>
	/// A resource (raw material) with a fixed energy cost.
	/// </summary>
	public abstract class ResourceItem : Item
	{
		/// <summary>
		/// Gets the energy cost of one unit of the resource.
		/// </summary>
		public abstract int EnergyCost { get; }

		/// <inheritdoc />
		[ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return ToString(1);
		}

		/// <inheritdoc />
		public override string ToString(int itemCount)
		{
			return $"{Name}({EnergyCost * itemCount} энергии)";
		}
	}

	/// <summary>
	/// A resource that costs no energy. It is shown by its name only.
	/// </summary>
	public abstract class ZeroCostResourceItem : ResourceItem
	{
		/// <inheritdoc />
		public override int EnergyCost => 0;

		/// <inheritdoc />
		[ExcludeFromCodeCoverage]
		public override string ToString() => Name;

		/// <inheritdoc />
		public override string ToString(int itemCount)
		{
			return Name;
		}
	}
}
