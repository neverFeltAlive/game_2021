/// <remarks>
/// 
/// IMoving is used for [short description]
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

namespace Custom.Mechanics
{
    public interface IMoving
    {
        public void Move(Vector3 direction, float speed);
    }
}
