using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //References
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private ParticleSystem muzzleFlash;
    private PlayerAbilities playerAbilitiesScript;
    public GameObject player;
    private Katana katanaScript;

    //AI
    public NavMeshAgent agent;
    public Transform playerTransform;
    public LayerMask groundLayer, playerLayer;
    [SerializeField] private Transform mechUpperBody;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Tracking
    Vector3 oldPos;
    Vector3 curPos;

    //State Bools for Referencing in Animations
    public bool chasing, attacking;



    void Awake() 
    {
        player = GameObject.FindWithTag("Player");
        playerAbilitiesScript = player.GetComponent<PlayerAbilities>();
        playerTransform = player.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();

        oldPos = playerTransform.position;
    }


    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange) 
        {
            Patrolling();
            Debug.Log("Patrolling");
            chasing = true;
            attacking = false;
        }

        if (playerInSightRange && !playerInAttackRange) 
        {
            ChasePlayer();
            chasing = true;
            Debug.Log("Chasing");
            attacking = false;
        }

        if (playerInSightRange && playerInAttackRange) 
        {
            AttackPlayer();
            chasing = false;
            Debug.Log("not chasing");
            attacking = true;
        }
    }



    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint Reached
        if (distanceToWalkPoint.magnitude < 0.5f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Raycast to navigate on / between layers
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true; 
        }
    }

    //Sets destination to position of player
    private void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        //mechUpperBody.LookAt(oldPos);
        //transform.LookAt(oldPos);

        

        if (!alreadyAttacked)
        {

            //Faces enemy towards player
            Vector3 forwardVector = Vector3.forward;
            float angle = Random.Range(0f, 360f);
            forwardVector = Quaternion.AngleAxis(enemyData.deviation, Vector3.up) * forwardVector;
            forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
            forwardVector = transform.rotation * forwardVector;

            //Attacking Logic
            //muzzleFlash.Play();            

            if (Physics.Raycast(transform.position, forwardVector, out RaycastHit hitInfo))
            {
                playerAbilitiesScript.TakeDamage(enemyData.damage);
            }


            alreadyAttacked = true;

            //Fire Rate & Atk Rate
            Invoke(nameof(ResetAttack), enemyData.timeBetweenAttacks);
        }

        StartCoroutine(LookDelay());
    }



    private void ResetAttack()
    {
        alreadyAttacked = false;
    }



    private IEnumerator LookDelay()
    {
        curPos = playerTransform.position;

        yield return new WaitForSeconds(0.25f);

        oldPos = curPos;

    }
}
