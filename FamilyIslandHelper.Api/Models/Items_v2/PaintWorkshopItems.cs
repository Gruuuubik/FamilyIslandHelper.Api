using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class Ocher : ProducibleItem
	{
		public override string Name => "Охра";
		public override int LevelWhenAppears => 40;
		public override Building BuildingToCreate => new PaintWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Clay(), 20),
				(new Stick(), 15),
				(new Grass(), 22)
			};
	}

	internal class BluePaint : ProducibleItem
	{
		public override string Name => "Синяя краска";
		public override int LevelWhenAppears => 55;
		public override Building BuildingToCreate => new PaintWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
		{
			(new Blueberry(), 15),
			(new Pot(), 2),
			(new Stone(), 16)
		};
	}

	internal class YellowPaint : ProducibleItem
	{
		public override string Name => "Желтая краска";
		public override int LevelWhenAppears => 80;
		public override Building BuildingToCreate => new PaintWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
		{
			(new Grass(), 15),
			(new Rope(), 2),
			(new Clay(), 15)
		};
	}
}
