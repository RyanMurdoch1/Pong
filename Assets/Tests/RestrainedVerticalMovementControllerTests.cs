using NSubstitute;
using NUnit.Framework;
using UnityEngine;

public class RestrainedVerticalMovementControllerTests
{
    private IRestrainedVerticalMovement _movingObject;
    private RestrainedVerticalMovementController _movementController;
    private Transform _transform;
    
    [SetUp]
    public void SetUp()
    {
        _movingObject = Substitute.For<IRestrainedVerticalMovement>();
        _movingObject.GetObjectPixelHeight().Returns(2);
        _movingObject.GetScreenHeightInPixels().Returns(10);
        _movingObject.MovementSpeed.Returns(1);
        var obj = new GameObject();
        _transform = obj.GetComponent<Transform>();
        _movingObject.ObjectTransform.Returns(_transform);
        _movementController = new RestrainedVerticalMovementController(_movingObject);
    }
    
    [Test] 
    public void Move_Object_Up() 
    {
        _movementController.AttemptMoveUp(0, 1);
        Assert.AreEqual(1, _transform.localPosition.y);
    }

    [Test] 
    public void Move_Object_Down()
    {
        _movementController.AttemptMoveDown(1, 1);
        Assert.AreEqual(0, _transform.localPosition.y);
    }

    [Test] 
    public void Restrain_Upward_Movement()
    {
        _movementController.AttemptMoveUp(9, 1);
        Assert.AreNotEqual(1, _transform.localPosition.y);
    }

    [Test] 
    public void Restrain_Downward_Movement()
    {
        _movementController.AttemptMoveDown(0, 1);
        Assert.AreEqual(0, _transform.localPosition.y);
    }
}
