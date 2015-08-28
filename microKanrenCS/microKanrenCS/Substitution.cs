namespace microKanrenCS
{
	public class Substitution
	{
		public Substitution(LogicVar @base, LogicValue target)
		{
			Base = @base;
			Target = target;
		}

		public LogicVar Base { get; }
		public LogicValue Target { get; }


	}
}
