using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class Apron : ProducibleItem
	{
		public override string Name => "Фартук";
		public override int LevelWhenAppears => 85;
		public override Building BuildingToCreate => new Sewing();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
		{
			(new Grass(), 35),
			(new Wood(), 40),
			(new Clay(), 50)
		};
	}
}
