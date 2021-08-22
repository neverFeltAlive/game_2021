/// <remarks>
/// 
/// EnemyMovementController is used for controlling movement logics of enemies AI
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Custom.Utils;
using Platformer.Mechanics.General;

namespace Platformer.Mechanics.EnemyAI
{
    public class EnemyMovementController : Mover
    /* DEBUG statements for this document 
     * 
     * Debug.Log("EnemyMovementController --> Start: ");
     * Debug.Log("<size=13><i><b> EnemyMovementController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> EnemyMovementController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> EnemyMovementController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> EnemyMovementController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public enum State
        {
            Roaming,
            Chase
        }



        #region Serialized Fields
        [SerializeField] protected EnemyFightController attack;
        [Space]
        [Header("AI Behaviour")]
        [SerializeField] [Tooltip("Distance to player to trigger attack")] private float triggerRange = 1f;
        [SerializeField] [Tooltip("Distance to player to continue chasing")] private float chaseRange = 5f;
        [SerializeField] [Range(0.5f, 2f)] [Tooltip("Maximum distance unit can walk in roaming state")] private float maxRoamingDistance = 2f;
        [SerializeField] [Range(0f, 2f)] [Tooltip("Minimum distance unit can walk in roaming state")] private float minRoamingDistance = 1f;
        [Space]
        [Header("Pathfinding")]
        [SerializeField] private int gridWidth = 20;
        [SerializeField] private int gridHeight = 20;
        [SerializeField] private float gridSellSize = 0.5f;
        [SerializeField] private Vector3 gridOriginPosition = new Vector3(-5, -5);
        /// <remarks>
        /// Max and min roaming ranges should be withing the boundaries of pathfinding grid
        /// which are defined by the grid's origin position and its width and height
        /// </remarks>
        #endregion

        #region Private Fields
        private bool isChasing;

        private int currentPathVectorIndex;

        private State currentState;

        private Vector3 roamingPosition;
        private Vector3 startingPosition;

        private Transform characterTransform;

        private List<Vector3> pathVectorsList;
        #endregion



        #region MonoBehaviour Callbacks
        protected override void Start()
        {
            base.Start();

            // Instantiate pathfinding if needed
            Pathfinding pathfinding;
            if (Pathfinding.Instance == null)
                pathfinding = new Pathfinding(gridWidth, gridHeight, gridOriginPosition, gridSellSize);

            characterTransform = GameObject.Find(Constants.CHARACTER_NAME).transform;

            currentState = State.Roaming;
            startingPosition = transform.position;
            roamingPosition = GetRoamingPosition();
            
            CalculatePath(roamingPosition);
        }

        private void Update()
        {
            CheckState();

            if (currentState == State.Roaming)
            {
                Move();

                // Check if reached position or path is null
                float reachedPositionDistance = .1f;
                if (Vector3.Distance(transform.position, roamingPosition) < reachedPositionDistance || pathVectorsList == null)
                {
                    roamingPosition = GetRoamingPosition();
                    CalculatePath(roamingPosition);
                    Move();
                }
            }
            else
            {
                CalculatePath(characterTransform.position);
                Move();
            }

        }
        #endregion

        #region Private Functions
        protected override void Move()
        {
            if (pathVectorsList != null)
            {
                Vector3 targetPosition = pathVectorsList[currentPathVectorIndex];
                if (Vector3.Distance(transform.position, targetPosition) > .09f)
                {
                    // DEBUG 
                    Debug.Log("<size=13><i><b> EnemyMovementController --> </b></i><color=green> Function: </color></size>" + targetPosition);

                    Vector2 direction = (targetPosition - transform.position).normalized;
                    body.MovePosition(body.position + direction * currentSpeed * Time.deltaTime);
                }
                else
                {
                    currentPathVectorIndex++;
                    if (currentPathVectorIndex >= pathVectorsList.Count)
                        pathVectorsList = null;
                }
            }
        }

        private void CalculatePath(Vector3 targetPosition)
        {
            currentPathVectorIndex = 0;
            pathVectorsList = Pathfinding.Instance.FindPath(transform.position, targetPosition);
        }

        // Checks if player is within the triggering range
        private void CheckState()
        {
            if (Vector3.Distance(characterTransform.position, transform.position) < triggerRange)
                currentState = State.Chase;
            else
                currentState = State.Roaming;
        }

        #endregion

        private Vector3 GetRoamingPosition()
        {
            return
                startingPosition +
                new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized *
                Random.Range(minRoamingDistance, maxRoamingDistance);
        }
        /// <summary>
        /// This function generates random normalized direction and multiply it by random distance and add it to the starting position
        /// </summary>
    }
}

/*
            // Check for collisions with player
            Collider2D[] collisions = Physics2D.OverlapCircleAll(gameObject.transform.position, attack.AttackToggleRange);
            foreach (Collider2D collision in collisions)
            {
                if (collision.name == Constants.CHARACTER_NAME)
                {
                    // Refresh direction to stop animation
                    Move(Vector2.zero);

                    // ToggleAttack
                    attack.ToggleAttack = true;
                    return;
                }
                if (collision.tag == Constants.ENEMY_TAG && collision.name != gameObject.transform.name)
                    GameObject.Destroy(gameObject);
            }

            // Check if player is in minRange
            if (Vector3.Distance(gameObject.transform.position, characterTransform.position) <= chaseRange)
            {
                // Trigger chasing
                if (Vector3.Distance(gameObject.transform.position, characterTransform.position) <= triggerRange)
                    isChasing = true;

                if (isChasing) 
                { 
                    // Move towards player
                    Move((characterTransform.position - gameObject.transform.position).normalized);
                }
            }
            else
            {
                if (Vector3.Distance(gameObject.transform.position, startingPosition) <= 0.01f)
                    // Move back to the start point
                    Move(startingPosition - gameObject.transform.position);
                else
                    Move(Vector2.zero);

                isChasing = false;
            }
*/
