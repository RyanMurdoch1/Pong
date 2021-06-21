using NSubstitute;
using NUnit.Framework;
using UnityEngine;

public class BallMovementControllerTests
{
    private const int SubstituteMovementSpeed = 1;
    private const int SubstituteBallHeight = 2;
    private const int TestTimeStep = 1;
    private BallMovementController _movementController;
    private IBall _substituteBall;
    private RectTransform _ballRect, _paddleRect;
    private readonly Vector2 _substituteScreenSize = new Vector2(10, 10);

        [SetUp]
        public void Setup()
        {
            _substituteBall = Substitute.For<IBall>();
            _ballRect = CreateGameObjectsWithRectTransform();
            _paddleRect = CreateGameObjectsWithRectTransform();
            _substituteBall.RectTransform = _ballRect;
            _substituteBall.MovementSpeed().Returns(SubstituteMovementSpeed);
            _substituteBall.ReturnBallScreenHeight().Returns(SubstituteBallHeight);
            _substituteBall.ReturnViewScreenSize().Returns(_substituteScreenSize);
            _movementController = new BallMovementController(_substituteBall);
            _movementController.ResetBallPosition();
        }

        private static RectTransform CreateGameObjectsWithRectTransform()
        {
            var newObj = new GameObject();
            var rectTransform = newObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(2, 2);
            return rectTransform;
        }

        [Test] 
        public void Update_Ball_Position_Ball_Moves_Up()
        {
            MoveBallInDirection(Vector3.up);
        }
        
        [Test] 
        public void Update_Ball_Position_Ball_Moves_Down()
        {
            MoveBallInDirection(Vector3.down);
        }
        
        [Test] 
        public void Update_Ball_Position_Ball_Moves_Left()
        {
            MoveBallInDirection(Vector3.left);
        }
        
        [Test] 
        public void Update_Ball_Position_Ball_Moves_Right()
        {
            MoveBallInDirection(Vector3.right);
        }

        private void MoveBallInDirection(Vector3 direction)
        {
            _substituteBall.CurrentDirection = direction;
            _movementController.UpdateBallLocalPosition(TestTimeStep);
            Assert.AreEqual(_substituteBall.RectTransform.localPosition , direction);
        }
        
        [Test] 
        public void Reset_Ball_Position_Ball_Is_Set_To_Centre_Screen()
        {
            _movementController.ResetBallPosition();
            Assert.AreEqual(_ballRect.localPosition, Vector3.zero);
        }

        [Test]
        public void Ball_Exits_Right_Player_One_Scores()
        {
            _substituteBall.BallScreenPosition().Returns(new Vector3(_substituteScreenSize.x + 1, 0));
            _movementController.CheckForScoring();
            _substituteBall.Received().ScoredPoint(Player.PlayerOne);
        }
        
        [Test]
        public void Ball_Exits_Left_Player_Two_Scores()
        {
            _substituteBall.BallScreenPosition().Returns(Vector3.left);
            _movementController.CheckForScoring();
            _substituteBall.Received().ScoredPoint(Player.PlayerTwo);
        }

        [Test]
        public void Check_Collision_Collide_With_Screen_Top()
        {
            _substituteBall.CurrentDirection.Returns(Vector3.up);
            _substituteBall.BallScreenPosition().Returns(new Vector3(0, _substituteScreenSize.y + 1, 0));
            _movementController.CheckForWallCollision();
            Assert.IsFalse(_substituteBall.CurrentDirection == Vector3.up);
            Assert.IsTrue(_substituteBall.CurrentDirection == Vector3.down);
        }
        
        [Test]
        public void Check_Collision_Collide_With_Screen_Top_When_Moving_Down()
        {
            _substituteBall.CurrentDirection.Returns(Vector3.down);
            _substituteBall.BallScreenPosition().Returns(new Vector3(0, _substituteScreenSize.y + 1, 0));
            _movementController.CheckForWallCollision();
            Assert.IsFalse(_substituteBall.CurrentDirection == Vector3.up);
            Assert.IsTrue(_substituteBall.CurrentDirection == Vector3.down);
        }
        
        [Test]
        public void Check_Collision_Collide_With_Screen_Bottom()
        {
            _substituteBall.CurrentDirection.Returns(Vector3.down);
            _substituteBall.BallScreenPosition().Returns(new Vector3(0, -1, 0));
            _movementController.CheckForWallCollision();
            Assert.IsFalse(_substituteBall.CurrentDirection == Vector3.down);
            Assert.IsTrue(_substituteBall.CurrentDirection == Vector3.up);
        }
        
        [Test]
        public void Check_Collision_Collide_With_Screen_Bottom_When_Moving_Up()
        {
            _substituteBall.CurrentDirection.Returns(Vector3.up);
            _substituteBall.BallScreenPosition().Returns(new Vector3(0, -1, 0));
            _movementController.CheckForWallCollision();
            Assert.IsFalse(_substituteBall.CurrentDirection == Vector3.down);
            Assert.IsTrue(_substituteBall.CurrentDirection == Vector3.up);
        }

        [Test]
        public void Check_Collision_Collide_With_Paddle()
        {
            PositionBallAndPaddle(new Vector3(1, 0, 0));
            _movementController.CheckForPaddleCollision(_paddleRect);
            Assert.IsFalse(_substituteBall.CurrentDirection == Vector3.right);
            Assert.IsTrue(_substituteBall.CurrentDirection == Vector3.left);
        }
        
        [Test]
        public void Check_Collision_No_Collisions()
        {
            PositionBallAndPaddle(new Vector3(3, 0, 0));
            _movementController.CheckForPaddleCollision(_paddleRect);
            _movementController.CheckForWallCollision();
            Assert.IsTrue(_substituteBall.CurrentDirection == Vector3.right);
            Assert.IsFalse(_substituteBall.CurrentDirection == Vector3.left);
        }

        private void PositionBallAndPaddle(Vector3 paddlePosition)
        {
            _substituteBall.CurrentDirection = Vector3.right;
            _paddleRect.localPosition = paddlePosition ;
            _substituteBall.RectTransform.localPosition = Vector3.zero;
        }
}
