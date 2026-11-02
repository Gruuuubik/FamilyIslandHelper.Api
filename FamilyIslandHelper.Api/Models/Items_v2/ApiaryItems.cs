using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class Pollen : ProducibleItem
	{
		public override string Name => "Пыльца";
		public override int LevelWhenAppears => 60;
		public override Building BuildingToCreate => new Apiary();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stick(), 18),
				(new Wood(), 10),
				(new Grass(), 25)
			};
	}

	public class BeesWax : ProducibleItem
	{
		public override string Name => "Пчелиный воск";
		public override int LevelWhenAppears => 65;
		public override Building BuildingToCreate => new Apiary();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Clay(), 22),
				(new Stick(), 25),
				(new Wood(), 20)
			};
	}

	public class Honey : ProducibleItem
	{
		public override string Name => "Мёд";
		public override int LevelWhenAppears => 70;
		public override Building BuildingToCreate => new Apiary();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Wood(), 20),
				(new Clay(), 15),
				(new Grass(), 25)
			};
	}
}
