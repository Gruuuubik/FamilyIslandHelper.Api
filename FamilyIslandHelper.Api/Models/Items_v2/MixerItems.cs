using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Api.Models.Buildings_v2;
using FamilyIslandHelper.Api.Models.Resources_v2;
using System.Collections.Generic;

namespace FamilyIslandHelper.Api.Models.Items_v2
{
	public class Soap : ProducibleItem
	{
		public override string Name => "Мыло";
		public override Building BuildingToCreate => new Mixer();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
		{
			(new Clay(), 7),
			(new Egg(), 2)
		};
	}

	public class Butter : ProducibleItem
	{
		public override string Name => "Масло";
		public override Building BuildingToCreate => new Mixer();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
		{
			(new Milk(), 3)
		};
	}

	public class Cheese : ProducibleItem
	{
		public override string Name => "Сыр";
		public override Building BuildingToCreate => new Mixer();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
		{
			(new Milk(), 6)
		};
	}

	public class WhippedCream : ProducibleItem
	{
		public override string Name => "Взбитые сливки";
		public override Building BuildingToCreate => new Mixer();

		public override List<(Item item, int count)> Components => new List<(Item item, int count)>
		{
			(new Milk(), 6)
		};
	}
}