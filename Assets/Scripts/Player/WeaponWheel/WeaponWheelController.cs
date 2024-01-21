using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public Animator animator;
    public bool weaponWheelSelected;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponID;

    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] private HUDContoller hudContoller;
    public MouseLook mouseLook;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            weaponWheelSelected = !weaponWheelSelected;
        }

        if(weaponWheelSelected)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }

    public void Select()
    {
        mouseLook.enabled = false;
        playerShoot.isOnMenu = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        animator.SetBool("weaponWheelSelected", true);
    }

    public void Deselect()
    {
        weaponWheelSelected = false;
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        animator.SetBool("weaponWheelSelected", false);
    }

    public void UnlockLook()
    {
        mouseLook.enabled = true;
        playerShoot.isOnMenu = false;
    }

    public void ChangeWeapon(GunTemplate newGun)
    {
        playerShoot.ChangeWeapon(newGun);
        hudContoller.ChangeWeapon(newGun);
    }

}
