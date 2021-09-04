using System;
using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// IMeeleAttack is used for implementing basic meele attack mechanics
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public interface IMeeleAttack
    {
        public event EventHandler OnAttack;

        public void TriggerAttack(Vector3 direction);
    }
}
