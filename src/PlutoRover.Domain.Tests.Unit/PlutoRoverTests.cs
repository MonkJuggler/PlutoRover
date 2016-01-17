using System;
using Xunit;

namespace PlutoRover.Domain.Tests.Unit
{
    public class PlutoRoverTests
    {
        private readonly PlutoRover _plutoRover;
        private Position _defaultPosition = new Position();

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
    }
}
