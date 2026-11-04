using System;

namespace FamilyIslandHelper.Api
{
	/// <summary>
	/// Versions of API.
	/// </summary>
	public enum ApiVersion
	{
		/// <summary>
		/// Represents an API before update 10.2024.
		/// </summary>
		[Obsolete("API v.1 is not supported. Use API v.2.")]
		v1,

		/// <summary>
		/// Represents an API after update 10.2024.
		/// </summary>
		v2
	}
}
