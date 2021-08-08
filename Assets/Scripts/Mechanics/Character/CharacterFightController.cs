/// <remarks>
/// 
/// CharacterFightController is used for [short description]
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Mechanics.General;
using Platformer.Utils;

namespace Platformer.Mechanics.Character
{
    public class CharacterFightController : Fighter
    /* DEBUG statements for this document 
     * 
     * Debug.Log("CharacterFightController --> Start: ");
     * Debug.Log("<size=13><i><b> CharacterFightController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterFightController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterFightController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterFightController --> </b></i><color=green> Function: </color></size>");
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
        [Header("Mover Script")]
        [Space]
        [Header("Variables")]
        [SerializeField] [Range(0.1f, 5f)] [Tooltip("Time it takes to perform attack (needed for movement stopping)")] private float attackAnimationTime = 0.3f;
        #endregion

        // Functions 

        #region Context Menu
        #endregion

        #region MonoBehaviour Callbacks
        protected sealed override void Start()
        {
            base.Start();

            targetTag = Constants.ENEMY_TAG;
        }

        protected sealed override void Update()
        {
            base.Update();

            if (Input.GetButtonDown(Constants.A_BUTTON))
            {
                // Disable movement
                movement.StopMoving(attackAnimationTime);

                Attack();
            }
        }
        #endregion

        #region Event Handlers
        #endregion

        // Methods

        #region Private Methods
        #endregion

    }
}
