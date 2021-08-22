/// <remarks>
/// 
/// IDisablableMovement is used for [short description]
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

namespace Custom.Mechanics
{
    public interface IDisablableMovement : IMoving
    {
        public Vector3 Direction { get; }



        public void DisableMovement();
        public void EnableMovement();
    }
}
