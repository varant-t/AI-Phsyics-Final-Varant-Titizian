using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NewAi : MonoBehaviour
{
    public float lookRadius = 8f;
    public float interactRadius = 4f;
    public Transform target;
    NavMeshAgent agent;
    KinematicInput player;

    
    
    // Start is called before the first frame update
    void Start()
    {

        GameObject person = GameObject.Find("Player");
        player = person.GetComponent<KinematicInput>();

        player = GetComponent<KinematicInput>();
        agent = GetComponent<NavMeshAgent>();
        //target = PlayerManager.instance.player.transform;
        //person = GameObject.FindGameObjectWithTag("Player");
    }
     
    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if(distance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
        }

        if(distance <= interactRadius)
        {
            Invoke("explode", 3);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
            
    }

    void explode()
    {
        
        Debug.Log("Explode");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

  
}
