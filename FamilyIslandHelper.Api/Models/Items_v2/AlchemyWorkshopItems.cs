using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class HomemadeSnowflake : ProducibleItem
	{
		public override string Name => "Самодельная снежинка";
		public override Building BuildingToCreate => new AlchemyWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
		{
			(new Iron(), 18),
			(new Pot(), 3),
			(new Stick(), 25)
		};
	}

	public class SnowDust : ProducibleItem
	{
		public override string Name => "Снежная пыль";
		public override Building BuildingToCreate => new AlchemyWorkshop();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
		{
			(new Stone(), 50),
			(new Wood(), 40),
			(new Iron(), 35)
		};
	}
}
