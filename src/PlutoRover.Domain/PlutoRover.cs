using System;
using System.Text.RegularExpressions;

namespace PlutoRover.Domain
{
    public class PlutoRover : IRover
    {
        private readonly Position _position;
        private readonly ICheckObstacles _obstaclesChecker;
        private readonly int _gridWidth;
        private readonly int _gridHeight;

        public PlutoRover(Position position, int gridWidth, int gridHeight, ICheckObstacles obstaclesChecker)
        {
            _position = position;
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
            _obstaclesChecker = obstaclesChecker;
        }

        public PlutoRover(int x, int y, Direction direction, int gridWidth, int gridHeight, ICheckObstacles obstaclesChecker)
        {
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
            _obstaclesChecker = obstaclesChecker;
            _position = new Position(x, y, direction);
        }

        public Position Position => _position;

        public ExecutionResult ExecuteCommands(string cmd)
        {
            var isValidCommand = ValidateCommand(cmd);

            if (!isValidCommand)
            {
                return new ExecutionResult(Status.Failure, $"Command [{cmd}] contains invalid characters");
            }

            foreach (var singleCommand in cmd)
            {
                bool success = true;
                switch (singleCommand)
                {
                    case ('F'):
                    {
                        success = MoveForward();
                        break;
                    }
                    case ('B'):
                    {
                        success = MoveBackward();
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
                        return new ExecutionResult(Status.Failure, $"Command [{singleCommand}] not recognized");
                    }
                }

                if (!success)
                {
                    return new ExecutionResult(Status.Failure, "An obstable was found on the Rover path");
                }
            }

            return new ExecutionResult(Status.Success, "Commands successfully executed");
        }

        private bool ValidateCommand(string cmd)
        {
            var match = Regex.Match(cmd, @"^[FBLR]+$");
            return match.Success;
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

        private bool MoveForward()
        {
            return Move(Way.Forward);
        }

        private bool MoveBackward()
        {
            return Move(Way.Backward);
        }

        private bool Move(Way way)
        {
            var coefficient = way == Way.Forward ? 1 : -1;
            var newX = _position.X;
            var newY = _position.Y;

            switch (_position.Direction)
            {
                case (Direction.North):
                {
                    newY = WrapAround(_position.Y + coefficient, _gridHeight);
                    break;
                }
                case (Direction.East):
                {
                    newX = WrapAround(_position.X + coefficient, _gridWidth);
                    break;
                }
                case (Direction.South):
                {
                    newY = WrapAround(_position.Y - coefficient, _gridHeight);
                    break;
                }
                case (Direction.West):
                {
                    newX = WrapAround(_position.X - coefficient, _gridWidth);
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }

            if (_obstaclesChecker.ObstacleExist(newX, newY))
            {
                return false;
            }
            _position.X = newX;
            _position.Y = newY;
            return true;
        }

        private int WrapAround(int n, int mod)
        {
            return (n + mod) % mod;
        }
    }
}
