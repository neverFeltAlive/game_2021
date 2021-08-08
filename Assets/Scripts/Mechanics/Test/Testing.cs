/// <remarks>
/// 
/// Testing is used for [short description]
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Mechanics.EnemyAI;
using Platformer.Utils;

namespace Platformer.Mechanics
{
    public class Testing : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Testing --> Start: ");
     * Debug.Log("<size=13><i><b> Testing --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Testing --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Testing --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Testing --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        private Pathfinding pathfinding;

        private void Start()
        {
            pathfinding = new Pathfinding(10, 10, Vector3.zero);

            //List<PathNode> path = pathfinding.FindPath(0, 0, Random.Range(0, 9), Random.Range(0, 9));
            List<Vector3> path = pathfinding.FindPath(new Vector3(0, 0), new Vector3(9, 9));
            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    // DEBUG
                    Debug.Log("<size=13><i><b> Testing --> </b></i><color=red> Update: </color></size>" + path[i]);
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y));
                }
            }
            else
                // DEBUG
                Debug.Log("<size=13><i><b> Testing --> </b></i><color=red> Update: </color></size>");
        }
    }
}
