using System.Collections.Generic;
using System.Linq;

namespace Relational
{
	public static class MicroKanren
	{
		public delegate IEnumerable<ISubstitutions> Goal(ISubstitutions substitutions);

		public static ISubstitutions GetEmptySubst()
		{
			return new InfiniteSubstitutions(); //TODO Fabric
		}

		public static Goal Equal(object var1, object var2)
		{
			return subst =>
			{
				var s = subst.Unify(var1, var2);
				return s == null ? null : new[] { s };
			};
		}

		public static Goal Conjunction(Goal goal1, Goal goal2)
		{
			return subst =>
			{
				var res = Conjunction(subst, goal1, goal2);
				return res.Any() ? res.Select(s => new InfiniteSubstitutions(s)) : null;
			};  //TODO Fabric
		}

		public static Goal Disjunction(Goal goal1, Goal goal2)
		{
			return subst =>
			{
				var res = Disjunction(subst, goal1, goal2);
				return res.Any() ? res.Select(s => new InfiniteSubstitutions(s)) : null;
			};
		}

		private static IEnumerable<IEnumerable<Substitution>> Disjunction(ISubstitutions subst, Goal goal1, Goal goal2)
		{
			var iter1 = goal1(subst).GetEnumerator();
			var iter2 = goal2(subst).GetEnumerator();

			bool firstNotEmpty = iter1.MoveNext(), secondNotEmpty = iter2.MoveNext();

			while (firstNotEmpty || secondNotEmpty)
			{
				if (firstNotEmpty)
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

		private static IEnumerable<IEnumerable<Substitution>> Conjunction(ISubstitutions subst, Goal goal1, Goal goal2)
		{
			foreach (var firstSubst in goal1(subst))
			{
				foreach (var secondSubst in goal2(subst))
				{
					var resSubst = Conjunction(firstSubst, secondSubst);
					if (resSubst != null)
						yield return resSubst;
				}
			}
		}

		private static IEnumerable<Substitution> Conjunction(ISubstitutions firstSubst, ISubstitutions secondSubst)
		{
			var iter1 = firstSubst.GetEnumerator();
			var iter2 = secondSubst.GetEnumerator();

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
