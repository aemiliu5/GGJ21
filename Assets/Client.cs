using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Client : MonoBehaviour
{
    public GameObject target;

    public enum State
    {
        GoingToCounter,
        AtCounter,
        LeavingHappy,
        LeavingAngry
    };

    public State currentState;
    private NavMeshAgent nav;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.destination = target.transform.position;
        currentState = State.GoingToCounter;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.GoingToCounter:
                if (nav.remainingDistance > 0.5f)
                {
                    nav.destination = target.transform.position;
                }
                else
                {
                    currentState = State.AtCounter;
                }

                break;
            
            case State.AtCounter:
                nav.isStopped = true;
                anim.SetBool("walking", false);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.05f);
                
                break;
                
            
            // [TODO]
            case State.LeavingAngry: break;
            case State.LeavingHappy: break;
        }
    }
}
