using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FamilyIslandHelper.Api.Helpers
{
	/// <summary>
	/// Finds the classes of the game data (buildings, items and resources) by their namespace.
	/// </summary>
	public class ClassHelper
	{
		internal static IEnumerable<Type> GetClasses(string nameSpace)
		{
			var assembly = Assembly.Load("FamilyIslandHelper.Api");

			return assembly.GetTypes().Where(type => type.Namespace == nameSpace);
		}

		internal static IEnumerable<string> GetClassesNames(string nameSpace)
		{
			return GetClasses(nameSpace).Select(type => type.Name);
		}
	}
}
