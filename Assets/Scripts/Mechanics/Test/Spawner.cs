/// <remarks>
/// 
/// Spawner is used for testing mob spawning
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [add custom usings if needed]

namespace Platformer.Mechanics.Objects
{
    public class Spawner : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Spawner --> Start: ");
     * Debug.Log("<size=13><i><b> Spawner --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Spawner --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Spawner --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Spawner --> </b></i><color=green> Function: </color></size>");
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
        [Header("Spawner Script")]
        [Space]
        [Header("Links")]
        [SerializeField] [Tooltip("Object to spawn")] private GameObject obj;
        [Space]
        [Header("Variables")]
        [SerializeField] [Range(1f, 2f)] [Tooltip("Minimum delay between 2 spawns in seconds")] private float minDelay;
        [SerializeField] [Range(2f, 10f)] [Tooltip("Maximum delay between 2 spawns in seconds")] private float maxDelay;
        #endregion

        #region Private Fields
        private int index = 0;

        private bool isSpawning = false;
        #endregion

        // Functions 

        #region MonoBehaviour Callbacks
        private void Update()
        {
            if (!isSpawning)
                StartCoroutine(Spawn(Random.Range(minDelay, maxDelay)));

        }
        #endregion

        // IEnumerators

        #region Coroutines
        // Spawn objects with delay
        IEnumerator Spawn(float delay)
        {
            isSpawning = true;
            yield return new WaitForSeconds(delay);
            GameObject.Instantiate(obj);

            // Rename
            GameObject.Find("Enemy(Clone)").transform.name = index.ToString();
            index++;

            isSpawning = false;
        }
        #endregion
    }
}

/*

        // Fields

        #region Serialized Fields
        [Space]
        [Space]
        [Header("Spawner Script")]
        [Space]
        [Header("Links")]
        [SerializeField] [Tooltip("Object to spawn")] private GameObject obj;
        [SerializeField] [Tooltip("Locations of spawning")] private Transform[] locations;
        [Space]
        [Header("Variables")]
        [SerializeField] [Range(1f, 2f)] [Tooltip("Minimum delay between 2 spawns in seconds")] private float minDelay = 5f;
        [SerializeField] [Range(2f, 10f)] [Tooltip("Maximum delay between 2 spawns in seconds")] private float maxDelay = 10f;
        [SerializeField] [Range(2, 10)] [Tooltip("Maximum delay between 2 spawns in seconds")] private int maxQuantity = 5;
        #endregion

        #region Private Fields
        private GameObject[] objects;

        private int index = 0;
        
        private bool isSpawning = false;
        #endregion

        // Functions 

        #region MonoBehaviour Callbacks
        private void Start()
        {
            objects = new GameObject[maxQuantity];

            for (int i = 0; i < maxQuantity; i++)
                InstantiateObj(i);
        }

        private void Update()
        {
            for (int i = 0; i < maxQuantity; i++)
            {
                if (!isSpawning)
                {
                    if (objects[i] == null)
                        StartCoroutine(Spawn(Random.Range(minDelay, maxDelay) , i));
                }
            }
        }
        #endregion

        #region Private Functions
        private void InstantiateObj(int index)
        {
            objects[index] = GameObject.Instantiate(obj, locations[(int)Random.Range(0f, locations.Length - 1)]);

            // Rename
            GameObject.Find("Enemy(Clone)").transform.name = index.ToString();
            index++;
        }
        #endregion

        // IEnumerators

        #region Coroutines
        // Spawn objects with delay
        IEnumerator Spawn(float delay, int index)
        {
            isSpawning = true;
            yield return new WaitForSeconds(delay);

            InstantiateObj(index);

            isSpawning = false;
        }
        #endregion
*/
