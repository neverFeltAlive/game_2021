/// <remarks>
/// 
/// IAttacking is used for [short description]
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    public interface IAttacking
    {
        public Vector3 Direction { get; }



        public void TriggerAttack();
        public void HandleAttack(Vector3 direction, Damage damage, float range);
    }
}
