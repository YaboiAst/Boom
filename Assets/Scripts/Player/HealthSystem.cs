using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float shield = 50f;
    private float health = 100f;
    public float maxHealth = 100f;

    public void TakeDamage(float amount){
        if (shield > 0)
        {
            shield -= amount;
            if (shield < 0)
            {
                health += shield;
                shield = 0;
            }
        }
        else
        {
            if(health - amount < 0){
                health = 0;
            }
            else
                health -= amount;
        }
    }
}
