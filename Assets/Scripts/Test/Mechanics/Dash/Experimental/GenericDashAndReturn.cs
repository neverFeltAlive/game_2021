/// <remarks>
/// 
/// GenericDashAndReturn is used for extending simple dash mechanics.
/// It adds a generic posibolity to return to the destinations of dash.
/// However it is an abstract generic class which requires to be inherited
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    public abstract class GenericDashAndReturn<T> : Dash
    /* DEBUG statements for this document 
     * 
     * Debug.Log("GenericDashAndReturn --> Start: ");
     * Debug.Log("<size=13><i><b> GenericDashAndReturn --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> GenericDashAndReturn --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> GenericDashAndReturn --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> GenericDashAndReturn --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        #region Protected Fields
        protected bool isDashReady;
        protected bool isReturnReady;

        protected T savedCoordinates;
        #endregion

        private bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>



        #region Functions
        protected abstract void OnSave();

        protected abstract void OnReturn();

        public override void HandleDash(Vector3 direction, float multiplier = 0f)
        {
            StartCoroutine(DashCoroutine(direction, multiplier));
        }

        public virtual void HandleReturn()
        {
            OnReturn();
        }
        #endregion



        #region Coroutines
        protected virtual IEnumerator DashCoroutine(Vector3 direction, float multiplier = 1f)
        {
            base.HandleDash(direction, multiplier);

            yield return new WaitForFixedUpdate();

            OnSave();

            // DEBUG
            if (showDebug)
                UtilsClass.DrawCross(transform.position, Color.yellow, 2f);
        }
        /// <remarks>
        /// Dash is implemented as a coroutine because of the need to save coordinates
        /// We cannot save target location before moving to that location as we need to check if there are any colliders on the way
        /// </remarks>
        #endregion
    }
}
