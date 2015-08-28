using System.Collections.Generic;

namespace microKanrenCS
{
	public static class Relational
	{
		public delegate ISubstitutions Goal(ISubstitutions substitutions);

		public static ISubstitutions GetEmptySubst()
		{
			return new InfiniteSubstitutions(); //TODO Fabric
		}

		public static Goal Equal(object var1, object var2)
		{
			return subst => subst.Unify(var1, var2);
		}

		private static ISubstitutions Unify(this ISubstitutions subst, object var1, object var2)
		{
			var logVar1 = var1 as LogicVar;
			var logVar2 = var2 as LogicVar;

			var val1 = logVar1 == null ? var1 : subst.GetValue(logVar1);
			var val2 = logVar2 == null ? var2 : subst.GetValue(logVar2);

			var logVal1 = val1 as LogicVar;
			var logVal2 = val2 as LogicVar;
			if (logVal1 != null && logVal2 != null && logVal1 == logVal2)
				return subst;

			if (logVal1 != null)
				return subst.Extend(logVal1, val2);

			if (logVal2 != null)
				return subst.Extend(logVal2, val1);

			var list1 = val1 as IEnumerable<object>;
			var list2 = val2 as IEnumerable<object>;

			if (list1 != null && list2 != null)
			{
				var iter1 = list1.GetEnumerator();
				var iter2 = list2.GetEnumerator();
				var s1 = subst.Unify(iter1.Current, iter2.Current);
				ISubstitutions s2 = null;

				if (s1 != null && iter1.MoveNext() && iter2.MoveNext())
				{
					s2 = s1.Unify(GetIEnumerable(iter1), GetIEnumerable(iter2));

				}
				return s2;
			}

			return val1 == val2 ? subst : null;
		}

		public static Goal Disjunction(Goal goal1, Goal goal2)
		{
			return subst => new InfiniteSubstitutions(DisjunctGoals(subst, goal1, goal2));	//TODO Fabric
		}

		private static IEnumerable<Substitution> DisjunctGoals(ISubstitutions subst, Goal goal1, Goal goal2)
		{
			var iter1 = goal1(subst).GetEnumerator();
			var iter2 = goal2(subst).GetEnumerator();

			bool firstNotEmpty = true, secondNotEmpty = true;

			while (firstNotEmpty || secondNotEmpty)
			{
				if (firstNotEmpty)	//TODO apply (refactor unify?)
				{
					yield return iter1.Current;
					firstNotEmpty = iter1.MoveNext();
				}

				if (secondNotEmpty)
				{
					yield return iter2.Current;
					secondNotEmpty = iter2.MoveNext();
				}
			}
		}

		private static IEnumerable<object> GetIEnumerable(IEnumerator<object> enumerator)
		{
			do
			{
				yield return enumerator.Current;
			} while (enumerator.MoveNext());
		}
	}
}
