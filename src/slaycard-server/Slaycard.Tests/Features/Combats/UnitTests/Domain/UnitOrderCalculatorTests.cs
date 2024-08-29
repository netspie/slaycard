using Slaycard.Api.Features.Combats.Domain;

namespace Slaycard.Tests.Features.Combats.UnitTests.Domain;

public class UnitOrderCalculatorTests
{
    [Test]
    [TestCase(new int[] { 1, 1, 1, 1 }, new int[] { 0, 1, 2, 3 }, TestName = "[1, 1, 1, 1]")]
    [TestCase(new int[] { 2, 1, 1, 1 }, new int[] { 0, 1, 0, 2, 3 }, TestName = "[2, 1, 1, 1]")]
    [TestCase(new int[] { 20, 10, 10, 10 }, new int[] { 0, 1, 0, 2, 3 }, TestName = "[20, 10, 10, 10]")]
    [TestCase(new int[] { 17, 13, 10, 10 }, new int[] { 0, 1, 0, 1, 2, 0, 2, 1, 0, 3, 2, 0, 1, 3, 0, 1, 2, 0, 3, 1, 0, 2, 3, 0, 1, 2, 0, 1, 3, 0, 1, 2, 0, 3, 1, 0, 2, 3, 0, 1, 2, 0, 1, 3, 0, 2, 1, 0, 3, 3 }, TestName = "[17, 13, 10, 10]")]
    [TestCase(new int[] { 20, 10, 10, 5, 4, 2 }, new int[] { 0, 1, 0, 2, 0, 1, 2, 0, 3, 0, 1, 0, 2, 3, 0, 1, 0, 2, 0, 4, 1, 0, 2, 0, 3, 1, 0, 2, 0, 4, 0, 1, 2, 0, 3, 0, 1, 0, 2, 4, 0, 1, 0, 2, 5 }, TestName = "[20, 10, 10, 5, 4, 2]")]
    public void TestCalculateActionOrder(int[] speeds, int[] expected)
    {
        var order = UnitOrderCalculator.CalculateActionOrder(speeds);
        var orderStr = order.Select(i => i.ToString()).Aggregate((x, y) => $"{x}, {y}");

        Assert.IsTrue(order.SequenceEqual(expected));
    }
}
