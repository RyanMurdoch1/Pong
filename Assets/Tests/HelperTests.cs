using NUnit.Framework;
using UnityEngine;

public class HelperTests
{
    private RectTransform _rectOne, _rectTwo;

    [SetUp]
    public void Setup()
    {
        var goOne = new GameObject();
        _rectOne = goOne.AddComponent<RectTransform>();
        _rectOne.sizeDelta = new Vector2(10, 10);
        
        var goTwo = new GameObject();
        _rectTwo = goTwo.AddComponent<RectTransform>();
        _rectTwo.sizeDelta = new Vector2(10, 10);
    }
    
    [Test] 
    public void RectOverlaps_Rects_Do_Overlap()
    {
        _rectOne.localPosition = new Vector3(0, 0, 0);
        _rectTwo.localPosition = new Vector3(0, 0, 0);
        Assert.IsTrue(Helper.RectOverlaps(_rectOne, _rectTwo));
    }

    [Test]
    public void RectOverlaps_Rects_Do_Not_Overlap()
    {
        _rectOne.localPosition = new Vector3(-1000, 0, 0);
        _rectTwo.localPosition = new Vector3(1000, 0, 0);
        Assert.IsFalse(Helper.RectOverlaps(_rectOne, _rectTwo));
    }

    [Test]
    public void GetRequiredYPosition_Objects_Are_Level_With_Upwards_Direction_Returns_One()
    {
        var objectPosition = new Vector3(1, 0);
        var targetPosition = new Vector3(0, 0);
        var targetDirection = new Vector2(1, 1);
        Assert.AreEqual(1f, Helper.GetRequiredYPosition(objectPosition, targetPosition, targetDirection), double.Epsilon);
    }
    
    [Test]
    public void GetRequiredYPosition_Objects_Are_Level_With_Downwards_Direction_Returns_Negative_One()
    {
        var objectPosition = new Vector3(1, 0);
        var targetPosition = new Vector3(0, 0);
        var targetDirection = new Vector2(1, -1);
        Assert.AreEqual(-1f, Helper.GetRequiredYPosition(objectPosition, targetPosition, targetDirection), double.Epsilon);
    }
    
    [Test]
    public void GetRequiredYPosition_Objects_Are_Level_With_Forwards_Direction_Returns_Zero()
    {
        var objectPosition = new Vector3(1, 0);
        var targetPosition = new Vector3(0, 0);
        var targetDirection = new Vector2(1, 0);
        Assert.AreEqual(0f, Helper.GetRequiredYPosition(objectPosition, targetPosition, targetDirection), double.Epsilon);
    }
    
    [Test]
    public void GetRequiredYPosition_Target_Is_Above_With_Forwards_Direction_Returns_One()
    {
        var objectPosition = new Vector3(1, 0);
        var targetPosition = new Vector3(0, 1);
        var targetDirection = new Vector2(1, 0);
        Assert.AreEqual(1f, Helper.GetRequiredYPosition(objectPosition, targetPosition, targetDirection), double.Epsilon);
    }
    
    [Test]
    public void GetRequiredYPosition_Target_Is_Below_With_Forwards_Direction_Returns_Negative_One()
    {
        var objectPosition = new Vector3(1, 0);
        var targetPosition = new Vector3(0, -1);
        var targetDirection = new Vector2(1, 0);
        Assert.AreEqual(-1f, Helper.GetRequiredYPosition(objectPosition, targetPosition, targetDirection), double.Epsilon);
    }
    
    [Test]
    public void GetRequiredYPosition_Target_Is_Moving_Away_From_Object_Returns_Zero()
    {
        var objectPosition = new Vector3(1, 0);
        var targetPosition = new Vector3(0, -1);
        var targetDirection = new Vector2(-1, 0);
        Assert.AreEqual(0f, Helper.GetRequiredYPosition(objectPosition, targetPosition, targetDirection), double.Epsilon);
    }
}

