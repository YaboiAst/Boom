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
    [SerializeField] private float dashForce = 10;
    [SerializeField] private int maxDashCharges = 2;
    private int dashCharges = 2;
    [SerializeField] private float dashCooldown = 2.5f;
    //private int dashesToCharge = 0;
    private float dashCooldownCounter = 0f;
    private bool isDashing;

    [Header("Gravity")]
    [SerializeField] private float g = -9.81f;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private Vector3 velocity;
    private bool isGrounded;

    // [Header("Jumping")]
    // [SerializeField] private float jumpForce = 50f;
    // private bool jump;

    void Start(){
        playerController = GetComponent<CharacterController>();
        dashCharges = maxDashCharges;
    }

    void myInput(){
        movInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

<<<<<<< Updated upstream
        if(Input.GetKeyDown(KeyCode.LeftShift) && movInput != Vector2.zero)
            if(dashCharges > 0)
                isDashing = true;
=======
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            velocity.y = Mathf.Sqrt(jumpForce * -2f * g);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && movInput != Vector2.zero){
            if(dashCharges > 0){
                dashCharges -= 1;
                dashTimeCounter = dashTime;
            }
        }
>>>>>>> Stashed changes
        
        // if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
        //     jump = true;
        // }
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

        if(isDashing){
            playerController.Move(moveDirection * dashForce * 10f * Time.deltaTime);
            dashCharges -= 1;
            if(dashCooldownCounter <= 0){
                dashCooldownCounter = dashCooldown;
            }
            isDashing = false;
        }

        // if(jump && isGrounded){
        //     playerController.Move(transform.up * jumpForce * 10f * Time.deltaTime);
        //     jump = false;
        // }
    }
}
