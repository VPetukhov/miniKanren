using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Relational
{
	public class InfiniteSubstitutions : ISubstitutions
	{
		private readonly IEnumerable<Substitution> substs;

		public InfiniteSubstitutions(IEnumerable<Substitution> substs = null)
		{
			this.substs = substs ?? Enumerable.Empty<Substitution>();
		}

		public ISubstitutions Extend(LogicVar variable, object value)
		{
			return new InfiniteSubstitutions(substs.Concat(new [] {new Substitution(variable, value)}));
		}

		public object GetValue(LogicVar logicVar)
		{
			var value = substs.FirstOrDefault(s => s.Variable == logicVar)?.Value;
			if (value == null)
				return logicVar;

			var logVal = value as LogicVar;
			if (logVal != null)
				return GetValue(logVal);

			return value;
		}

		public ISubstitutions Unify(object var1, object var2)
		{
			if (var1 == null)
				throw new ArgumentNullException(nameof(var1));

			if (var2 == null)
				throw new ArgumentNullException(nameof(var2));

			var logVar1 = var1 as LogicVar;
			var logVar2 = var2 as LogicVar;

			var val1 = logVar1 == null ? var1 : GetValue(logVar1);
			var val2 = logVar2 == null ? var2 : GetValue(logVar2);

			var logVal1 = val1 as LogicVar;
			var logVal2 = val2 as LogicVar;
			if (logVal1 != null && logVal2 != null && logVal1 == logVal2)
				return this;

			if (logVal1 != null)
				return Extend(logVal1, val2);

			if (logVal2 != null)
				return Extend(logVal2, val1);

			var list1 = val1 as IEnumerable;
			var list2 = val2 as IEnumerable;

			if (list1 != null && list2 != null)
			{
				var iter1 = list1.GetEnumerator();
				iter1.MoveNext();

				var iter2 = list2.GetEnumerator();
				iter2.MoveNext();

				var s = Unify(iter1.Current, iter2.Current);

				var move1 = iter1.MoveNext();
				var move2 = iter2.MoveNext();

				if (s == null || move1 != move2)
					return null;

				if (move1)
				{
					s = s.Unify(iter1.EnumerateFromCurrent(), iter2.EnumerateFromCurrent());
				}

				return s;
			}

			if (val1.GetType() != val2.GetType())
				return null;

			dynamic dval1 = val1, dval2 = val2;

			return dval1 == dval2 ? this : null;
		}

		public ISubstitutions Unify(Substitution substitution)
		{
			return Unify(substitution.Variable, substitution.Value);
		}

		public IEnumerator<Substitution> GetEnumerator() => substs.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) substs).GetEnumerator();
	}
}
