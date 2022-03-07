using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NAvi : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
            this.gameObject.GetComponent<NavMeshAgent>().destination=target.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
