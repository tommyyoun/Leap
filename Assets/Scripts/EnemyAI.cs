using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{
    public float pushForce = 10f; // Adjust this value to control the force of the push.
    private Transform player;
    private Rigidbody enemyRigidbody;
  


    private Vector3 originalPosition;
    private Vector3 walkPoint;
    public float walkPointRange;
    bool walkPointSet; 

    public NavMeshAgent agent;
    public LayerMask IsGround, IsPlayer;

    public float timeBetweenpush;
    bool alreadypushed;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool isReturning = false;
    public Animator snake; 


    public Vector3 directionToPlayer;
    private void Start()
    {
        player = GameObject.Find("Frog").transform;
        enemyRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;     
    }

    private void Update()
    {


        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        Vector3 currPos = transform.position; 
        
        if (!playerInSightRange && !playerInAttackRange)
        {
            if ( currPos == originalPosition) 
            {
                Patroling();
                
            }
            else if(currPos == walkPoint)
            {
                ReturnToOrigin(originalPosition);
            }
            else
            {
                Patroling(); 
            }
           
        } 

        if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer(); 
        }

        if (playerInAttackRange && playerInSightRange)
        {

            PushPlayer();
        }


      
    } 

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            snake.SetTrigger("Walk");
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint; 

        if(distanceToWalkPoint.magnitude < 1f )
        {
            walkPointSet = false;
        }

    }

    private void SearchWalkPoint()
    {
        float randZ = Random.Range(-walkPointRange, walkPointRange);
        float randX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ); 

        if(Physics.Raycast(walkPoint, -transform.up, 2f , IsGround)) 
        { 

            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position); 
    }

    private void PushPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadypushed)
        {
            snake.SetTrigger("Attack");
            enemyRigidbody.AddForce(transform.forward * pushForce, ForceMode.Force);
            alreadypushed = true;
            Invoke(nameof(ResetAttack), timeBetweenpush);
        }

    }
    private void ResetAttack()
     {
       alreadypushed = false;
     }
    
    private void ReturnToOrigin(Vector3 origin) 
    {
        agent.SetDestination(originalPosition);
    }
}

