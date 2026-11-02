using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	internal class IceBowl : ProducibleItem
	{
		public override string Name => "Ледяная миска";
		public override int LevelWhenAppears => 100;
		public override Building BuildingToCreate => new Building1();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stick(), 25),
				(new Iron(), 25)
			};
	}
}
