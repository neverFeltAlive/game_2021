/// <remarks>
/// 
/// VectorDashAndReturn is used for [short description]
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [add custom usings if needed]

namespace Custom.Mechanics
{
    public class VectorDashAndReturn : GenericDashAndReturn<Vector3>
    /* DEBUG statements for this document 
     * 
     * Debug.Log("VectorDashAndReturn --> Start: ");
     * Debug.Log("<size=13><i><b> VectorDashAndReturn --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> VectorDashAndReturn --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> VectorDashAndReturn --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> VectorDashAndReturn --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        protected sealed override void OnReturn()
        {
            transform.position = savedCoordinates;
            savedCoordinates = default(Vector3);
        }

        protected sealed override void OnSave()
        {
            savedCoordinates = transform.position;
        }
    }
}
