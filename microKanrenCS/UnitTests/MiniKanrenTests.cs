using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Relational.MiniKanren;
using Assert = NUnit.Framework.Assert;

namespace UnitTests
{
	[TestClass]
	public class MiniKanrenTests
	{
		[TestMethod]
		public void TestRun()
		{
			var res = Run(10, x => (Equal(x[0], 10)), 1);
			Assert.AreEqual(1, res.Count);
			Assert.AreEqual(1, res[0].Count);
			Assert.AreEqual(10, res[0][0]);
		}
	}
}
