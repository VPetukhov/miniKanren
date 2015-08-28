using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace microKanrenCS
{
	public class Substitutions : ISubstitutions
	{
		private readonly Dictionary<LogicVar, object> states;
		
		public Substitutions()
		{
			states = new Dictionary<LogicVar, object>();
		}

		public Substitutions(Substitutions src)
		{
			states = src.states.ToDictionary(s => s.Key, s => s.Value);
		}

		public object GetValue(LogicVar logicVar)
		{
			if (!states.ContainsKey(logicVar))
				return logicVar;
			
			var value = states[logicVar];
			var logVal = value as LogicVar;
			if (logVal != null)
				return GetValue(logVal);

			return value;
		}

		public ISubstitutions Extend(LogicVar variable, object value)
		{
			if (value is LogicVar)
				throw new Exception("value shouldn't be a logic");

			var substitutions = new Substitutions(this);
			substitutions.Add(variable, value);
			return substitutions;
		}

		private void Add(LogicVar var1, object var2)
		{
			states[var1] = var2;
		}

		public IEnumerator<Substitution> GetEnumerator()
		{
			return states.Select(s => new Substitution(s.Key, s.Value)).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}