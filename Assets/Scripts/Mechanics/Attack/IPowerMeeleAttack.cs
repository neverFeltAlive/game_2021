using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// IPowerMeeleAttack is used for implementing power attack mechanics
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public interface IPowerMeeleAttack : IMeeleAttack
    {
        public void TriggerAttack(Vector3 direction, bool isPower);
    }
}
