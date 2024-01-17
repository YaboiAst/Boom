using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float shield = 50f;
    private float health = 100f;
    public float maxHealth = 100f;
    public float maxShield = 50f;
    private float healthRegenTimer = 2f;
    private float healthRegenCounter = 0f;
    private bool isOnCombat = false;
    private float shieldRegenSpeed = 30f;
    private float healthRegenSpeed = 10f;
    [Header("HUD")]
    [SerializeField] private HUDContoller hud;

    public float GetHealth(){return health;}
    public float GetShield(){return shield;}

    public void TakeDamage(float amount){
        healthRegenCounter = 0f;
        isOnCombat = true;
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

        hud.OnUpdateHUD?.Invoke();
    }

    private void Heal(float amount){
        if(health == maxHealth) return;
        
        if(health + amount > maxHealth) health = maxHealth;
        else health += amount;

        hud.OnUpdateHUD?.Invoke();
    }

    private void Shield(float amount){
        if(shield == 50) return;

        if(shield + amount > 50) shield = 50;
        else shield += amount; 

        hud.OnUpdateHUD?.Invoke();
    }

    /*TEMPORARY TEST CODE*/
    private void Update() {
        if(healthRegenCounter < healthRegenTimer){
            healthRegenCounter += Time.deltaTime;
        }
        else if(health < maxHealth){
            health += Time.deltaTime * healthRegenSpeed;
            if(health > maxHealth) health = maxHealth;
            hud.OnUpdateHUD?.Invoke();
        }
        else if (shield < maxShield)
        {
            shield += Time.deltaTime * shieldRegenSpeed;
            if (shield > maxShield) shield = maxShield;
            hud.OnUpdateHUD?.Invoke();
        }
        
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Z)){ TakeDamage(10f);}

        if(Input.GetKeyDown(KeyCode.X)){ Heal(5f);}

        if(Input.GetKeyDown(KeyCode.C)){ Shield(10f);}

        #endif
    }

    [ContextMenu("Take Damage")]
    void TestTakeDamage(){ TakeDamage(10f); }

    [ContextMenu("Get Health")]
    void TestGetHealth(){ Heal(5f); }

    [ContextMenu("Get Shield")]
    void TestGetShield(){ Shield(10f); }
}
