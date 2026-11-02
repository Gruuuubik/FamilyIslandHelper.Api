using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	internal class SnowShovel : ProducibleItem
	{
		public override string Name => "SnowShovel";
		public override int LevelWhenAppears => 80;
		public override Building BuildingToCreate => new WinterWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Iron(), 18),
				(new Wood(), 18),
				(new Stick(), 20)
			};
	}

	internal class Skis : ProducibleItem
	{
		public override string Name => "Skis";
		public override int LevelWhenAppears => 90;
		public override Building BuildingToCreate => new WinterWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stick(), 30),
				(new Wood(), 20),
				(new Iron(), 25)
			};
	}
}
