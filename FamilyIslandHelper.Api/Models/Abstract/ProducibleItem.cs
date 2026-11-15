using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FamilyIslandHelper.Api.UnitTests")]

namespace FamilyIslandHelper.Api.Models.Abstract
{
	/// <summary>
	/// An item that is produced in a building from components (resources and other produced items).
	/// </summary>
	public abstract class ProducibleItem : Item
	{
		/// <summary>
		/// Gets the recipe: the components and how many of each one are needed to produce one item.
		/// </summary>
		public abstract List<(Item item, int count)> Components { get; }

		/// <summary>
		/// Gets the building that produces the item.
		/// </summary>
		public abstract Building BuildingToCreate { get; }

		/// <summary>
		/// Gets the total energy cost of one item: the sum of the energy costs of all components,
		/// including the components of the components.
		/// </summary>
		public int ProduceEnergyCost
		{
			get
			{
				var totalEnergyCost = 0;

				foreach (var (item, count) in Components)
				{
					if (item is ProducibleItem producibleItem)
					{
						totalEnergyCost += producibleItem.ProduceEnergyCost * count;
					}
					else if (item is ResourceItem resourceItem)
					{
						totalEnergyCost += resourceItem.EnergyCost * count;
					}
				}

				return totalEnergyCost;
			}
		}

		/// <summary>
		/// Gets the text tree of components needed to produce several items.
		/// </summary>
		/// <param name="tabsCount">The nesting level of the first lines. Every line is indented by <c>tabsCount + 1</c> tab characters.</param>
		/// <param name="itemCount">The number of items to produce.</param>
		/// <returns>The lines of the tree: a line for every component followed by the lines of its own components.</returns>
		public List<string> ComponentsInfo(int tabsCount, int itemCount)
		{
			var componentsInfo = new List<string>();
			var tabs = new string('\t', tabsCount + 1);

			foreach (var (item, count) in Components)
			{
				componentsInfo.Add($"{tabs}{item.ToString(itemCount)} - {count * itemCount} шт.");

				if (item is ProducibleItem producibleItem)
				{
					componentsInfo.AddRange(producibleItem.ComponentsInfo(tabsCount + 1, itemCount * count));
				}
			}

			return componentsInfo;
		}

		/// <inheritdoc />
		public override string ToString(int itemCount)
		{
			return $"{Name}({ProduceEnergyCost * itemCount} энергии)";
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{Name}({ProduceEnergyCost} энергии)";
		}
	}
}
