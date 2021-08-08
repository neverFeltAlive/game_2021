/// <remarks>
/// 
/// DataTypes is used for storing custom data types usefull throughout the project
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [add custom usings if needed]

namespace Platformer.Utils
{
    // Classes

    // Frequently used values 
    public static class Constants
    {
        // Object identifiers 
        public const string CHARACTER_NAME = "Character";
        public const string ENEMY_TAG = "Enemy";
        public const string FRIENDLY_TAG = "Friendly";

        // Animation parameters
        public const string ATTACK = "Attack";                                                          // triggers attack
        public const string HORIZONTAL = "Horizontal";                                                  // holds name of animator parameter
        public const string VERTICAL = "Vertical";                                                      // holds name of animator parameter
        public const string MAGNITUDE = "Magnitude";                                                    // holds name of animator parameter

        // Controls
        public const string MOVEMENT_X = "Horizontal";
        public const string MOVEMENT_Y = "Vertical";

        public const string MOVEMENT_JOYSTICK_X = "Left Joystick Horizontal";
        public const string MOVEMENT_JOYSTICK_Y = "Left Joystick Vertical";
        public const string AIM_JOYSTICK_X = "Right Joystick Horizontal";
        public const string AIM_JOYSTICK_Y = "Right Joystick Vertical";
        public const string A_BUTTON = "A Button";                         
        public const string X_BUTTON = "X Button";                          
        public const string Y_BUTTON = "Y Button";                             
        public const string B_BUTTON = "B Button";                        
        /// <summary>
        /// All input settengs are done with Unity Input Manager. 
        /// This script binds axes names with buttons to avoid typoes 
        /// Axes use sepparate input manager elements for PC and joysticks while buttons use the same.
        /// Buttons are named according to Xbox 360 controller.
        /// </summary>

        /// <remarks> Controls convertion table
        /// 
        /// Square button on PS          /       X button on Xbox
        /// Round button on PS           /       Y button on Xbox
        /// X button on PS               /       A button on Xbox
        /// Triangular button on PS      /       B button on Xbox
        /// 
        /// </remarks>
    }

    // Damage data type
    [System.Serializable]
    public class Damage
    {
        #region Serialized Fields
        [Header("Damage Stats")]
        [SerializeField] [Range(1, 20)] [Tooltip("Weapons damage in hit points")] private int _amountOfDamage = 1;
        [SerializeField] [Range(1f, 30f)] [Tooltip("Push force of a weapon")] private float _aimPunch = 1f;
        [Space]
        [SerializeField] [Tooltip("Damage type")] private bool _isInstant = true;
        #endregion

        #region Private Fields
        private Vector3 _orrigin;                // save where damage came from
        #endregion

        #region Properties
        public bool IsInstant { get { return _isInstant; } }
        public int AmountOfDamage { get { return _amountOfDamage; } }
        public float AimPunch { get { return _aimPunch; } }
        public Vector3 Orrigin { get { return _orrigin; } }
        #endregion

        // Cunstructors 

        public Damage(bool instant = true, int amount = 1, float punch = 1f)
        {
            this._aimPunch = punch;
            this._amountOfDamage = amount;
            this._isInstant = instant;
        }
        /// <summary>
        /// We basically need a constructor only to assign values in context menu functions outside this script
        /// It is the easiest way to achieve that without changing accessibility modifiers and adding set properties
        /// </summary>
    }
}
