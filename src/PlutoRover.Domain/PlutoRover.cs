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
                        return new ExecutionResult(Status.Failure, $"Command [{singleCommand}] not recognized");
                    }
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
                    _position.Y = WrapAround(_position.Y + coefficient, _gridHeight);
                    break;
                }
                case (Direction.East):
                {
                    _position.X = WrapAround(_position.X + coefficient, _gridWidth);
                    break;
                }
                case (Direction.South):
                {
                        _position.Y = WrapAround(_position.Y - coefficient, _gridHeight);
                        break;
                }
                case (Direction.West):
                {
                        _position.X = WrapAround(_position.X - coefficient, _gridWidth);
                        break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        private int WrapAround(int n, int mod)
        {
            return (n + mod) % mod;
        }
    }
}
