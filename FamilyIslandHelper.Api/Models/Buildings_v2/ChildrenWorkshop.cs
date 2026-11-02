using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Items_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Buildings_v2
{
	public class ChildrenWorkshop : Building
	{
		public override string Name => "Детская мастерская";
		public override List<ProducibleItem> Items => new List<ProducibleItem>
		{
			new ColoredPencils(),
			new Plasticine(),
			new BeadKit()
		};
	}
}
