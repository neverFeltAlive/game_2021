/// <remarks>
/// 
/// Enemy is used for creating enemies
/// It implements further differenciation from fighter class 
/// (fighter is also inherited by some character's mechanics).
/// This class should also be inherited by enemy species' classes
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [add custom usings if needed]

namespace Platformer.Mechanics.General
{
    public class Enemy : Fighter
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Enemy --> Start: ");
     * Debug.Log("<size=13><i><b> Enemy --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Enemy --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Enemy --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Enemy --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        // Fields

        #region Serialized Fields
        #endregion

        #region Private Fields
        #endregion

        #region Properties
        #endregion

        // Functions 

        #region MonoBehaviour Callbacks
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion
    }
}
