using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Guards;

namespace P5Tests;

[TestClass]
public class QuirkyGuardTests
{
    private readonly int[] inputArray =
        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

    [TestMethod]
    public void Value_isPrime()
    {
        QuirkyGuard g = new QuirkyGuard(inputArray);
        int output = g.Value(5);
        bool notFound = output == 0;
        Assert.IsTrue(IsPrime(output) || notFound);
    }

    [TestMethod]
    public void Value_withUp_isSmallerThanX()
    {
        QuirkyGuard g = new QuirkyGuard(inputArray);
        const int x = 5;
        int output = g.Value(x);
        bool notFound = output == 0;
        Assert.IsTrue(output > x || notFound);
    }

    [TestMethod]
    public void Value_withDown_isBiggerThanX()
    {
        QuirkyGuard g = new QuirkyGuard(inputArray);
        const int x = 5;
        g.Toggle(); // to down
        int output = g.Value(x);
        bool notFound = output == 0;
        Assert.IsTrue(output < x || notFound);
    }

    [TestMethod]
    public void Value_withDown_output()
    {
        QuirkyGuard g = new QuirkyGuard(inputArray);
        g.Toggle();
        int output = g.Value(16);
        const int expected = 13;
        Assert.AreEqual(expected, output);
    }

    [TestMethod]
    public void Value_appends()
    {
        QuirkyGuard g = new QuirkyGuard(inputArray);
        int output1 = g.Value(17);
        for (int i = 0; i < 10; i++)
        {
            g.Value(17);
        }

        int output2 = g.Value(17);

        Assert.AreNotEqual(output1, output2);
    }

    [TestMethod]
    public void Value_withUp_output()
    {
        QuirkyGuard g = new QuirkyGuard(inputArray);
        int output = g.Value(16);
        const int expected = 17;
        Assert.AreEqual(expected, output);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void ToggleLimit()
    {
        QuirkyGuard g = new QuirkyGuard(inputArray);

        int limit = inputArray[0] + 1;
        for (int i = 0; i < limit; i++)
        {
            g.Toggle();
        }
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