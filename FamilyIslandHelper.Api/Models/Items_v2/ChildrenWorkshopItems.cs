using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class ColoredPencils : ProducibleItem
	{
		public override string Name => "Цветные карандаши";
		public override int LevelWhenAppears => 50;
		public override Building BuildingToCreate => new ChildrenWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stick(), 30),
				(new Clay(), 30),
				(new Stone(), 30)
			};
	}

	public class Plasticine : ProducibleItem
	{
		public override string Name => "Пластилин";
		public override int LevelWhenAppears => 60;
		public override Building BuildingToCreate => new ChildrenWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Wood(), 20),
				(new Clay(), 25),
				(new Stone(), 18)
			};
	}

	public class BeadKit : ProducibleItem
	{
		public override string Name => "Бисерный набор";
		public override int LevelWhenAppears => 70;
		public override Building BuildingToCreate => new ChildrenWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stone(), 15),
				(new IceKnife(), 2),
				(new Paints(), 2)
			};
	}
}
