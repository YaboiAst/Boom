using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private ParticleSystem explosionEffect;

    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDamage;

    [SerializeField] private LayerMask whatIsPlayer, whatIsEnemie, whatIsBarrel;

    private void Start() {
        explosionEffect = GetComponentInChildren<ParticleSystem>();
    }
    public void Explode(){
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject, 2f);
        explosionEffect.Play();

        Collider[] enemieCollisions = Physics.OverlapSphere(this.transform.position, explosionRadius, whatIsEnemie);
        Collider[] playerCollision = Physics.OverlapSphere(this.transform.position, explosionRadius, whatIsPlayer);
        Collider[] barrelCollision = Physics.OverlapSphere(this.transform.position, explosionRadius, whatIsBarrel);

        if(playerCollision.Length > 0){
            playerCollision[0].gameObject.GetComponentInParent<HealthSystem>().TakeDamage(explosionDamage);
        }

        foreach(Collider enemie in enemieCollisions){
            enemie.gameObject.GetComponent<EnemieAI>().TakeDamage(explosionDamage);
        }
        
        foreach(Collider barrel in barrelCollision){
            barrel.gameObject.GetComponent<Barrel>().Explode();
        }
    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(this.transform.position, explosionRadius);
    // }
}
