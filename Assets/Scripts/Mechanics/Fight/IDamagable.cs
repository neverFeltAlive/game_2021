/// <remarks>
/// 
/// IDamagable is used for identifying objects that can be damaged
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    public interface IDamagable
    {
        public void TakeDamage(Damage damage);
    }
}
