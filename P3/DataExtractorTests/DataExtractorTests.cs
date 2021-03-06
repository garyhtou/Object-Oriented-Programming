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
    public void Constructor_LessThanMinLength_IsInactive()
    {
        DataExtractor de = new(oneToFourArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        Assert.IsTrue(de.IsInactive());
    }

    [TestMethod]
    public void Constructor_DuplicateValue_IsInactive()
    {
        DataExtractor de = new(duplicateValueArray, ONE_MIN_LENGTH, ONE_MIN_LENGTH);
        Assert.IsTrue(de.IsInactive());
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

        CollectionAssert.AreEqual(Any_WithValid_ReturnsAlternatingArray_Expected, results);
    }

    [TestMethod]
    public void Any_WithActive_IsActive()
    {
        DataExtractor de = DataExtractor_Active_Factory();
        de.Any();
        Assert.IsTrue(de.IsActive());
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Any_withInactive_ThrowsException()
    {
        DataExtractor de = DataExtractor_Inactive_Factory();
        de.Any();
    }

    private readonly int[] Target_WithValidAndEven_ReturnsEven_Expected = { 2, 4 };

    [TestMethod]
    public void Target_WithValidAndEven_ReturnsEven()
    {
        DataExtractor de = new(oneToFiveArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = de.Target(10);

        CollectionAssert.AreEqual(Target_WithValidAndEven_ReturnsEven_Expected, results);
    }

    private readonly int[] Target_WithValidAndOdd_ReturnsOdd_Expected = { 1, 3, 5 };

    [TestMethod]
    public void Target_WithValidAndOdd_ReturnsOdd()
    {
        DataExtractor de = new(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = de.Target(3);

        CollectionAssert.AreEqual(Target_WithValidAndOdd_ReturnsOdd_Expected, results);
    }

    [TestMethod]
    public void Target_WithActive_IsActive()
    {
        DataExtractor de = DataExtractor_Active_Factory();
        de.Target(10);
        Assert.IsTrue(de.IsActive());
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Target_withInactive_ThrowsException()
    {
        DataExtractor de = DataExtractor_Inactive_Factory();
        de.Target(10);
    }

    private const int Sum_Even_IsSumOfTarget_Z = 10;

    [TestMethod]
    public void Sum_Even_IsSumOfTarget()
    {
        DataExtractor de = new DataExtractor(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);

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
        DataExtractor de = new DataExtractor(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);

        int value = de.Sum(Sum_Odd_IsSumOfTarget_Z);
        int expectedValue = oneToTenArray.Where(i => i % 2 == 1)
            .Take(Sum_Odd_IsSumOfTarget_Z)
            .Sum();

        Assert.AreEqual(expectedValue, value);
    }

    [TestMethod]
    public void Sum_WithActive_IsActive()
    {
        DataExtractor de = DataExtractor_Active_Factory();
        de.Sum(10);
        Assert.IsTrue(de.IsActive());
    }

    // =========================================================================
    // --------------------------- HELPER METHODS ------------------------------
    // =========================================================================

    private DataExtractor DataExtractor_Active_Factory()
    {
        return new DataExtractor(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    private DataExtractor DataExtractor_Inactive_Factory()
    {
        return new DataExtractor(oneToFourArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }
}