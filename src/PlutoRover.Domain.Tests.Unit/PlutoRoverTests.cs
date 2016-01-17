using System;
using System.Threading;
using Xunit;

namespace PlutoRover.Domain.Tests.Unit
{
    public class PlutoRoverTests
    {
        private PlutoRover _plutoRover;
        private Position _defaultPosition = new Position(0, 0, Direction.North);

        public PlutoRoverTests()
        {
            _plutoRover = new PlutoRover(_defaultPosition);
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
            Assert.Throws<ArgumentException>(() => _plutoRover.ExecuteCommands(cmd));
        }

        [Theory]
        [InlineData(1, 1, Direction.North, "F", 1, 2)]
        [InlineData(1, 1, Direction.East, "F", 2, 1)]
        [InlineData(1, 1, Direction.South, "F", 1, 0)]
        [InlineData(1, 1, Direction.West, "F", 0, 1)]
        public void ExecuteCommands_WhenMoveForwardCommand_TheRoverMovesForward(int start_x, int start_y, Direction direction, string cmd, int end_x, int end_y)
        {
            _plutoRover = new PlutoRover(start_x, start_y, direction);

            _plutoRover.ExecuteCommands(cmd);

            Assert.Equal(end_x, _plutoRover.Position.X);
            Assert.Equal(end_y, _plutoRover.Position.Y);
            Assert.Equal(direction, _plutoRover.Position.Direction);
        }
    }
}
