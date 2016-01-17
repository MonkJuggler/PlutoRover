using System;
using System.Text.RegularExpressions;

namespace PlutoRover.Domain
{
    public class PlutoRover : IRover
    {
        private readonly Position _position;
        private readonly int _gridWidth;
        private readonly int _gridHeight;

        public PlutoRover(Position position, int gridWidth, int gridHeight)
        {
            _position = position;
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
        }

        public PlutoRover(int x, int y, Direction direction, int gridWidth, int gridHeight)
        {
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
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
                    case ('R'):
                    {
                        RotateRight();
                        break;
                    }
                    case ('L'):
                    {
                        RotateLeft();
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

        private void RotateRight()
        {
            switch (_position.Direction)
            {
                case (Direction.North):
                {
                    _position.Direction = Direction.East;
                    break;
                }
                case (Direction.East):
                {
                    _position.Direction = Direction.South;
                    break;
                }
                case (Direction.South):
                {
                    _position.Direction = Direction.West;
                    break;
                }
                case (Direction.West):
                {
                    _position.Direction = Direction.North;
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void RotateLeft()
        {
            switch (_position.Direction)
            {
                case (Direction.North):
                    {
                        _position.Direction = Direction.West;
                        break;
                    }
                case (Direction.West):
                    {
                        _position.Direction = Direction.South;
                        break;
                    }
                case (Direction.South):
                    {
                        _position.Direction = Direction.East;
                        break;
                    }
                case (Direction.East):
                    {
                        _position.Direction = Direction.North;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
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
