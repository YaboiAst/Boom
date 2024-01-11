using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private GameObject Paw;
    [SerializeField] private LayerMask whatIsEnemie;
    [Range(0f, 4f)][SerializeField] private float meleeRange;
    [SerializeField] private float meleeDamage = 10f;

    private void Start() {
        Paw.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            Paw.SetActive(true);

            Collider[] enemiesInMeleeRange = Physics.OverlapSphere(transform.position + transform.forward * meleeRange, meleeRange, whatIsEnemie);
            foreach(Collider col in enemiesInMeleeRange){
                col.gameObject.GetComponent<EnemieAI>().TakeDamage(meleeDamage);
            }

            Invoke("StopMelee", 1f);
        }
    }

    public void MeleeAtack()
    {
        Paw.SetActive(true);

        Collider[] enemiesInMeleeRange = Physics.OverlapSphere(transform.position + transform.forward * meleeRange, meleeRange, whatIsEnemie);
        foreach(Collider col in enemiesInMeleeRange){
            col.gameObject.GetComponent<HealthSystem>().TakeDamage(meleeDamage);
        }

        Invoke("StopMelee", 1f);
    }
    
    

    void StopMelee(){
        Paw.SetActive(false);
    }
}
