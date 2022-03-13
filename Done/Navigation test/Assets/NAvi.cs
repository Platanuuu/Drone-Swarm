using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NAvi : MonoBehaviour
{
    public Transform target;
    public float speed;
    NavMeshAgent m_agent;
    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
            {
                m_agent=gameObject.GetComponent<NavMeshAgent>();
            }
  
            
    }

    // Update is called once per frame
    void Update()
    {
        m_agent.destination=target.transform.position;
       // m_agent.transform.position = Vector3.MoveTowards(this.transform.position,m_agent.nextPosition,speed * Time.deltaTime);
      //  transform.position = Vector3.MoveTowards(start.position, end.position, speed * Time.deltaTime);
    }
}
