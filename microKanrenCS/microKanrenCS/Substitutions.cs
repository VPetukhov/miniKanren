using System.Collections.Generic;
using System.Linq;

namespace microKanrenCS
{
	public class Substitutions
	{
		private readonly Dictionary<LogicVar, LogicVar> states;
		
		public Substitutions()
		{
			states = new Dictionary<LogicVar, LogicVar>();
		}

		public Substitutions(Substitutions src)
		{
			states = src.states.ToDictionary(s => s.Key, s => s.Value);
		}

		//public 
		public Substitutions Unify(LogicVar var1, LogicVar var2)
		{
			var substitutions = new Substitutions(this);
			substitutions.Add(var1, var2);
			return substitutions;
		}

		private void Add(LogicVar var1, LogicVar var2)
		{
			
		}

		public LogicVar GetValue(LogicVar logicVar)
		{
			return states.ContainsKey(logicVar) 
				? GetValue(states[logicVar]) 
				: logicVar;
		}
	}
}