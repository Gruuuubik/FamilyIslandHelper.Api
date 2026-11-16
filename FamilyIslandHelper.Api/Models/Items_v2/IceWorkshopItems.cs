using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	internal class Quickdraw : ProducibleItem
	{
		public override string Name => "Оттяжка";
		public override Building BuildingToCreate => new IceWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Grass(), 12),
				(new Stone(), 20),
				(new Iron(), 20)
			};
	}
}
