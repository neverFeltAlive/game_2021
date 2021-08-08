/// <remarks>
/// 
/// Damagable is used as a basic data type for all objects which recieve damage
/// It provides simple damage recieving mechanics which are futher inherited by other 
/// classes to create destroyable objects like boxes and enemies.
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Utils;

namespace Platformer.Mechanics.General 
{
    public class Damagable : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Damagable --> Start: ");
     * Debug.Log("<size=13><i><b> Damagable --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Damagable --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Damagable --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Damagable --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        // Fields

        #region Serialized Fields
        [Space]
        [Space]
        [Header("Damagable Script")]
        [Space]
        [Header("Variables")]
        [SerializeField] [Range(1, 100)] [Tooltip("Maximum amount of health")] private int maxHitPoints = 1;
        #endregion

        #region Private Fields
        private int currentHitPoints;                   // current amount of health
        #endregion

        // Functions 

        #region MonoBehaviour Callbacks
        // When the script is enabled
        protected virtual void Start() =>
            // Assign variables
            currentHitPoints = maxHitPoints;

        protected virtual void Update()
        {
            // Check if object is dead
            if (currentHitPoints <= 0)
                Despawn();
        }
        #endregion

        #region Protected Functions
        // Destroy object 
        protected virtual void Despawn()
        {
            GameObject.Destroy(gameObject);
        }
        #endregion

        #region Event Handlers
        // When damage is taken
        protected virtual void OnDamageTaken(Damage damage)
        {
            currentHitPoints -= damage.AmountOfDamage;

            // DEBUG
            Debug.Log("<size=13><i><b> Damagable --> </b></i><color=green> OnDamageTaken: </color></size> " 
                + gameObject.name + " took " + damage.AmountOfDamage + " damage and currently has " + currentHitPoints + " hit points");
        }
        #endregion
    }
}
