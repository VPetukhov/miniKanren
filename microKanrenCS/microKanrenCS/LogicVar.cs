using System;

namespace microKanrenCS
{
	public class LogicVar
	{
		public object Value { get; private set; }

		protected LogicVar()
		{ }

		public LogicVar(object value)
		{
			SetValue(value);
		}

		public void SetValue(object value)
		{
			if (IsEvalueted)
				throw new Exception("Variable is already evalueted");

			if (value == null)
				return;

			if (!(value is int) && !(value is double) && !(value is float)
			    && !(value is decimal) && !(value is string))
				throw new Exception("Bad value type");

			Value = value;
		}

		public bool IsEvalueted => Value != null;
	}
}
