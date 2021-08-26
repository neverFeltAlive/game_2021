using System;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// Health is used for simple health mechanics (take damage, despawn if 0 hp)
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Health : MonoBehaviour, IDamagable<Damage> 
    {
        public static event EventHandler OnDeath;
        public static event EventHandler<OnDamageTakenEventArgs> OnDamageTaken;
        public class OnDamageTakenEventArgs : EventArgs
        {
            public Damage damage;
            public int currentHitPoints;
        }



        [SerializeField] protected int maxHitPoints = 3;

        protected int currentHitPoints;



        protected virtual void Awake() =>
            currentHitPoints = maxHitPoints;


        protected virtual void Update()
        {
            if (currentHitPoints <= 0)
            {
                OnDeath?.Invoke(this, EventArgs.Empty);
                Despawn();
            }
        }

        protected virtual void Despawn()
        {
            Destroy(gameObject);
        }

        public virtual void TakeDamage(Damage damage)
        {
            currentHitPoints -= damage.amountOfDamage;
            OnDamageTaken?.Invoke(this, new OnDamageTakenEventArgs { damage = damage, currentHitPoints = currentHitPoints });
        }
    }
}
