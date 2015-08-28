using System;
using System.Collections.Generic;
using System.Linq;

namespace microKanrenCS
{
	public static class Relational
	{
		//public static IReadOnlyList<Substitution> 

		public delegate IEnumerable<Substitution> Goal(IEnumerable<Substitution> substitutions);

		public static Substitutions GetEmptySubst()
		{
			return new Substitutions();
		}

		public static Goal Equal(LogicVar var1, LogicVar var2)
		{
			return subst => subst.Unify(var1, var2);
		}

		public static Goal Equal(LogicVar var1, Func<List<LogicVar>, object> var2, List<LogicVar> var2Args)
		{
			return subst =>
			{
				if (!var2Args.Any())
					return subst.Evaluate(var1, var2(var2Args));

				var newArgs = var2Args.Select(arg => arg.IsEvalueted ? arg : subst.GetValue(arg)).ToList();

				return null;
			};
			//return subst => subst.Unify(var1, var2);
		}

		public static Goal Disjunction(Goal goal1, Goal goal2)
		{
			return subst =>
			{
				var iter1 = goal1(subst).GetEnumerator();
				var iter2 = goal2(subst).GetEnumerator();

				bool finish1 = false, finish2 = false;

				while (finish1 || finish2)
				{
					if (!finish1)
					{
						yield return iter1.Current;
						finish1 = !
					}
				}

				return null;
			};
		}

		private static IEnumerable<Substitution>  
	}
}
