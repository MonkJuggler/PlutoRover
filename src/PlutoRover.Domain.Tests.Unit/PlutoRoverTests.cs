using System;
using Moq;
using Xunit;

namespace PlutoRover.Domain.Tests.Unit
{
    public class PlutoRoverTests
    {
        private PlutoRover _plutoRover;
        private Position _defaultPosition = new Position(0, 0, Direction.North);
        private Mock<ICheckObstacles> _mockObstaclesChecker;
        private int _defaultGridWidth = 100;
        private int _defaultGridHeight = 100;

        public PlutoRoverTests()
        {
            _mockObstaclesChecker = new Mock<ICheckObstacles>();
            _mockObstaclesChecker
                .Setup(x => x.ObstacleExist(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            _plutoRover = new PlutoRover(_defaultPosition, _defaultGridWidth, _defaultGridHeight, _mockObstaclesChecker.Object);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("FBLR ")]
        [InlineData(" FBLR ")]
        [InlineData("FB LR")]
        [InlineData("FBALR")]
        [InlineData("fblr")]
        [InlineData("FBLRr ")]
        public void ExecuteCommands_WhenInputIsInvalid_AnArgumentExceptionIsThrown(string cmd)
        {
            var result = _plutoRover.ExecuteCommands(cmd);

            Assert.Equal(Status.Failure, result.Status);
        }

        [Theory]
        [InlineData(1, 1, Direction.North, 1, 2)]
        [InlineData(1, 1, Direction.East, 2, 1)]
        [InlineData(1, 1, Direction.South, 1, 0)]
        [InlineData(1, 1, Direction.West, 0, 1)]
        public void ExecuteCommands_WhenMoveForwardCommand_TheRoverMovesForward(int start_x, int start_y, Direction direction, int end_x, int end_y)
        {
            _plutoRover = new PlutoRover(start_x, start_y, direction, _defaultGridWidth, _defaultGridHeight, _mockObstaclesChecker.Object);

            _plutoRover.ExecuteCommands("F");

            Assert.Equal(end_x, _plutoRover.Position.X);
            Assert.Equal(end_y, _plutoRover.Position.Y);
            Assert.Equal(direction, _plutoRover.Position.Direction);
        }

        [Theory]
        [InlineData(1, 1, Direction.North, 1, 0)]
        [InlineData(1, 1, Direction.East, 0, 1)]
        [InlineData(1, 1, Direction.South, 1, 2)]
        [InlineData(1, 1, Direction.West, 2, 1)]
        public void ExecuteCommands_WhenMoveBackwardCommand_TheRoverMovesBackward(int start_x, int start_y, Direction direction, int end_x, int end_y)
        {
            _plutoRover = new PlutoRover(start_x, start_y, direction, _defaultGridWidth, _defaultGridHeight, _mockObstaclesChecker.Object);

            _plutoRover.ExecuteCommands("B");

            Assert.Equal(end_x, _plutoRover.Position.X);
            Assert.Equal(end_y, _plutoRover.Position.Y);
            Assert.Equal(direction, _plutoRover.Position.Direction);
        }

        [Theory]
        [InlineData(1, 1, Direction.North, Direction.East)]
        [InlineData(1, 1, Direction.East, Direction.South)]
        [InlineData(1, 1, Direction.South, Direction.West)]
        [InlineData(1, 1, Direction.West, Direction.North)]
        public void ExecuteCommands_WhenRotateRightCommand_TheRoverRotatesRight(int x, int y, Direction start_dir, Direction end_dir)
        {
            _plutoRover = new PlutoRover(x, y, start_dir, _defaultGridWidth, _defaultGridHeight, _mockObstaclesChecker.Object);

            _plutoRover.ExecuteCommands("R");

            Assert.Equal(x, _plutoRover.Position.X);
            Assert.Equal(y, _plutoRover.Position.Y);
            Assert.Equal(end_dir, _plutoRover.Position.Direction);
        }

        [Theory]
        [InlineData(1, 1, Direction.North, Direction.West)]
        [InlineData(1, 1, Direction.West, Direction.South)]
        [InlineData(1, 1, Direction.South, Direction.East)]
        [InlineData(1, 1, Direction.East, Direction.North)]
        public void ExecuteCommands_WhenRotateLeftCommand_TheRoverRotatesLeft(int x, int y, Direction start_dir, Direction end_dir)
        {
            _plutoRover = new PlutoRover(x, y, start_dir, _defaultGridWidth, _defaultGridHeight, _mockObstaclesChecker.Object);

            _plutoRover.ExecuteCommands("L");

            Assert.Equal(x, _plutoRover.Position.X);
            Assert.Equal(y, _plutoRover.Position.Y);
            Assert.Equal(end_dir, _plutoRover.Position.Direction);
        }

        [Theory]
        [InlineData(1, 1, 2, 2, Direction.North, 1, 0)]
        [InlineData(1, 1, 2, 2, Direction.East, 0, 1)]
        [InlineData(1, 0, 2, 2, Direction.South, 1, 1)]
        [InlineData(0, 1, 2, 2, Direction.West, 1, 1)]
        [InlineData(2, 1, 3, 2, Direction.North, 2, 0)]
        [InlineData(2, 1, 3, 2, Direction.East, 0, 1)]
        [InlineData(2, 0, 3, 2, Direction.South, 2, 1)]
        [InlineData(0, 0, 3, 2, Direction.West, 2, 0)]
        public void ExecuteCommands_WhenMoveForwardCommandOverTheGridLimit_TheRoverWrapsAround(
            int start_x, int start_y, int gridWidth, int gridHeight, Direction direction, int end_x, int end_y)
        {
            _plutoRover = new PlutoRover(start_x, start_y, direction, gridWidth, gridHeight, _mockObstaclesChecker.Object);

            _plutoRover.ExecuteCommands("F");

            Assert.Equal(end_x, _plutoRover.Position.X);
            Assert.Equal(end_y, _plutoRover.Position.Y);
            Assert.Equal(direction, _plutoRover.Position.Direction);
        }

        [Theory]
        [InlineData(0, 0, 2, 2, Direction.North, 0, 1)]
        [InlineData(0, 0, 2, 2, Direction.East, 1, 0)]
        [InlineData(1, 1, 2, 2, Direction.South, 1, 0)]
        [InlineData(1, 0, 2, 2, Direction.West, 0, 0)]
        [InlineData(2, 0, 3, 2, Direction.North, 2, 1)]
        [InlineData(0, 1, 3, 2, Direction.East, 2, 1)]
        [InlineData(2, 1, 3, 2, Direction.South, 2, 0)]
        [InlineData(2, 0, 3, 2, Direction.West, 0, 0)]
        public void ExecuteCommands_WhenMoveBackwardCommandOverTheGridLimit_TheRoverWrapsAround(
            int start_x, int start_y, int gridWidth, int gridHeight, Direction direction, int end_x, int end_y)
        {
            _plutoRover = new PlutoRover(start_x, start_y, direction, gridWidth, gridHeight, _mockObstaclesChecker.Object);

            _plutoRover.ExecuteCommands("B");

            Assert.Equal(end_x, _plutoRover.Position.X);
            Assert.Equal(end_y, _plutoRover.Position.Y);
            Assert.Equal(direction, _plutoRover.Position.Direction);
        }

        [Theory]
        [InlineData(1, 1, Direction.North, 1, 2, "FFF")]
        [InlineData(1, 1, Direction.East, 2, 1, "FFF")]
        [InlineData(1, 1, Direction.South, 1, 0, "FFF")]
        [InlineData(1, 1, Direction.West, 0, 1, "FFF")]
        public void ExecuteCommands_WhenRoverFindsAnObstacle_TheRoverReportsAFailure(
            int start_x, int start_y, Direction direction, int obstacle_x, int obstacle_y, string cmd)
        {
            SetupObstacle(obstacle_x, obstacle_y);
            _plutoRover = new PlutoRover(start_x, start_y, direction, _defaultGridWidth, _defaultGridHeight, _mockObstaclesChecker.Object);

            var result = _plutoRover.ExecuteCommands(cmd);

            Assert.Equal(Status.Failure, result.Status);
        }

        private void SetupObstacle(int x, int y)
        {
            _mockObstaclesChecker = new Mock<ICheckObstacles>();
            _mockObstaclesChecker.Setup(c => c.ObstacleExist(It.IsNotIn(x), It.IsNotIn(y)))
                .Returns(false);
            _mockObstaclesChecker.Setup(c => c.ObstacleExist(It.IsIn(x), It.IsIn(y)))
                .Returns(true);
        }
    }
}
