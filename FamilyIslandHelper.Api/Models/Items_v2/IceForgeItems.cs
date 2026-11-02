using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	internal class IceKnife : ProducibleItem
	{
		public override string Name => "Ледяной нож";
		public override int LevelWhenAppears => 80;
		public override Building BuildingToCreate => new IceForge();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
			{
				(new Stone(), 30),
				(new Iron(), 30),
				(new Wood(), 20)
			};
	}
}
