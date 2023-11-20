using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using UnityEditor;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GunTemplate gun;
    [SerializeField] private float shootingSpeed = 1f;
    private bool isShooting = false;
    [SerializeField] private float critMultiplier = 2f;
    private float cooldownCounter = 0f;

    public GameObject bullet;
    public Transform gunEnd;
    private Vector3 shotDirection;

    public Camera mainCam;
    private Vector3 aim;

    private void Start() {
        mainCam = GetComponentInChildren<Camera>();
        aim = new Vector3 (Screen.width / 2, Screen.height / 2, 0f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            if(cooldownCounter <= 0 && !isShooting){
                isShooting = true;
                cooldownCounter = shootingSpeed;

                gun.bulletsLeft = gun.bulletsPerShot;

                if(gun.ammo > 0){
                    Shoot();
                }
            }
        }

        cooldownCounter -= Time.deltaTime;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(ray);    
    }

    Ray ray;
    void Shoot(){
        float x = Random.Range(-gun.spread, gun.spread);
        float y = Random.Range(-gun.spread, gun.spread);
        Vector3 spreading = aim + new Vector3(x, y, 0f);

        shotDirection = mainCam.ScreenToWorldPoint(spreading) + mainCam.transform.forward * 100f - gunEnd.position;
        GameObject bul = Instantiate(bullet, gunEnd.position, transform.rotation);
        Destroy(bul, 5f);
        bul.GetComponent<Rigidbody>().AddForce(shotDirection * 1000f);

        ray = mainCam.ScreenPointToRay(spreading);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            if(hit.rigidbody != null){
                EnemieAI enemy = hit.rigidbody.gameObject.GetComponent<EnemieAI>();
                if(hit.collider.gameObject.tag == "Head"){
                    Debug.Log("Headshot");
                    enemy.TakeDamage(critMultiplier * gun.damagePerShot);
                }
                else enemy.TakeDamage(gun.damagePerShot);
            }
        }

        gun.ammo--;
        gun.bulletsLeft--;
        if(gun.bulletsLeft > 0 && gun.ammo > 0){
            Invoke("Shoot", gun.fireRate);
        }
        else if(gun.bulletsLeft == 0){
            isShooting = false;
        }
    }
}
