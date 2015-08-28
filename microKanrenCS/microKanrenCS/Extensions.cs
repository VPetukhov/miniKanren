using System.Collections;
using System.Collections.Generic;

namespace microKanrenCS
{
	public static class Extensions
	{
		public static IEnumerable<T> EnumerateFromCurrent<T>(this IEnumerator<T> enumerator)
		{
			do
			{
				yield return enumerator.Current;
			} while (enumerator.MoveNext());
		}

		public static IEnumerable EnumerateFromCurrent(this IEnumerator enumerator)
		{
			do
			{
				yield return enumerator.Current;
			} while (enumerator.MoveNext());
		}
	}
}
