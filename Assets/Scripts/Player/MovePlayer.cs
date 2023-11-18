using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [Header("Base Movement")]
    [SerializeField] private float baseSpeed;
    private Vector2 movInput;
    private CharacterController playerController;

    [Header("Dash")]
    [SerializeField] private float dashForce = 3;
    [SerializeField] private float dashTime = 0.25f;
    private float dashTimeCounter = 0f;
    [SerializeField] private int maxDashCharges = 2;
    private int dashCharges = 2;
    [SerializeField] private float dashCooldown = 2.5f;
    private float dashCooldownCounter = 0f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 20f;

    [Header("Gravity")]
    [SerializeField] private float g = -9.81f;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private Vector3 velocity;
    private bool isGrounded;


    void Start(){
        playerController = GetComponent<CharacterController>();
        dashCharges = maxDashCharges;
    }

    void myInput(){
        movInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            velocity.y = Mathf.Sqrt(jumpForce * -2f * g);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && movInput != Vector2.zero)
            if(dashCharges > 0){
                dashCharges -= 1;
                dashTimeCounter = dashTime;
            }
        
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, whatIsGround);

        myInput();

        dashCooldownCounter -= Time.deltaTime;
        if(dashCooldownCounter < 0 && dashCharges < maxDashCharges){
            dashCharges += 1;
            if(dashCharges < maxDashCharges){
                dashCooldownCounter = dashCooldown;
            }
        }
        
    }

    void FixedUpdate(){
        if(isGrounded && velocity.y < 0f)
            velocity.y = -2f;
        else if(!isGrounded)
            velocity.y += g * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);

        Vector3 moveDirection = transform.right * movInput.x + transform.forward * movInput.y;
        playerController.Move(moveDirection * baseSpeed * Time.deltaTime);

        if(dashTimeCounter > 0){
            playerController.Move(moveDirection * dashForce * 10f * Time.deltaTime);
            dashTimeCounter -= Time.deltaTime;
        }
        else if(dashCooldownCounter <= 0){
            if(dashCharges == maxDashCharges)
                dashCooldownCounter = dashCooldown;
        }
    }
}
