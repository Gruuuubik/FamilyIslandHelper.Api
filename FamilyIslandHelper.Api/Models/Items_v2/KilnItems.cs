using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class BurntBrick : ProducibleItem
	{
		public override string Name => "Обожженный кирпич";
		public override int LevelWhenAppears => 35;
		public override Building BuildingToCreate => new Kiln();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Brick(), 3)
			};
	}

	public class Shingles : ProducibleItem
	{
		public override string Name => "Черепица";
		public override int LevelWhenAppears => 46;
		public override Building BuildingToCreate => new Kiln();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new SmoothBoard(), 3),
				(new Clay(), 17),
				(new Wood(), 10)
			};
	}

	public class Cup : ProducibleItem
	{
		public override string Name => "Чашка";
		public override int LevelWhenAppears => 60;
		public override Building BuildingToCreate => new Kiln();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Clay(), 15),
				(new Amphora(), 2),
				(new Wood(), 8)
			};
	}

	public class Plate : ProducibleItem
	{
		public override string Name => "Тарелка";
		public override int LevelWhenAppears => 80;
		public override Building BuildingToCreate => new Kiln();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stick(), 30),
				(new Clay(), 20),
				(new Grass(), 20)
			};
	}
}