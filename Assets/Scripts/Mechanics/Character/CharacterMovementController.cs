/// <remarks>
/// 
/// CharacterMovementController is used for controlling target's core movement
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Platformer.Utils;
using Platformer.Mechanics.General;

namespace Platformer.Mechanics.Character
{
    public class CharacterMovementController : Mover
    /* DEBUG statements for this document 
     * 
     * Debug.Log("CharacterMovementController --> Start: ");
     * Debug.Log("<size=13><i><b> CharacterMovementController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterMovementController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterMovementController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterMovementController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        // Functions 

        #region MonoBehaviour Callbacks
        protected sealed override void Start()=>
            // Call base class 
            base.Start();

        private void Update()
        {
            // Define movement _direction based on user input
            float horizontalDirection = Input.GetAxis(Constants.MOVEMENT_JOYSTICK_X);
            float vertivalDirection = Input.GetAxis(Constants.MOVEMENT_JOYSTICK_Y);
            Direction = new Vector2(horizontalDirection, vertivalDirection);
        }

        protected void FixedUpdate() =>
            Move(Direction);

        #endregion

        /*   // Fields

           #region Serialized Fields
           [Header("Components")]
           [SerializeField] private Rigidbody2D characterBody;
           [Space]

           [Header("Variables")]
           [SerializeField] [Range(0f, 5f)] [Tooltip("Experiment with speed values")] private float _movementSpeed = 1f;
           [Space]

           [Header("Controls")]
           [SerializeField] private ControlKeys moveKey = ControlKeys.M;               // define move key
           #endregion

           #region Private Fields
           private Vector2 _direction;                                                  // define the _direction for future movement

           private string moveKeyStr;                                                  // contain move key string
           private bool triggerMovement = false;                                       // trigger movement
           #endregion

           #region Properties
           public Vector2 Direction { get { return _direction; } }
           public float currentSpeed { get { return _movementSpeed; } set { _movementSpeed = value; } }
           #endregion

           // Functions 

           #region MonoBehaviour Callbacks
           private void Start()
           {
               // Manage keys
               moveKeyStr = CommonMethods.SwitchKeys(moveKey);

               // Check if all required elements are assigned
               if (!characterBody)
                   characterBody = gameObject.GetComponent<Rigidbody2D>();
           }

           private void Update()
           {
               // Define movement _direction based on user input
               float horizontalDirection = Input.GetAxis(Constants.MOVEMENT_X);
               float vertivalDirection = Input.GetAxis(Constants.MOVEMENT_Y);
               if (horizontalDirection != 0f || vertivalDirection != 0f)                             // check if _direction was changed
                   _direction = new Vector2(horizontalDirection, vertivalDirection);

               // Prevent continious movement
               if (!Input.GetKey(moveKeyStr))
                   triggerMovement = false;

               // Trigger movement
               if (Input.GetKeyDown(moveKeyStr))
                   triggerMovement = true;

               // Stop movement
               if (Input.GetKeyUp(moveKeyStr))
                   triggerMovement = false;
           }

           private void FixedUpdate()
           {
               // Swap target's sprite to match movement _direction
               if (_direction.x >= 0)
                   gameObject.transform.localScale = Vector3.one;
               else
                   gameObject.transform.localScale = new Vector3(-1, 1, 1);

               // Move RigitBody
               if (triggerMovement)
                   characterBody.MovePosition(characterBody.position + _direction * _movementSpeed * Time.fixedDeltaTime);
           }

           #endregion

           #region Private Functions
           #endregion

           // Methods

           #region Private Methods
           #endregion*/

    }
}
