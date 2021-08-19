/// <remarks>
/// 
/// DataTypes is used for storing custom data types usefull throughout the project
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

// [add custom usings if needed]

namespace Platformer.Utils
{
    // Classes

    // Frequently used values 
    public static class Constants
    {
        // Action maps
        public const string DEFAULT_MAP = "Main Controls";
        public const string SHOOTING_MAP = "Shooting Controls";

        // Object identifiers 
        public const string CHARACTER_NAME = "Character";
        public const string ENEMY_TAG = "Enemy";
        public const string FRIENDLY_TAG = "Friendly";

        // Animation parameters
        public const string ATTACK = "Attack";                                                          // triggers attack
        public const string ROLL = "Roll";                                                              // triggers roll
        public const string DASH = "Dash";                                                              // triggers dash
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
        [Range(1, 20)] [Tooltip("Weapons damage in hit points")] public int amountOfDamage = 1;
        [Range(0f, 5f)] [Tooltip("Push force of a weapon")] public float aimPunch = .25f;
        [Tooltip("Damage type")] public bool isInstant = true;
        #endregion

        [HideInInspector] public Vector3 orrigin;

        

        public Damage(bool instant, int amount, float punch)
        {
            this.aimPunch = punch;
            this.amountOfDamage = amount;
            this.isInstant = instant;
        }
        /// <summary>
        /// We basically need a constructor only to assign values in context menu functions outside this script
        /// It is the easiest way to achieve that without changing accessibility modifiers and adding set properties
        /// </summary>
    }
}
