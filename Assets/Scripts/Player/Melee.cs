using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private GameObject Paw;
    [SerializeField] private LayerMask whatIsEnemie;
    [Range(0f, 4f)][SerializeField] private float meleeRange;
    [SerializeField] private float meleeDamage = 10f;

    [SerializeField] private GameObject bloodEffect;

    private void Start() {
        Paw.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && Paw.activeSelf == false){
            Paw.SetActive(true);

            Collider[] enemiesInMeleeRange = Physics.OverlapSphere(transform.position + transform.forward * meleeRange, meleeRange, whatIsEnemie);
            foreach(Collider col in enemiesInMeleeRange){
                // Isso significa que o inimiga consegue tomar dano duas vezes de um melee...
                // Deixar assim?
                if(col.gameObject.CompareTag("Head"))
                    col.gameObject.GetComponentInParent<EnemieAI>().TakeDamage(meleeDamage);
                else 
                    col.gameObject.GetComponent<EnemieAI>().TakeDamage(meleeDamage);

                GameObject blood = Instantiate(bloodEffect, col.transform.position, col.transform.rotation);
                Destroy(blood, 5f);
            }

            Invoke("StopMelee", 1f);
        }
    }

    void StopMelee(){
        Paw.SetActive(false);
    }
}
