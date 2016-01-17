using System;
using System.Runtime.Remoting.Messaging;
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
            foreach (var singleCommand in cmd)
            {
                switch (singleCommand)
                {
                    case ('F'):
                    {
                        MoveForward();
                        break;
                    }
                    case ('B'):
                    {
                        MoveBackward();
                        break;
                    }
                    default:
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        private void ValidateCommand(string cmd)
        {
            var match = Regex.Match(cmd, @"^[FBLR]+$");

            if (!match.Success)
            {
                throw new ArgumentException("Invalid command", nameof(cmd));
            }
        }

        private void MoveForward()
        {
            Move(Way.Forward);
        }

        private void MoveBackward()
        {
            Move(Way.Backward);
        }

        private void Move(Way way)
        {
            var coefficient = way == Way.Forward ? 1 : -1;
            switch (_position.Direction)
            {
                case (Direction.North):
                {
                    _position.Y += coefficient;
                    break;
                }
                case (Direction.East):
                {
                    _position.X += coefficient;
                    break;
                }
                case (Direction.South):
                {
                    _position.Y -= coefficient;
                    break;
                }
                case (Direction.West):
                {
                    _position.X -= coefficient;
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
