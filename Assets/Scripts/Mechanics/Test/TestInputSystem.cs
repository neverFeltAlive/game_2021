using UnityEngine;

namespace Platformer
{
    public class TestInputSystem : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("TestInputSystem --> Start: ");
     * Debug.Log("<size=13><i><b> TestInputSystem --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> TestInputSystem --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> TestInputSystem --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> TestInputSystem --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        Vector2[] testArray;
        public void Start()
        {
            testArray = new Vector2[3];

            Debug.Log(testArray[0] + "" +  testArray[1]);
            Debug.Log(default(Vector2));
        }
    }
}
