namespace PlutoRover.Domain
{
    public class PlutoRover : IRover
    {
        private readonly Position _position;

        public PlutoRover(Position position)
        {
            _position = position;
        }

        public Position Position => _position;

        public void ExecuteCommands(string cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}
