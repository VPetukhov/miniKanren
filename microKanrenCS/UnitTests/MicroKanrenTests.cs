using System.Linq;
using Relational;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace UnitTests
{
	[TestClass]
	public class MicroKanrenTests
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

			var goal1 = MicroKanren.Equal(v1, 5);
			var goal2 = MicroKanren.Equal(v2, "5");

			var subst1 = goal1(MicroKanren.GetEmptySubst()).SingleOrDefault();
			Assert.IsNull(MicroKanren.Equal(6, v1)(subst1));

			var subst2 = goal2(subst1).SingleOrDefault();

			Assert.NotNull(subst2);
			Assert.AreEqual(5, subst2.GetValue(v1));
			Assert.AreEqual("5", subst2.GetValue(v2));

			Assert.IsNull(MicroKanren.Equal(v2, v1)(subst2));
		}

		[TestMethod]
		public void TestEqualSame()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();

			var goal1 = MicroKanren.Equal(v1, 5);
			var goal2 = MicroKanren.Equal(v2, 5);
			var goal3 = MicroKanren.Equal(v2, v1);

			var subst1 = goal1(MicroKanren.GetEmptySubst()).Single();
			var subst2 = goal2(subst1).Single();
			var subst3 = goal3(subst2).SingleOrDefault();

			Assert.NotNull(subst3);
			Assert.AreEqual(5, subst3.GetValue(v1));
			Assert.AreEqual(5, subst3.GetValue(v2));
		}

		[TestMethod]
		public void TestEqualList()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();

			var goal = MicroKanren.Equal(new [] {5, 6}, new[] { v1, v2 });
			var subst = goal(MicroKanren.GetEmptySubst()).SingleOrDefault();

			Assert.NotNull(subst);
			Assert.AreEqual(5, subst.GetValue(v1));
			Assert.AreEqual(6, subst.GetValue(v2));

			goal = MicroKanren.Equal(new object[] { 5, 5, v1 }, new[] { v1, v1, v2 });
			subst = goal(MicroKanren.GetEmptySubst()).SingleOrDefault();

			Assert.NotNull(subst);
			Assert.AreEqual(5, subst.GetValue(v1));
			Assert.AreEqual(5, subst.GetValue(v2));

			goal = MicroKanren.Equal(new[] { 5, 6, 7 }, new[] { v1, v2 });
			Assert.IsNull(goal(MicroKanren.GetEmptySubst()));

			var v3 = new LogicVar();
			goal = MicroKanren.Equal(new[] { 5, 6}, new[] { v1, v2, v3 });
			Assert.IsNull(goal(MicroKanren.GetEmptySubst()));
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

			var goal0 = MicroKanren.Equal(v1, v2);
			var goal1 = MicroKanren.Equal(v1, 5);
			var goal2 = MicroKanren.Equal(v2, 6);

			var conjGoal = MicroKanren.Conjunction(goal0, MicroKanren.Disjunction(goal1, goal2));

			var subst = conjGoal(MicroKanren.GetEmptySubst());
			Assert.NotNull(subst);

			var substList = subst.ToList();

			Assert.AreEqual(5, substList[0].GetValue(v1));
			Assert.AreEqual(5, substList[0].GetValue(v2));
			Assert.AreEqual(6, substList[1].GetValue(v1));
			Assert.AreEqual(6, substList[1].GetValue(v2));
		}

		[TestMethod]
		public void TestRecursiveConjunction()
		{
			var v1 = new LogicVar();
			var v2 = new LogicVar();
			var v3 = new LogicVar();

			var goal1 = MicroKanren.Equal(v1, 5);
			var goal2 = MicroKanren.Equal(v2, v1);
			var goal3 = MicroKanren.Equal(v3, v2);

			var conjGoal = MicroKanren.Conjunction(goal3, MicroKanren.Conjunction(goal1, goal2));

			var subst = conjGoal(MicroKanren.GetEmptySubst()).SingleOrDefault();
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

			var goal1 = MicroKanren.Equal(v1, 5);
			var goal2 = MicroKanren.Equal(v2, "6");
			var goal3 = MicroKanren.Equal(v2, v1);

			var conjGoal = MicroKanren.Conjunction(goal3, MicroKanren.Conjunction(goal1, goal2));

			var subst = conjGoal(MicroKanren.GetEmptySubst());

			Assert.IsNull(subst);
		}
	}
}
