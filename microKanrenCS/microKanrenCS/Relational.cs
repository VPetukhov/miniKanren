using System;
using System.Collections.Generic;
using System.Linq;

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

		public static Goal Conjunction(Goal goal1, Goal goal2)
		{
			return subst =>
			{
				var res = Conjunction(subst, goal1, goal2);
				return res == null ? null : new InfiniteSubstitutions(res);
			};  //TODO Fabric
		}

		public static Goal Disjunction(Goal goal1, Goal goal2)
		{

			throw new NotImplementedException();
		}

		private static IEnumerable<Substitution> Conjunction(ISubstitutions subst, Goal goal1, Goal goal2)
		{
			var iter1 = goal1(subst).GetEnumerator();
			var iter2 = goal2(subst).GetEnumerator();

			var resSubst = GetEmptySubst();

			bool firstNotEmpty = iter1.MoveNext(), secondNotEmpty = iter2.MoveNext();

			while (firstNotEmpty && secondNotEmpty)
			{
				resSubst = resSubst.Unify(iter1.Current);
				if (resSubst == null)
					return null;

				firstNotEmpty = iter1.MoveNext();

				resSubst = resSubst.Unify(iter2.Current);
				if (resSubst == null)
					return null;

				secondNotEmpty = iter2.MoveNext();
			}

			if (firstNotEmpty)
				return ConjunctFinite(resSubst, new InfiniteSubstitutions(iter1.EnumerateFromCurrent())); //TODO Fabric

			if (secondNotEmpty)
				return ConjunctFinite(resSubst, new InfiniteSubstitutions(iter2.EnumerateFromCurrent())); //TODO Fabric

			return resSubst;
		}

		private static IEnumerable<Substitution> ConjunctFinite(ISubstitutions finit, ISubstitutions potentialInfinite)
		{
			ISubstitutions res = potentialInfinite;
			foreach (var subst in finit)
			{
				res = res.Unify(subst);
				if (res == null)
					return null;
			}
			return res;
		}
	}
}
