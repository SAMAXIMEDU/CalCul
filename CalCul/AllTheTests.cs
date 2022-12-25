using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CalCul.Calculator;

namespace CalCul
{
	[TestClass]
	public class AllTheTests
	{
		[TestMethod]
		public void CalCul_ShouldTransformExpression_WhenSimpleExpression()
		{
			Assert.AreEqual(CheckExp("2+  2"), "2+2");
		}

		[TestMethod]
		public void CalCul_ShouldTransformExpression_WhenHaveFewNumbers()
		{
			Assert.AreEqual(CheckExp("2 * 2+1"), "2*2+1");
		}

		[TestMethod]
		public void CalCul_ShouldTransformExpression_WhenHavePriority()
		{
			Assert.AreEqual(CheckExp("34   *(21 -11)"), "34*(21+(0-11))");
		}

		[TestMethod]
		public void CalCul_ShouldTransformExpression_WhenHaveHardExpAndPriority()
		{
			Assert.AreEqual(CheckExp("4412,22*(23+ 90*(11))"), "4412,22*(23+90*(11))");
		}

		[TestMethod]
		public void CalCul_ShouldTransformExpression_WhenDontHaveMul()
		{
			Assert.AreEqual(CheckExp("25(25-10(15,1))"), "25*(25+(0-10)*(15,1))");
		}

		[TestMethod]
		public void CalCul_ShouldTransformExpression_WhenUserForgetPriority()
		{
			Assert.AreEqual(CheckExp("(((25-15)* -65))"), "(((25+(0-15))*(0-65)))");
		}

		[TestMethod]
		public void CalCul_ShouldEvaluate_WhenSimpleExpression()
		{
			Assert.AreEqual(Compute("2+  2"), 2 + 2);
		}

		[TestMethod]
		public void CalCul_ShouldEvaluate_WhenHaveFewNumbers()
		{
			Assert.AreEqual(Compute("2 * 2+1"), 2 * 2 + 1);
		}

		[TestMethod]
		public void CalCul_ShouldEvaluate_WhenHavePriority()
		{
			Assert.AreEqual(Compute("34   *(21 -11)"), 34 * (21 - 11));
		}

		[TestMethod]
		public void CalCul_ShouldEvaluate_WhenHaveHardExpAndPriority()
		{
			Assert.AreEqual(Compute("4412,22*(23+ 90*(11))"), 4412.22m * (23m + 90m * (11m)));
		}

		[TestMethod]
		public void CalCul_ShouldEvaluate_WhenDontHaveMul()
		{
			Assert.AreEqual(Compute("25(25-10(15,1))"), 25m * (25m - 10m * (15.1m)));
		}

		[TestMethod]
		public void CalCul_ShouldEvaluate_WhenUserForgetPriority()
		{
			Assert.AreEqual(Compute("(((25-15)* -65))"), (((25m - 15m) * (-65m))));
		}
	}
}