/// <remarks>
/// 
/// IDisablableMovement is used for implementing a possibility to disable default input in the movement mechinics
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

namespace Custom.Mechanics
{
    public interface IDisablableMovement<TDir, TSpeed> : IMovement<TDir, TSpeed>
    {
        public void DisableMovement();
        public void EnableMovement();

        public bool IsMoving();
    }
}
