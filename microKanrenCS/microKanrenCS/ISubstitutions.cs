using System.Collections.Generic;

namespace microKanrenCS
{
	public interface ISubstitutions : IEnumerable<Substitution>
	{
		ISubstitutions Extend(LogicVar variable, object value);
		object GetValue(LogicVar logicVar);
	}
}