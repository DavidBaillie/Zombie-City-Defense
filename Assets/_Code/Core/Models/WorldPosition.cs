namespace Assets.Core.Models
{
    public struct WorldPosition
    {
        public System.Guid Id;
        public UnityEngine.Vector3 Coordinate;

        public WorldPosition(System.Guid Id, UnityEngine.Vector3 Coordinate)
        {
            this.Id = Id;
            this.Coordinate = Coordinate;
        }
    }
}
