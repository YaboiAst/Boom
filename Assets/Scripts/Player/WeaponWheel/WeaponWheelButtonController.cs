using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class WeaponWheelButtonController : MonoBehaviour
{
    public int id;
    public string itemName;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    public Sprite icon;

    private WeaponWheelController weaponWheelController;
    private bool selected;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        weaponWheelController = GetComponentInParent<WeaponWheelController>();
    }

    public void Selected()
    {
        selected = true;
        // selectedItem = icon;
        itemText.text = itemName;
        weaponWheelController.Deselect();
    }

    public void Deselected()
    {
        selected = false;
        // selectedItem = null;
        itemText.text = "";
    }

    public void OnHoverEnter()
    {
        animator.SetBool("onHover", true);
        itemText.text = itemName;
    }

    public void OnHoverExit()
    {
        animator.SetBool("onHover", false);
        itemText.text = "";
    }
}
