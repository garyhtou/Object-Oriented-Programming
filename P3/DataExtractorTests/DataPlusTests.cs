using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using DataExtractors;

namespace DataExtractorTests;

[TestClass]
public class DataPlusTests
{
    private readonly int[] emptyArray = { };
    private readonly int[] oneToFourArray = { 1, 2, 3, 4 };
    private readonly int[] oneToFiveArray = { 1, 2, 3, 4, 5 };
    private readonly int[] oneToTenArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private readonly int[] duplicateValueArray = { 3, 3 };
    private const uint INVALID_MIN_LENGTH = 0;
    private const uint ONE_MIN_LENGTH = 1;
    private const uint FIVE_MIN_LENGTH = 5;

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Constructor_ZeroLength_ThrowsErrors()
    {
        new DataPlus(emptyArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Constructor_XZeroMinLength_ThrowsErrors()
    {
        new DataPlus(oneToFiveArray, INVALID_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Constructor_YZeroMinLength_ThrowsErrors()
    {
        new DataPlus(oneToFiveArray, FIVE_MIN_LENGTH, INVALID_MIN_LENGTH);
    }

    [TestMethod]
    public void Constructor_LessThanMinLength_IsInactive()
    {
        DataPlus dp = new(oneToFourArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        Assert.IsTrue(dp.IsInactive());
    }

    [TestMethod]
    public void Constructor_DuplicateValue_IsInactive()
    {
        DataPlus dp = new(duplicateValueArray, ONE_MIN_LENGTH, ONE_MIN_LENGTH);
        Assert.IsTrue(dp.IsInactive());
    }

    [TestMethod]
    public void Constructor_Valid_IsActive()
    {
        DataPlus dp = DataPlus_Active_Factory();
        Assert.IsTrue(dp.IsActive());
    }

    private readonly int[] Any_WithValid_ReturnsAlternatingArray_Expected = { 1, 4, 3, 8, 5 };

    [TestMethod]
    public void Any_WithValid_ReturnsAlternatingArray()
    {
        DataPlus dp = new(oneToFiveArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = dp.Any();

        CollectionAssert.AreEqual(Any_WithValid_ReturnsAlternatingArray_Expected, results);
    }

    [TestMethod]
    public void Any_WithActive_IsActive()
    {
        DataPlus dp = DataPlus_Active_Factory();
        dp.Any();
        Assert.IsTrue(dp.IsActive());
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Any_withInactive_ThrowsException()
    {
        DataPlus dp = DataPlus_Inactive_Factory();
        dp.Any();
    }

    private readonly int[] Target_WithZ10_ReturnsOddXAndEvenY_Expected = { 1, 3, 5, 2, 4, 6, 8, 10 };

    [TestMethod]
    public void Target_WithZ10_ReturnsOddXAndEvenY()
    {
        DataPlus dp = new(oneToFiveArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = dp.Target(10);

        CollectionAssert.AreEqual(Target_WithZ10_ReturnsOddXAndEvenY_Expected, results);
    }

    private readonly int[] Target_WithZ1_ReturnsOddXAndEvenY_Expected = { 1, 2 };

    [TestMethod]
    public void Target_WithZ1_ReturnsOddXAndEvenY()
    {
        DataPlus dp = new(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = dp.Target(1);

        CollectionAssert.AreEqual(Target_WithZ1_ReturnsOddXAndEvenY_Expected, results);
    }

    [TestMethod]
    public void Target_WithActive_IsActive()
    {
        DataPlus dp = DataPlus_Active_Factory();
        dp.Target(10);
        Assert.IsTrue(dp.IsActive());
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Target_withInactive_ThrowsException()
    {
        DataPlus dp = DataPlus_Inactive_Factory();
        dp.Target(10);
    }

    private const int Sum_Even_IsSumOfTarget_Z = 10;

    [TestMethod]
    public void Sum_Even_IsSumOfTarget()
    {
        DataPlus dp = new DataPlus(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);

        int value = dp.Sum(Sum_Even_IsSumOfTarget_Z);
        int expectedValue = oneToTenArray.Where(i => i % 2 == 0)
            .Take(Sum_Even_IsSumOfTarget_Z)
            .Sum();

        Assert.AreEqual(expectedValue, value);
    }

    private const int Sum_Odd_IsSumOfTarget_Z = 9;

    [TestMethod]
    public void Sum_Odd_IsSumOfTarget()
    {
        DataPlus dp = new DataPlus(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);

        int value = dp.Sum(Sum_Odd_IsSumOfTarget_Z);
        int expectedValue = oneToTenArray.Where(i => i % 2 == 1)
            .Take(Sum_Odd_IsSumOfTarget_Z)
            .Sum();

        Assert.AreEqual(expectedValue, value);
    }

    [TestMethod]
    public void Sum_WithActive_IsActive()
    {
        DataPlus dp = DataPlus_Active_Factory();
        dp.Sum(10);
        Assert.IsTrue(dp.IsActive());
    }

    private readonly int[] AddToY_EveryKRequests_X = { 5, 2 };

    [TestMethod]
    public void AddToY_EveryKRequests()
    {
        DataPlus dp = new(AddToY_EveryKRequests_X, ONE_MIN_LENGTH, ONE_MIN_LENGTH);

        int k = AddToY_EveryKRequests_X.Last();

        int[] initialTarget = dp.Target(10);
        for (int i = 0; i < k * 2; i++)
        {
            dp.Any();
        }

        int[] finalTarget = dp.Target(10);
        CollectionAssert.AreNotEqual(initialTarget, finalTarget);
    }

    // =========================================================================
    // --------------------------- HELPER METHODS ------------------------------
    // =========================================================================

    private DataPlus DataPlus_Active_Factory()
    {
        return new DataPlus(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    private DataPlus DataPlus_Inactive_Factory()
    {
        return new DataPlus(oneToFourArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }
}