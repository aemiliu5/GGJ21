using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Client : MonoBehaviour
{
    [Header("AI")]
    public GameObject targetCounter;
    public GameObject targetLeaving;
    public float stopDistance;

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

    private void Awake()
    {
        targetCounter = GameObject.Find("AI Target - Counter");
        targetLeaving = GameObject.Find("AI Target - Leaving");
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.destination = targetCounter.transform.position;
        currentState = State.GoingToCounter;
    }

    // Update is called once per frame
    void Update()
    {
        // Animation & Navigation
        switch (currentState)
        {
            case State.GoingToCounter:
                if (nav.remainingDistance > stopDistance)
                {
                    nav.destination = targetCounter.transform.position;
                }
                else
                {
                    currentState = State.AtCounter;
                }

                //Debug.Log(nav.remainingDistance);
                break;

            case State.AtCounter:
                nav.isStopped = true;
                anim.SetBool("walking", false);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 270, 0), 0.05f);
                
                break;
            
            case State.LeavingHappy:

                if (anim.GetBool("walking") == false)
                {
                    anim.SetBool("win", true);
                }
                
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(("Walking")))
                {
                    anim.SetBool("walking", true);
                    nav.destination = targetLeaving.transform.position;
                    nav.isStopped = false;
                }
                break;
            
            case State.LeavingAngry:

                if (anim.GetBool("walking") == false)
                {
                    anim.SetBool("lose", true);
                }
                
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(("Walking")))
                {
                    anim.SetBool("walking", true);
                    nav.destination = targetLeaving.transform.position;
                    nav.isStopped = false;
                }
                
                break;
        }
    }
}
