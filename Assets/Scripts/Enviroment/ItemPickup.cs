using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType {HEAL, SHIELD, AMMO}

[RequireComponent(typeof(BoxCollider))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private PickupType pickup;
    private PlayerShoot playerAmmo;
    private HealthSystem playerHealth;

    [SerializeField] public HUDContoller hud;

    [SerializeField] private int healAmount;
    [SerializeField] private int shieldAmount;
    [SerializeField] private int ammoAmount;

    private void Awake() {
        hud = FindObjectOfType<HUDContoller>();
    }


    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            playerAmmo = other.GetComponent<PlayerShoot>();
            playerHealth = other.GetComponent<HealthSystem>();
            switch(pickup){
                case PickupType.HEAL:
                    playerHealth.Heal(healAmount);
                    break;
                case PickupType.SHIELD:
                    playerHealth.Shield(shieldAmount);
                    break;
                case PickupType.AMMO:
                    playerAmmo.GetAmmo(ammoAmount);
                    break;
            }
            hud.OnUpdateHUD?.Invoke();

            Destroy(this.gameObject);
        }
    }
}
