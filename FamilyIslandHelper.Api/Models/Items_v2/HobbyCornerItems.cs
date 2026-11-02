using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	internal class Scissors : ProducibleItem
	{
		public override string Name => "Ножницы";
		public override int LevelWhenAppears => 40;
		public override Building BuildingToCreate => new HobbyCorner();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Clay(), 35),
				(new Stone(), 25),
				(new Iron(), 30)
			};
	}

	public class CorrugatedPaper : ProducibleItem
	{
		public override string Name => "Гофрированная бумага";
		public override int LevelWhenAppears => 67;
		public override Building BuildingToCreate => new HobbyCorner();
		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Wood(), 18),
				(new Clay(), 10),
				(new Stone(), 12)
			};
	}

	public class Paints : ProducibleItem
	{
		public override string Name => "Палитра";
		public override int LevelWhenAppears => 71;
		public override Building BuildingToCreate => new HobbyCorner();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stick(), 22),
				(new Wood(), 15),
				(new Grass(), 20)
			};
	}
}
