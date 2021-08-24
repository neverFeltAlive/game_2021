/// <remarks>
/// 
/// IMoving is used for creating objects that can move
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

namespace Custom.Mechanics
{
    public interface IMovement<TDir, TSpeed>
    {
        public Vector3 Direction { get; }

        public void Move(TDir direction, TSpeed speed);
    }
}
