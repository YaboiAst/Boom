using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Melee meleeScript;
    private float meleeCooldown = 1.5f;
    private float meleeCooldownCounter = 0f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        meleeScript = GetComponent<Melee>();
    }

    public void TakeDamage(float amount){
        Debug.Log("Tomando " + amount + " de dano");
    }

    private void Update()
    {
        agent.SetDestination(player.position);

        meleeCooldownCounter += Time.deltaTime;
        if(meleeCooldownCounter >= meleeCooldown && agent.remainingDistance < agent.stoppingDistance){
            meleeCooldownCounter = 0f;
            meleeScript.MeleeAtack();
        }
    }
}
