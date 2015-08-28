using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Relational.MicroKanren;

namespace Relational
{
	public static class MiniKanren
	{
		public delegate Goal RunDelegate(List<LogicVar> args);

		public static Goal Equal(object var1, object var2)
		{
			return MicroKanren.Equal(var1, var2);
		}

		public static List<List<object>> Run(int stepsCount, RunDelegate func, int argsCount)
		{
			if (stepsCount == 0)
				return new List<List<object>>();

			var args = Enumerable.Range(1, argsCount).Select(i => new LogicVar()).ToList();
			var substs = func(args)(GetEmptySubst());

			int stepNumber = 0;
			return substs.TakeWhile(subst => stepNumber++ != stepsCount)
				.Select(subst => args
					.Select(a =>
					{
						var val = subst.GetValue(a);
						return val is LogicVar ? null : val;
					}).ToList())
				.ToList();
		}
	}
}
