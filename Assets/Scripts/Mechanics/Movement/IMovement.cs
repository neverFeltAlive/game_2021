using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// IMoving is used for creating objects that can move
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public interface IMovement
    {
        public Vector3 Direction { get; set; }

        public void Move(Vector3 direction, float speed);
    }
}
