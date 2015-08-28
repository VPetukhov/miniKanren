using System;

namespace Relational
{
	public class Substitution
	{
		public LogicVar Variable { get; }
		public object Value { get; }

		public Substitution(LogicVar variable, object value)
		{
			if (variable == null)
				throw new ArgumentNullException(nameof(variable));

			if (value == null)
				throw new ArgumentNullException(nameof(value));

			Variable = variable;
			Value = value;
		}

		public override string ToString() => $"{Variable}: {Value} ({Value.GetType().Name})";
    }
}
