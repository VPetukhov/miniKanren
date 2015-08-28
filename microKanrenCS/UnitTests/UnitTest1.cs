using microKanrenCS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void Test()
		{
			var goal1 = Relational.Equal(g0, 5);
			var goal2 = Relational.Equal(ga, g0);

			var g1 = Relational.Disjunction(goal1, goal2);
			var g2 = Relational.Conjunction(goal1, goal2);

			Assert.That(g1(Relational.GetEmptySubst()) != null);
		}
	}
}
