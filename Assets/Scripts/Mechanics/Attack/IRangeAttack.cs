using System;
using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// IRangeAttack is used for implementing shoot mechanics
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public interface IRangeAttack 
    {
        public event EventHandler OnShoot;

        public void Shoot(Vector3 direction);
    }
}
