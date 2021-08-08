/// <remarks>
/// 
/// Collidable is used as a basic class for all collidable objects in the game
/// This class is inherited by most interactible objects 
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [add custom usings if needed]

namespace Platformer.Mechanics.Objects
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Collidable : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Collidable --> Start: ");
     * Debug.Log("<size=13><i><b> Collidable --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Collidable --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Collidable --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Collidable --> </b></i><color=green> Function: </color></size>");
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
        [Header("Collidable Script")]
        [Space]
        [Header("Components")]
        [SerializeField] protected BoxCollider2D boxCollider;
        [SerializeField] private ContactFilter2D filter;                                  
        #endregion

        #region Private Fields
        private Collider2D[] collisions = new Collider2D[10];           // stores all collisioins
        #endregion

        // Functions 

        #region MonoBehaviour Callbacks
        // When the script is enabled
        protected virtual void Start()
        {
            // Assign values
            if (!boxCollider)
                boxCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        // Every frame
        protected virtual void Update()
        {
            // Check for collisions
            boxCollider.OverlapCollider(filter, collisions);

            for (int i = 0; i < collisions.Length ; i++)
            {
                // Skip null results
                if (collisions[i] == null)
                    continue;

                // Call collision handler
                OnCollision(collisions[i]);

                // Clean up the array (It is not gonna clean up automatically)
                collisions[i] = null;
            }
        }
        #endregion

        #region Protected Functions
        // Custom collision handler
        protected virtual void OnCollision(Collider2D collider) =>
            // DEBUG
            Debug.Log("<size=13><i><b> Collidable --> </b></i><color=green> OnCollision: </color></size> Collided with " + collider.name);
        #endregion
    }
}
