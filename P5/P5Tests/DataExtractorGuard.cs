using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using P5;

namespace P5Tests;

[TestClass]
public class DataExtractorGuardTests
{
    private readonly int[] validArray = { 1, 2, 3, 4, 6, 5 };
    private readonly int[] oneToFiveArray = { 1, 2, 3, 4, 5 };
    private readonly int[] oneToTenArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    private readonly int[] oneToSeventeenArray =
        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

    private readonly int[] duplicateValueArray = { 3, 3 };
    private const uint INVALID_MIN_LENGTH = 0;
    private const uint ONE_MIN_LENGTH = 1;
    private const uint FIVE_MIN_LENGTH = 5;

    [TestMethod]
    public void Constructor_DuplicateValue_IsInactive()
    {
        DataExtractorGuard de = new(duplicateValueArray);
        Assert.IsTrue(de.IsInactive());
    }

    private readonly int[] Any_WithValid_ReturnsAlternatingArray_Expected =
        { 1, 4, 3, 8, 5 };

    private readonly int[] Target_WithValidAndEven_ReturnsEven_Expected =
        { 2, 4 };

    [TestMethod]
    public void Target_output()
    {
        DataExtractorGuard de = new(validArray);
        int[] results = de.Target(10);
        Console.WriteLine(String.Join(", ", results));

        CollectionAssert.AreEqual(Target_WithValidAndEven_ReturnsEven_Expected,
            results);
    }

    private readonly int[] Target_WithValidAndOdd_ReturnsOdd_Expected =
        { 1, 3, 5 };

    [TestMethod]
    public void Target_WithValidAndOdd_ReturnsOdd()
    {
        DataExtractorGuard de = new(oneToTenArray);
        int[] results = de.Target(3);

        CollectionAssert.AreEqual(Target_WithValidAndOdd_ReturnsOdd_Expected,
            results);
    }

    private const int Sum_Even_IsSumOfTarget_Z = 10;

    [TestMethod]
    public void Sum_Even_IsSumOfTarget()
    {
        DataExtractorGuard de = new DataExtractorGuard(oneToTenArray);

        int value = de.Sum(Sum_Even_IsSumOfTarget_Z);
        int expectedValue = oneToTenArray.Where(i => i % 2 == 0)
            .Take(Sum_Even_IsSumOfTarget_Z)
            .Sum();

        Assert.AreEqual(expectedValue, value);
    }

    private const int Sum_Odd_IsSumOfTarget_Z = 9;

    [TestMethod]
    public void Sum_Odd_IsSumOfTarget()
    {
        DataExtractorGuard de = new DataExtractorGuard(oneToTenArray);

        int value = de.Sum(Sum_Odd_IsSumOfTarget_Z);
        int expectedValue = oneToTenArray.Where(i => i % 2 == 1)
            .Take(Sum_Odd_IsSumOfTarget_Z)
            .Sum();

        Assert.AreEqual(expectedValue, value);
    }
}