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

        #region Overrides 

        public static bool operator ==(WorldPosition left, WorldPosition right)
        {
            return left.Id == right.Id && left.Coordinate == right.Coordinate;
        }
        public static bool operator !=(WorldPosition left, WorldPosition right)
        {
            return left.Id != right.Id || left.Coordinate != right.Coordinate;
        }
        public override bool Equals(object obj)
        {
            if (obj is WorldPosition pos)
            {
                return Id == pos.Id && Coordinate == pos.Coordinate;
            }

            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public override string ToString()
        {
            return $"[{Id} / {Coordinate}]";
        }

        #endregion
    }
}
