using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HUDContoller : MonoBehaviour
{
    public UnityEvent OnUpdateHUD;
    public GameObject playerInfo;
    private HealthSystem playerHealth;
    private GunTemplate playerAmmo;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;
    [SerializeField] private TextMeshProUGUI ammoIndicator;

    private void Start() {
        playerHealth = playerInfo.GetComponent<HealthSystem>();
        playerAmmo = playerInfo.GetComponent<PlayerShoot>().GetGun();

        OnUpdateHUD.AddListener(UpdateHUD);
        UpdateHUD();
    }

    private void UpdateHUD(){
        StopAllCoroutines();

        float healthPercentage = playerHealth.GetHealth() / playerHealth.maxHealth;
        float shieldPercentage = playerHealth.GetShield() / playerHealth.maxShield; // Change for max shield

        string ammo = playerAmmo.magCurrentAmmo.ToString() + " / " + playerAmmo.totalAmmo.ToString();
        float magPercentage = (float) playerAmmo.magCurrentAmmo / (float) playerAmmo.magTotalAmmo;
        //Debug.Log(playerAmmo.magCurrentAmmo + "/" + playerAmmo.magTotalAmmo + " = " + magPercentage);

        //healthBar.fillAmount = healthPercentage;
        //shieldBar.fillAmount = shieldPercentage;
        StartCoroutine(ChangeBar(healthBar, healthPercentage));
        StartCoroutine(ChangeBar(shieldBar, shieldPercentage));

        if(magPercentage > 0.2) ammoIndicator.color = Color.white;
        else ammoIndicator.color = Color.red;
        ammoIndicator.text = ammo;
    }

    IEnumerator ChangeBar(Image bar, float percentage){
        float mod = 1f;
        if(bar.fillAmount - percentage < 0) mod = -mod;
        
        while(bar.fillAmount != percentage){
            bar.fillAmount -= mod * Time.deltaTime;

            if(mod == 1  && bar.fillAmount < percentage) bar.fillAmount = percentage;
            if(mod == -1 && bar.fillAmount > percentage) bar.fillAmount = percentage;
            yield return null;
        }
    }
    
    public void ChangeWeapon(GunTemplate newGun){
        playerAmmo = newGun;
        UpdateHUD();
    }


}
