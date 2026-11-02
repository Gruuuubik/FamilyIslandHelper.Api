using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	internal class GraniteCobblestone : ProducibleItem
	{
		public override string Name => "Гранитный булыжник";
		public override int LevelWhenAppears => 80;
		public override Building BuildingToCreate => new IceMine();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stone(), 35),
				(new Clay(), 40),
				(new Iron(), 40)
			};
	}
}
