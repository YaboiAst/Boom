using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float amount){
        Debug.Log("Tomando " + amount + " de dano");
    }

    private void Update()
    {
        agent.SetDestination(player.position);
    }
}
