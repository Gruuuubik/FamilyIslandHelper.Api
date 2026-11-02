using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class GemNecklace : ProducibleItem
	{
		public override string Name => "Ожерелье";
		public override int LevelWhenAppears => 30;
		public override Building BuildingToCreate => new ShamanWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Grass(), 37),
				(new Emerald(), 2),
				(new Clay(), 32)
			};
	}

	public class Rune : ProducibleItem
	{
		public override string Name => "Руна";
		public override int LevelWhenAppears => 35;
		public override Building BuildingToCreate => new ShamanWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stick(), 40),
				(new Stone(), 40),
				(new Iron(), 35)
			};
	}

	public class Amulet : ProducibleItem
	{
		public override string Name => "Амулет";
		public override int LevelWhenAppears => 52;
		public override Building BuildingToCreate => new ShamanWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Grass(), 25),
				(new Stone(), 30),
				(new Iron(), 30)
			};
	}

	public class DreamCatcher : ProducibleItem
	{
		public override string Name => "Ловец снов";
		public override int LevelWhenAppears => 76;
		public override Building BuildingToCreate => new ShamanWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stick(), 25),
				(new Iron(), 20),
				(new Clay(), 25)
			};
	}
}