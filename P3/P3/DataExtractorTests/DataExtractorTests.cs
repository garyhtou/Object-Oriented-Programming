using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using DataExtractors;

namespace DataExtractorTests;

[TestClass]
public class DataExtractorTests
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
        new DataExtractor(emptyArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Constructor_XZeroMinLength_ThrowsErrors()
    {
        new DataExtractor(oneToFiveArray, INVALID_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Constructor_YZeroMinLength_ThrowsErrors()
    {
        new DataExtractor(oneToFiveArray, FIVE_MIN_LENGTH, INVALID_MIN_LENGTH);
    }

    [TestMethod]
    public void Constructor_LessThanMinLength_IsDeactivated()
    {
        DataExtractor de = new(oneToFourArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        Assert.IsTrue(de.IsDeactivated());
    }

    [TestMethod]
    public void Constructor_DuplicateValue_IsDeactivated()
    {
        DataExtractor de = new(duplicateValueArray, ONE_MIN_LENGTH, ONE_MIN_LENGTH);
        Assert.IsTrue(de.IsDeactivated());
    }

    [TestMethod]
    public void Constructor_Valid_IsActive()
    {
        DataExtractor de = DataExtractor_Active_Factory();
        Assert.IsTrue(de.IsActive());
    }

    private readonly int[] Any_WithValid_ReturnsAlternatingArray_Expected = { 1, 4, 3, 8, 5 };

    [TestMethod]
    public void Any_WithValid_ReturnsAlternatingArray()
    {
        DataExtractor de = new(oneToFiveArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = de.Any();

        Assert.AreEqual(results, Any_WithValid_ReturnsAlternatingArray_Expected);
    }

    [TestMethod]
    public void Any_WithActive_IsActive()
    {
        DataExtractor de = DataExtractor_Active_Factory();
        de.Any();
        Assert.IsTrue(de.IsActive());
    }

    [TestMethod]
    public void Any_withDeactivated_IsDeactivated()
    {
        DataExtractor de = DataExtractor_Deactivated_Factory();
        de.Any();
        Assert.IsTrue(de.IsDeactivated());
    }

    private readonly int[] Target_WithValidAndEven_ReturnsEven_Expected = { 2, 4 };

    [TestMethod]
    public void Target_WithValidAndEven_ReturnsEven()
    {
        DataExtractor de = new(oneToFiveArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = de.Target(10);

        Assert.Equals(results, Target_WithValidAndEven_ReturnsEven_Expected);
    }

    private readonly int[] Target_WithValidAndOdd_ReturnsOdd_Expected = { 1, 3, 5 };

    [TestMethod]
    public void Target_WithValidAndOdd_ReturnsOdd()
    {
        DataExtractor de = new(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = de.Target(3);

        Assert.Equals(results, Target_WithValidAndOdd_ReturnsOdd_Expected);
    }

    [TestMethod]
    public void Target_WithActive_IsActive()
    {
        DataExtractor de = DataExtractor_Active_Factory();
        de.Target(10);
        Assert.IsTrue(de.IsActive());
    }

    [TestMethod]
    public void Target_withDeactivated_IsDeactivated()
    {
        DataExtractor de = DataExtractor_Deactivated_Factory();
        de.Target(10);
        Assert.IsTrue(de.IsDeactivated());
    }

    private const int Sum_Even_IsSumOfTarget_Z = 10;

    [TestMethod]
    public void Sum_Even_IsSumOfTarget()
    {
        DataExtractor de = new DataExtractor(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);

        int value = de.Sum(Sum_Even_IsSumOfTarget_Z);
        int expectedValue = de.Target(Sum_Even_IsSumOfTarget_Z).Sum();

        Assert.Equals(value, expectedValue);
    }

    private const int Sum_Odd_IsSumOfTarget_Z = 9;

    [TestMethod]
    public void Sum_Odd_IsSumOfTarget()
    {
        DataExtractor de = new DataExtractor(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);

        int value = de.Sum(Sum_Odd_IsSumOfTarget_Z);
        int expectedValue = de.Target(Sum_Odd_IsSumOfTarget_Z).Sum();

        Assert.Equals(value, expectedValue);
    }

    [TestMethod]
    public void Sum_WithActive_IsActive()
    {
        DataExtractor de = DataExtractor_Active_Factory();
        de.Sum(10);
        Assert.IsTrue(de.IsActive());
    }

    [TestMethod]
    public void Sum_withDeactivated_IsDeactivated()
    {
        DataExtractor de = DataExtractor_Deactivated_Factory();
        de.Sum(10);
        Assert.IsTrue(de.IsDeactivated());
    }

    // =========================================================================
    // --------------------------- HELPER METHODS ------------------------------
    // =========================================================================

    private DataExtractor DataExtractor_Active_Factory()
    {
        return new DataExtractor(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    private DataExtractor DataExtractor_Deactivated_Factory()
    {
        return new DataExtractor(oneToFourArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }
}