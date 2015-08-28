using System.Linq;
using microKanrenCS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace UnitTests
{
	[TestClass]
	public class RelationalTests
	{
		[TestMethod]
		public void TestGetIEnumerable()
		{
			var lst = Enumerable.Range(1, 10);
			var enumerator = lst.GetEnumerator();
			enumerator.MoveNext();
			Assert.AreEqual(10, enumerator.EnumerateFromCurrent().Count());
		}

		[TestMethod]
		public void TestEqualDifferent()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();

			var goal1 = Relational.Equal(v1, 5);
			var goal2 = Relational.Equal(v2, "5");

			var subst1 = goal1(Relational.GetEmptySubst());
			Assert.IsNull(Relational.Equal(6, v1)(subst1));

			var subst2 = goal2(subst1);

			Assert.NotNull(subst2);
			Assert.AreEqual(5, subst2.GetValue(v1));
			Assert.AreEqual("5", subst2.GetValue(v2));

			Assert.IsNull(Relational.Equal(v2, v1)(subst2));
		}

		[TestMethod]
		public void TestEqualSame()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();

			var goal1 = Relational.Equal(v1, 5);
			var goal2 = Relational.Equal(v2, 5);
			var goal3 = Relational.Equal(v2, v1);

			var subst1 = goal1(Relational.GetEmptySubst());
			var subst2 = goal2(subst1);
			var subst3 = goal3(subst2);

			Assert.NotNull(subst3);
			Assert.AreEqual(5, subst3.GetValue(v1));
			Assert.AreEqual(5, subst3.GetValue(v2));
		}

		[TestMethod]
		public void TestEqualList()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();

			var goal = Relational.Equal(new [] {5, 6}, new[] { v1, v2 });
			var subst = goal(Relational.GetEmptySubst());

			Assert.NotNull(subst);
			Assert.AreEqual(5, subst.GetValue(v1));
			Assert.AreEqual(6, subst.GetValue(v2));

			goal = Relational.Equal(new object[] { 5, 5, v1 }, new[] { v1, v1, v2 });
			subst = goal(Relational.GetEmptySubst());

			Assert.NotNull(subst);
			Assert.AreEqual(5, subst.GetValue(v1));
			Assert.AreEqual(5, subst.GetValue(v2));

			goal = Relational.Equal(new[] { 5, 6, 7 }, new[] { v1, v2 });
			Assert.IsNull(goal(Relational.GetEmptySubst()));

			var v3 = new LogicVar();
			goal = Relational.Equal(new[] { 5, 6}, new[] { v1, v2, v3 });
			Assert.IsNull(goal(Relational.GetEmptySubst()));
		}

		[TestMethod]
		public void TestGetValue()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();
			var v3 = new LogicVar();
			var v4 = new LogicVar();

			var subst =
				new InfiniteSubstitutions(new[]
				{new Substitution(v1, 5), new Substitution(v2, v1), new Substitution(v3, v2), new Substitution(v4, v1)});

			Assert.AreEqual(5, subst.GetValue(v1));
			Assert.AreEqual(5, subst.GetValue(v2));
			Assert.AreEqual(5, subst.GetValue(v3));
			Assert.AreEqual(5, subst.GetValue(v4));
		}

		[TestMethod]
		public void TestDisjunction()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();
			var v3 = new LogicVar();

			var goal0 = Relational.Equal(v1, v2);
			var goal1 = Relational.Equal(v1, 5);
			var goal2 = Relational.Equal(v2, 6);

			var conjGoal = Relational.Conjunction(goal0, Relational.Disjunction(goal1, goal2));

			var subst = conjGoal(Relational.GetEmptySubst());
			Assert.NotNull(subst);

			Assert.AreEqual(5, subst.GetValue(v1));
			Assert.AreEqual(5, subst.GetValue(v2));
			Assert.AreEqual(5, subst.GetValue(v3));
		}

		[TestMethod]
		public void TestRecursiveConjunction()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();
			var v3 = new LogicVar();

			var goal1 = Relational.Equal(v1, 5);
			var goal2 = Relational.Equal(v2, v1);
			var goal3 = Relational.Equal(v3, v2);

			var conjGoal = Relational.Conjunction(goal3, Relational.Conjunction(goal1, goal2));

			var subst = conjGoal(Relational.GetEmptySubst());
			Assert.NotNull(subst);
			
			Assert.AreEqual(5, subst.GetValue(v1));
			Assert.AreEqual(5, subst.GetValue(v2));
			Assert.AreEqual(5, subst.GetValue(v3));
		}

		[TestMethod]
		public void TestConjunctionDifferent()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();

			var goal1 = Relational.Equal(v1, 5);
			var goal2 = Relational.Equal(v2, "6");
			var goal3 = Relational.Equal(v2, v1);

			var conjGoal = Relational.Conjunction(goal3, Relational.Conjunction(goal1, goal2));

			var subst = conjGoal(Relational.GetEmptySubst());

			Assert.IsNull(subst);
		}
	}
}
