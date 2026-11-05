using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	internal class Book : ProducibleItem
	{
		public override string Name => "Книга";
		public override Building BuildingToCreate => new PrintShop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Grass(), 30),
				(new Stone(), 30),
				(new Wood(), 30)
			};
	}

	internal class BottleOfInk : ProducibleItem
	{
		public override string Name => "Бутылка с чернилами";
		public override Building BuildingToCreate => new PrintShop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Feather(), 1),
				(new Stick(), 45),
				(new Pot(), 4)
			};
	}
}
