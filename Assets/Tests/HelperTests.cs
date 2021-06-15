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
}
