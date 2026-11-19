using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class FoodForMammoth : ProducibleItem
	{
		public override string Name => "Корм для мамонта";
		public override Building BuildingToCreate => new FeedBarn();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stick(), 25),
				(new Wood(), 25),
				(new Grass(), 25)
			};
	}

	public class FrozenFeed : ProducibleItem
	{
		public override string Name => "Морозный корм";
		public override Building BuildingToCreate => new FeedBarn();
		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Grass(), 50),
				(new Wheat(), 4),
				(new Blueberry(), 4)
			};
	}
}
