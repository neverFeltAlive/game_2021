/// <remarks>
/// 
/// IMeeleAttack is used for creating objects that can attack and deal damage to other objects
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

namespace Custom.Mechanics
{
    public interface IMeeleAttack<TDmg, TRange>
    {
        public Vector3 Direction { get; }



        public void TriggerAttack();
        public void HandleAttack(Vector3 direction, TDmg damage, TRange range);
    }
}
