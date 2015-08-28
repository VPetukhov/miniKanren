namespace microKanrenCS
{
	public struct Substitution
	{
		public LogicVar Variable { get; }
		public object Value { get; set; }

		public Substitution(LogicVar variable, object value)
		{
			Variable = variable;
			Value = value;
		}
	}
}
