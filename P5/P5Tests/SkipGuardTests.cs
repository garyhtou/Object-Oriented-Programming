using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guards;

namespace P5Tests;

[TestClass]
public class SkipGuardTests
{
    private readonly int[] inputArray =
        { 1, 4, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

    [TestMethod]
    public void Value_isPrime()
    {
        SkipGuard g = new SkipGuard(inputArray);
        int output = g.Value(5);
        bool notFound = output == 0;
        Assert.IsTrue(IsPrime(output) || notFound);
    }

    [TestMethod]
    public void Value_withUp_isSmallerThanX()
    {
        SkipGuard g = new SkipGuard(inputArray);
        const int x = 5;
        int output = g.Value(x);
        bool notFound = output == 0;
        Assert.IsTrue(output > x || notFound);
    }

    [TestMethod]
    public void Value_withDown_isBiggerThanX()
    {
        SkipGuard g = new SkipGuard(inputArray);
        const int x = 5;
        g.Toggle(); // to down
        int output = g.Value(x);
        bool notFound = output == 0;
        Assert.IsTrue(output < x || notFound);
    }

    [TestMethod]
    public void Value_withDown_output()
    {
        SkipGuard g = new SkipGuard(inputArray);
        g.Toggle();
        int output = g.Value(16);
        const int expected = 13;
        Assert.AreEqual(expected, output);
    }

    [TestMethod]
    public void Value_withUp_output()
    {
        SkipGuard g = new SkipGuard(inputArray);
        int output = g.Value(16);
        const int expected = 17;
        Assert.AreEqual(expected, output);
    }

    private static bool IsPrime(int val)
    {
        if (val <= 2) return false;

        for (int i = 2; i < val; i++)
        {
            if (val % i == 0)
            {
                return false;
            }
        }

        return true;
    }
}