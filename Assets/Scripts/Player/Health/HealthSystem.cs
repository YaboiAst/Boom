using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

    // Vignette postprocess
    [SerializeField] PostProcessProfile profile;
    private Vignette vignette;
    private float vignetteTime = 0.3f;
    private float vignetteTimer = 0f;
    private bool fadingIn = false;
    private bool fadingOut = false;

    private void Start()
    {
        profile.TryGetSettings(out vignette);
        vignette.intensity.value = 0f;
    }

    public float GetHealth(){return health;}
    public float GetShield(){return shield;}

    public void TakeDamage(float amount){
        healthRegenCounter = 0f;
        isOnCombat = true;
        if (shield > 0)
        {
            shield -= amount;
            VignnetteEffect(0.3f, Color.blue);
            if (shield < 0)
            {
                health += shield;
                shield = 0;
            }
        }
        else
        {
            VignnetteEffect(0.3f, Color.red);
            if(health - amount < 0){
                health = 0;
            }
            else
                health -= amount;
        }

        hud.OnUpdateHUD?.Invoke();
    }

    public void Heal(float amount){
        if(health == maxHealth) return;

        if (health + amount > maxHealth)
        {
            health = maxHealth;
        }
        else health += amount;

        hud.OnUpdateHUD?.Invoke();
    }

    public void Shield(float amount){
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
        else if (shield < maxShield)
        {
            shield += Time.deltaTime * shieldRegenSpeed;
            if (shield > maxShield)
            {
                shield = maxShield;
                VignnetteEffect(0.5f, Color.green);
            }
            hud.OnUpdateHUD?.Invoke();
        }
        
        if (fadingIn || fadingOut)
        {
            vignetteTimer += Time.deltaTime;
            float t = vignetteTimer / vignetteTime;
            if (fadingOut)
            {
                vignette.intensity.value = Mathf.Lerp(0.5f, 0f, t);
            }
            else
                vignette.intensity.value = Mathf.Lerp(0f, 0.5f, t*2f);
            
            if(vignetteTimer >= vignetteTime)
            {
                fadingIn = false;
                fadingOut = false;
                vignetteTimer = 0f;
            }
        }
        
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Z)){ TakeDamage(10f);}

        if(Input.GetKeyDown(KeyCode.X)){ Heal(5f);}

        if(Input.GetKeyDown(KeyCode.C)){ Shield(10f);}

        #endif
    }

    private void VignnetteEffect(float time, Color color)
    {
        vignetteTime = time;
        vignette.color.value = color;
        vignette.active = true;
        fadingIn = true;
        StartCoroutine("EndDamageVignette");
    }
    
    IEnumerator EndDamageVignette()
    {
        yield return new WaitForSeconds(vignetteTime);
        fadingIn = false;
        fadingOut = true;
    }
    
    [ContextMenu("Take Damage")]
    void TestTakeDamage(){ TakeDamage(10f); }

    [ContextMenu("Get Health")]
    void TestGetHealth(){ Heal(5f); }

    [ContextMenu("Get Shield")]
    void TestGetShield(){ Shield(10f); }
}
