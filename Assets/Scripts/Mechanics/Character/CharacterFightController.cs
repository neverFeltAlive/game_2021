/// <remarks>
/// 
/// CharacterFightController is used for [short description]
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
        #region Priavte Fields
        private bool overLoad = true;

        private Vector3 direction;
        #endregion



        #region Contex Menu
        [ContextMenu("Default Values")]
        private void DefaultValues()
        {
            maxHitPoints = 5;
            aimPunchResistance = 1.5f;

            attackRange = .2f;
            attackDashRange = .2f;

            damage = new Damage(amount: 1,  punch: .25f, instant: true);
            powerAttackDamage = new Damage(amount: 2,  punch: 1f, instant: true);
        }
        #endregion

        #region MonoBehaviour Callbacks
        protected sealed override void Start()
        {
            base.Start();

            targetTag = Constants.ENEMY_TAG;

            CharacterMovementController.OnOverLoadStateChange += OverLoadHandler;

            overLoad = false;
        }

        protected sealed override void Update()
        {
            base.Update();

            Vector2 input = CharacterMovementController.playerControls.MainControls.Walk.ReadValue<Vector2>();
            if (input != Vector2.zero)
                direction = input;
        }
        #endregion

        public void TriggerAttack(InputAction.CallbackContext context)
        {
            if (context.interaction is HoldInteraction)
            {
                if (context.performed)
                    StartCoroutine(PowerAttack(direction));
            }
            else
            {
                // Check interaction to destinquish power and simple attack
                if (overLoad)
                    StartCoroutine(PowerAttack(direction));
                else
                    HandleAttack(direction);
            }
        }

        private void OverLoadHandler(object sender, CharacterMovementController.OnOverLoadStateChangeEventArgs args)
        {
            if (args.state == CharacterMovementController.OverLoadState.Active)
                overLoad = true;
            else
                overLoad = false;
        }
    }
}
