/// <remarks>
/// 
/// Health is used for simple health mechanics (take damage, despawn if 0 hp)
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class Health : MonoBehaviour, IDamagable 
        /* DEBUG statements for this document 
         * 
         * Debug.Log("Health --> Start: ");
         * Debug.Log("<size=13><i><b> Health --> </b></i><color=yellow> FixedUpdate: </color></size>");
         * Debug.Log("<size=13><i><b> Health --> </b></i><color=red> Update: </color></size>");
         * Debug.Log("<size=13><i><b> Health --> </b></i><color=blue> Corutine: </color></size>");
         * Debug.Log("<size=13><i><b> Health --> </b></i><color=green> Function: </color></size>");
         * 
         */
        /* TODO
         * 
         */
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

        protected bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>



        protected virtual void Awake() =>
            currentHitPoints = maxHitPoints;

        protected virtual void Start()
        {
            OnDeath += DeathHandler;
            OnDamageTaken += DamageHandler;
        }

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

        // DEBUG
        private void DeathHandler(object sender, EventArgs args)
        {
            if (showDebug)
                Debug.Log("<size=13><i><b> Health --> </b></i><color=green> DeathHandler: </color></size>" + sender + " died");
        }
        private void DamageHandler(object sender, OnDamageTakenEventArgs args)
        {
            if (showDebug)
                Debug.Log("<size=13><i><b> Health --> </b></i><color=green> DeathHandler: </color></size>" + 
                    sender + " took " + args.damage.amountOfDamage + " damage and currently has " + 
                    args.currentHitPoints + " hp");
        }
    }
}
