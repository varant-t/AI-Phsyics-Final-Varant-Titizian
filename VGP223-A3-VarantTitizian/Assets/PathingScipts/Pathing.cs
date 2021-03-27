using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pathing : MonoBehaviour 
{
    [SerializeField]
    private Path path;
    [SerializeField]
    private float speed = 20.0f;
    [SerializeField]
    private float mass = 5.0f;
    [SerializeField]
    private bool isLooping = true;
    
    private float currentSpeed;
    private int currentPathIndex = 0;
    private Vector3 targetPoint;
    private Vector3 direction;
    private Vector3 targetDirection;


    public Rigidbody rb;
    public Transform target;
    public bool notChasing;
    public float lookRadius = 3f;

    public Animator anim;

 

    KinematicInput player;
	private void Start () 
    {
        // Initialize the direction as the agent's current facing direction
        direction = transform.forward; 
        // We get the firt point along the path
        targetPoint = path.GetPoint(currentPathIndex);
        notChasing = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        

       
	}
	
	private void Update ()
    {
      
       
        
    }

    private void FixedUpdate()
    {
        // patrols when the playeer is not in reach
        if (notChasing)
        {
            anim.SetTrigger("Patroling");
            if (path == null)
            {
                return;
            }

            currentSpeed = speed * Time.deltaTime;

            if (TargetReached())
            {
                if (!SetNextTarget())
                {
                    return;
                }
            }

            direction += Steer(targetPoint);
            transform.position += direction; //Move the agent according to the direction
            transform.rotation = Quaternion.LookRotation(direction); //Rotate the agent towards the desired direction
        }

        float distance = Vector3.Distance(target.position, transform.position);
         


        if (distance <= lookRadius)
        {
            anim.ResetTrigger("Patroling");
            anim.SetTrigger("Chasing");
            notChasing = false;
            AttackPlayer(target, 3);

        }

        if (distance >= lookRadius)
        {
            anim.ResetTrigger("Chasing");
            notChasing = true;
        }
    }


    /*
     * Attempt to set the next target point. If there are enough points available,
     * we simply increment the count. If we're out of points we have two choices:
     * if the isLooping bool is true, we go back to the first point in the path, otherwise,
     * we return false, indicating that there are no more points to visit. */
    private bool SetNextTarget() 
    {
        bool success = false;
        if (currentPathIndex < path.PathLength - 1) {
            currentPathIndex++;
            success = true;
        } 
        else 
        {
            if(isLooping) 
            {
                currentPathIndex = 0;
                success = true;
            } 
            else 
            {
                success = false;
            }
        }
        targetPoint = path.GetPoint(currentPathIndex);
        return success;
    }

    /* We use the path's tolerence radius to determine if the agent is "close enough"
     * to the target point to consider it "reached" */
    private bool TargetReached() 
    {
        return (Vector3.Distance(transform.position, targetPoint) < path.radius);
    }

    /* Steering algorithm to steer the agent towards the target vector */
    public Vector3 Steer(Vector3 target)
    {
        // Subtracting vector b - a gives you the direction from a to b. 
        targetDirection = (target - transform.position);
        targetDirection.Normalize();        
        targetDirection*= currentSpeed;
		
        Vector3 steeringForce = targetDirection - direction; 
        Vector3 acceleration = steeringForce / mass;

        return acceleration;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    // Function to chase the player, finding their postion, and using speed to follow through
    public void AttackPlayer(Transform targetObject, float maxSpeed)
    {

        Vector3 direction = targetObject.position - transform.position;
        direction.Normalize();
        transform.LookAt(new Vector3(targetObject.position.x, 0, targetObject.position.z));
        transform.position = transform.position + (direction * maxSpeed * Time.fixedDeltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.TakeDamage(1);
        }
    }

    public void expode()
    {
        
    }




}