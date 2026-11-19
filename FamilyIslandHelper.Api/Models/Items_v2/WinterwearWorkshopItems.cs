using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	internal class Mittens : ProducibleItem
	{
		public override string Name => "Варежки";
		public override Building BuildingToCreate => new WinterwearWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new MammothWool(), 3),
				(new Stone(), 25),
				(new Grass(), 25)
			};
	}

	internal class KnittedHat : ProducibleItem
	{
		public override string Name => "Вязанная шапка";
		public override Building BuildingToCreate => new WinterwearWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new MammothWool(), 1),
				(new Needle(), 2),
				(new Stick(), 20)
			};
	}
}
