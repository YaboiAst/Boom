//using System;
using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using UnityEditor;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Gun Info")]
    [SerializeField] private GunTemplate gun;
    [SerializeField] private float shootingSpeed = 1f; // Passar isso para gunTemplate?
    [SerializeField] private float critMultiplier = 2f;
    private bool isShooting = false;
    private float cooldownCounter = 0f;

    [Header("Shooting")]
    public Transform gunEnd;
    public GameObject bullet;
    private Vector3 shotDirection;
    
    public bool isOnMenu;

    [Header("Aim")]
    public Camera mainCam;
    private Vector3 aim;
    private float baseFOV;
    private bool isZooming = false;

    [Header("HUD")]
    [SerializeField] private HUDContoller hud;
    public GunTemplate GetGun(){return this.gun;}

    private void Start() {
        mainCam = GetComponentInChildren<Camera>();
        aim = new Vector3 (Screen.width / 2, Screen.height / 2, 0f);

        baseFOV = mainCam.fieldOfView;
    }

    void Update()
    {

        /* 
            === SCOPED AIM ===

            TODO
            - Fazer o crosshair escalar com o zoom?
            - Adicionar animação da arma mirando
            - UI da Scope (Escurecer as bordas)

        */
        if(Input.GetKeyDown(KeyCode.Mouse1)){
            isZooming = true;
            ZoomIn();
            //mainCam.fieldOfView = gun.zoomFOV;
        }
        if(Input.GetKeyUp(KeyCode.Mouse1)){
            isZooming = false;
            ZoomOut();
            //mainCam.fieldOfView = baseFOV;
        }

        /*
            === SHOOTING ===

            TODO
            - Atirar com o mouse segurado
            - Reduzir o spread se estiver mirando (?)
            - Atualizar HUD

        */
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            if(cooldownCounter <= 0 && !isShooting && !isOnMenu){
                if(gun.magCurrentAmmo > 0){
                    isShooting = true;
                    cooldownCounter = shootingSpeed;

                    gun.bulletsLeft = gun.bulletsPerShot;
                    Shoot();
                }
            }
        }


        /*
            === RELOAD ===

            TODO
            - Adicionar animação de reload
            - Atualizar HUD

        */
        if(Input.GetKeyDown(KeyCode.R)){
            // Play Reload Animation?
            if(gun.totalAmmo > 0 && gun.magCurrentAmmo < gun.magTotalAmmo){
                Invoke("Reload", 2f);
            }
        }

        cooldownCounter -= Time.deltaTime;
    }

    // private void OnDrawGizmos() {
    //     Gizmos.DrawRay(ray);    
    // }

    Ray ray;
    void Shoot(){
        /* Calcular Spread */
        float x = Random.Range(-gun.spread, gun.spread);
        float y = Random.Range(-gun.spread, gun.spread);
        Vector3 spreading = aim + new Vector3(x, y, 0f);

        /* Calcular direção */
        shotDirection = mainCam.ScreenToWorldPoint(spreading) + mainCam.transform.forward * 100f - gunEnd.position;
        
        /* Resposta visual (temporário) */
        GameObject bul = Instantiate(bullet, gunEnd.position, transform.rotation);
        Destroy(bul, 5f);
        bul.GetComponent<Rigidbody>().AddForce(shotDirection * 1000f);

        /* Raycast */
        ray = mainCam.ScreenPointToRay(spreading);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            if(hit.rigidbody != null){
                EnemieAI enemy = hit.rigidbody.gameObject.GetComponent<EnemieAI>();
                if(enemy != null){
                    if(hit.collider.gameObject.tag == "Head"){
                        Debug.Log("Headshot");
                        enemy.TakeDamage(critMultiplier * gun.damagePerShot);
                    }
                    else enemy.TakeDamage(gun.damagePerShot);
                }
            }
        }

        /* Atualiza munição */
        gun.magCurrentAmmo--;
        gun.bulletsLeft--;

        hud.OnUpdateHUD?.Invoke();

        /* Burst */
        if(gun.bulletsLeft > 0 && gun.magCurrentAmmo > 0)
            Invoke("Shoot", gun.burstRate);
        else if(gun.bulletsLeft == 0){
            isShooting = false;
        }
    }

    void Reload(){
        if(gun.totalAmmo < gun.magTotalAmmo){
            gun.magCurrentAmmo = gun.totalAmmo;
            gun.totalAmmo = 0;
        }
        else{
            gun.magCurrentAmmo = gun.magTotalAmmo;
            gun.totalAmmo -= gun.magTotalAmmo;
        }
        hud.OnUpdateHUD?.Invoke();
    }

    private void ZoomIn(){
        mainCam.fieldOfView -= Time.deltaTime * 50f;

        if(mainCam.fieldOfView < gun.zoomFOV){
            mainCam.fieldOfView = gun.zoomFOV;
        }

        if(!isZooming || mainCam.fieldOfView == gun.zoomFOV) return;
        else Invoke("ZoomIn", 0.01f);
    }

    private void ZoomOut(){
        mainCam.fieldOfView += Time.deltaTime * 50f;

        if(mainCam.fieldOfView > baseFOV){
            mainCam.fieldOfView = baseFOV;
        }

        if(isZooming || mainCam.fieldOfView == baseFOV) return;
        else Invoke("ZoomOut", 0.01f);
    }
    
    public void ChangeWeapon(GunTemplate newGun)
    {
        gun = newGun;
    }
}
