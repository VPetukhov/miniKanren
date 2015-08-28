using System;

namespace microKanrenCS
{
	public class LogicVar
	{
		private static int id;

		public int Id { get; }

		public LogicVar()
		{
			Id = id++;
		}

		public override string ToString() => $"{Id}";
	}
}
