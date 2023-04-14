using System;
using UnityEngine;

namespace Assets.Core.Models
{
    [System.Serializable]
    public struct WorldPosition
    {
        public Guid Id;
        public Vector3 Coordinate;

        public WorldPosition(Guid Id, Vector3 Coordinate)
        {
            this.Id = Id;
            this.Coordinate = Coordinate;
        }
    }
}
