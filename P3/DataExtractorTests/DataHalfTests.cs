using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using DataExtractors;

namespace DataExtractorTests;

[TestClass]
public class DataHalfTests
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
        new DataHalf(emptyArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Constructor_XZeroMinLength_ThrowsErrors()
    {
        new DataHalf(oneToFiveArray, INVALID_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Constructor_YZeroMinLength_ThrowsErrors()
    {
        new DataHalf(oneToFiveArray, FIVE_MIN_LENGTH, INVALID_MIN_LENGTH);
    }

    [TestMethod]
    public void Constructor_LessThanMinLength_IsInactive()
    {
        DataHalf dh = new(oneToFourArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        Assert.IsTrue(dh.IsInactive());
    }

    [TestMethod]
    public void Constructor_DuplicateValue_IsInactive()
    {
        DataHalf dh = new(duplicateValueArray, ONE_MIN_LENGTH, ONE_MIN_LENGTH);
        Assert.IsTrue(dh.IsInactive());
    }

    [TestMethod]
    public void Constructor_Valid_IsActive()
    {
        DataHalf dh = DataHalf_Active_Factory();
        Assert.IsTrue(dh.IsActive());
    }

    private readonly int[] Any_WithValid_ReturnsAlternatingArray_Expected = { 0, 4, 1, 8, 2 };

    [TestMethod]
    public void Any_WithValid_ReturnsAlternatingArray()
    {
        DataHalf dh = new(oneToFiveArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = dh.Any();

        CollectionAssert.AreEqual(Any_WithValid_ReturnsAlternatingArray_Expected, results);
    }

    [TestMethod]
    public void Any_WithActive_IsActive()
    {
        DataHalf dh = DataHalf_Active_Factory();
        dh.Any();
        Assert.IsTrue(dh.IsActive());
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Any_withInactive_ThrowsException()
    {
        DataHalf dh = DataHalf_Inactive_Factory();
        dh.Any();
    }

    [TestMethod]
    public void Any_1stAnd2nd_SameComposite()
    {
        DataHalf dh = DataHalf_Active_Factory();
        Assert.AreEqual(dh.Any(), dh.Any());
    }

    [TestMethod]
    public void Any_3rdAnd4th_SameComposite()
    {
        DataHalf dh = DataHalf_Active_Factory();

        dh.Any(); // First
        dh.Any(); // Second

        Assert.AreEqual(dh.Any(), dh.Any());
    }

    [TestMethod]
    public void Any_2ndAnd3rd_DifferentComposite()
    {
        DataHalf dh = DataHalf_Active_Factory();

        dh.Any(); // First

        Assert.AreNotEqual(dh.Any(), dh.Any());
    }

    private readonly int[] Target_WithValidAndEven_ReturnsEven_Expected = { 0, 2, 2 };

    [TestMethod]
    public void Target_WithValidAndEven_ReturnsEven()
    {
        DataHalf dh = new(oneToFiveArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = dh.Target(10);

        CollectionAssert.AreEqual(Target_WithValidAndEven_ReturnsEven_Expected, results);
    }

    private readonly int[] Target_WithValidAndOdd_ReturnsOdd_Expected = { 1, 1, 3 };

    [TestMethod]
    public void Target_WithValidAndOdd_ReturnsOdd()
    {
        DataHalf dh = new(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
        int[] results = dh.Target(3);

        CollectionAssert.AreEqual(Target_WithValidAndOdd_ReturnsOdd_Expected, results);
    }

    [TestMethod]
    public void Target_WithActive_IsActive()
    {
        DataHalf dh = DataHalf_Active_Factory();
        dh.Target(10);
        Assert.IsTrue(dh.IsActive());
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Target_withInactive_ThrowsException()
    {
        DataHalf dh = DataHalf_Inactive_Factory();
        dh.Target(10);
    }

    private const int Sum_Even_IsSumOfTarget_Z = 10;

    [TestMethod]
    public void Sum_Even_IsSumOfTarget()
    {
        DataHalf dh = new DataHalf(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);

        int value = dh.Sum(Sum_Even_IsSumOfTarget_Z);
        int expectedValue = oneToTenArray
            .Select(x => x / 2)
            .Where(i => i % 2 == 0)
            .Take(Sum_Even_IsSumOfTarget_Z)
            .Sum();

        Assert.AreEqual(expectedValue, value);
    }

    private const int Sum_Odd_IsSumOfTarget_Z = 9;

    [TestMethod]
    public void Sum_Odd_IsSumOfTarget()
    {
        DataHalf dh = new DataHalf(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);

        int value = dh.Sum(Sum_Odd_IsSumOfTarget_Z);
        int expectedValue = oneToTenArray
            .Select(x => x / 2)
            .Where(i => i % 2 == 1)
            .Take(Sum_Odd_IsSumOfTarget_Z)
            .Sum();

        Assert.AreEqual(expectedValue, value);
    }

    [TestMethod]
    public void Sum_WithActive_IsActive()
    {
        DataHalf dh = DataHalf_Active_Factory();
        dh.Sum(10);
        Assert.IsTrue(dh.IsActive());
    }

    [TestMethod]
    public void DataHalf_WithFailedRequests_IsDeactivated()
    {
        DataHalf dh = DataHalf_Inactive_Factory();

        for (int i = 0; i < oneToFourArray.Last() + 1; i++)
        {
            try
            {
                dh.Any();
            }
            catch (Exception)
            {
                // Intentionally ignore exception from invalid request.
            }
        }

        Assert.IsTrue(dh.IsDeactivated());
    }


    // =========================================================================
    // --------------------------- HELPER METHODS ------------------------------
    // =========================================================================

    private DataHalf DataHalf_Active_Factory()
    {
        return new DataHalf(oneToTenArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }

    private DataHalf DataHalf_Inactive_Factory()
    {
        return new DataHalf(oneToFourArray, FIVE_MIN_LENGTH, FIVE_MIN_LENGTH);
    }
}