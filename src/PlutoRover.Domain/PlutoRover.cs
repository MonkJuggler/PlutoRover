using System;
using System.Text.RegularExpressions;

namespace PlutoRover.Domain
{
    public class PlutoRover : IRover
    {
        private readonly Position _position;

        public PlutoRover(Position position)
        {
            _position = position;
        }

        public PlutoRover(int x, int y, Direction direction)
        {
            _position = new Position(x, y, direction);
        }

        public Position Position => _position;

        public void ExecuteCommands(string cmd)
        {
            ValidateCommand(cmd);
        }

        private void ValidateCommand(string cmd)
        {
            var match = Regex.Match(cmd, @"^[FBLR]+$");

            if (!match.Success)
            {
                throw new ArgumentException("Invalid command", nameof(cmd));
            }
        }
    }
}
