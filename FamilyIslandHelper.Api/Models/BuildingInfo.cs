namespace FamilyIslandHelper.Api.Models
{
	/// <summary>
	/// Short description of a building: its identifier and display name.
	/// </summary>
	public class BuildingInfo
	{
		/// <summary>
		/// Gets or sets the display name of the building (in Russian).
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the building. Pass it to
		/// <see cref="FamilyIslandHelper.Api.Helpers.BuildingHelper.CreateBuilding(string)"/> to get the building.
		/// </summary>
		public string Value { get; set; }
	}
}
