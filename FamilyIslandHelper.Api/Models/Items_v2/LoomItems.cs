using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class Lace : ProducibleItem
	{
		public override string Name => "Шнурок";
		public override Building BuildingToCreate => new Loom();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Grass(), 3)
			};
	}

	public class Rope : ProducibleItem
	{
		public override string Name => "Верёвка";
		public override Building BuildingToCreate => new Loom();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Grass(), 14),
				(new Stone(), 8)
			};
	}

	public class LeatherСord : ProducibleItem
	{
		public override string Name => "Кожаный шнур";
		public override Building BuildingToCreate => new Loom();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Knife(), 3),
				(new Leather(), 2),
				(new Clay(), 17)
			};
	}

	public class Basket : ProducibleItem
	{
		public override string Name => "Корзина";
		public override Building BuildingToCreate => new Loom();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Needle(), 3),
				(new Grass(), 12),
				(new Stone(), 15)
			};
	}
}