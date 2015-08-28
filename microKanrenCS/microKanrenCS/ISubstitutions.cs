using System.Collections.Generic;

namespace Relational
{
	public interface ISubstitutions : IEnumerable<Substitution>
	{
		ISubstitutions Extend(LogicVar variable, object value);
		object GetValue(LogicVar logicVar);
		ISubstitutions Unify(object var1, object var2);
		ISubstitutions Unify(Substitution substitution);
	}
}