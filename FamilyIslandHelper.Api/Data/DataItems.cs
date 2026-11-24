using FamilyIslandHelper.Api.Models.Abstract;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Data
{
	internal sealed class DataResourceItem : ResourceItem
	{
		private readonly string id;
		private readonly string name;
		private readonly int energyCost;

		public DataResourceItem(string id, string name, int energyCost)
		{
			this.id = id;
			this.name = name;
			this.energyCost = energyCost;
		}

		public override string Id => id;

		public override string Name => name;

		public override int EnergyCost => energyCost;

		public override string ToString(int itemCount)
		{
			return EnergyCost == 0 ? Name : base.ToString(itemCount);
		}
	}

	internal sealed class DataProducibleItem : ProducibleItem
	{
		private readonly string id;
		private readonly string name;
		private Building building;
		private List<(Item item, int count)> components = new List<(Item item, int count)>();

		public DataProducibleItem(string id, string name)
		{
			this.id = id;
			this.name = name;
		}

		public override string Id => id;

		public override string Name => name;

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>(components);

		public override Building BuildingToCreate => building;

		internal IReadOnlyList<(Item item, int count)> RawComponents => components;

		internal void Initialize(Building building, List<(Item item, int count)> components)
		{
			this.building = building;
			this.components = components;
		}
	}

	internal sealed class DataBuilding : Building
	{
		private readonly string id;
		private readonly string name;
		private List<ProducibleItem> items = new List<ProducibleItem>();

		public DataBuilding(string id, string name)
		{
			this.id = id;
			this.name = name;
		}

		public override string Id => id;

		public override string Name => name;

		public override List<ProducibleItem> Items => new List<ProducibleItem>(items);

		internal void SetItems(List<ProducibleItem> items)
		{
			this.items = items;
		}
	}
}
