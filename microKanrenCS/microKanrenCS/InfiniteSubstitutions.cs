using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace microKanrenCS
{
	class InfiniteSubstitutions : ISubstitutions
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
			var value = substs.First(s => s.Variable == logicVar).Value;
			if (value == null)
				return logicVar;

			var logVal = value as LogicVar;
			if (logVal != null)
				return GetValue(logVal);

			return value;
		}

		public IEnumerator<Substitution> GetEnumerator()
		{
			return substs.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) substs).GetEnumerator();
		}
	}
}
