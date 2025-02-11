using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float walkSpeed = 4f;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    public Animator animator;
    public float gravity = -9.8f;
    public float jumpHeight = 3.0f;
    public Interactable focus;
    private InputAction movement;
    public static bool isInteraction = false;

    float turnSmoothVelocity;
    bool isRun;
    bool isGrounded;
    bool isJump = false;
    Vector3 velo;
    Vector3 direction;
    float targetAngle;
    float angle;
    private PlayerControls playerControl;

    private void Awake()
    {
        playerControl = new PlayerControls();
    }

    public void OnEnable()
    {
        movement = playerControl.Player.Move;
        movement.Enable();

        playerControl.Player.Jump.performed += DoJump;
        playerControl.Player.Jump.Enable();
        InteractEnable();
    }

    public void InteractEnable()
    {
        playerControl.Player.Interact.performed += DoInteract;
        playerControl.Player.Interact.Enable();
    }

    public void InteractDisable()
    {
        isInteraction = false;
        playerControl.Player.Interact.Disable();
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if(isGrounded)
        {
            isJump = true;
            isGrounded = false;
            Debug.Log("Jump");
            animator.SetTrigger("isJump");
            StartCoroutine(Jump());
        }

    }

    private void DoInteract(InputAction.CallbackContext obj)
    {
        isInteraction = true;
        animator.SetBool("interaction", isInteraction);
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.1f);
        velo.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    public void OnDisable()
    {
        movement.Disable();
        playerControl.Player.Jump.Disable();
        InteractDisable();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            if (!TryGetComponent<Rigidbody>(out rb))
            {
                Debug.Log("Rigidbody2D is missing");
            }

        }
    }

    void OnCollisionStay(Collision collision )
    {
        int x = 0;
        foreach (ContactPoint hitPos in collision.contacts)
        {
            if (hitPos.normal.x != 0f) // check if the wall collided on the sides
            {
                if(velo.y > -10f)
                {
                   // x++;
                }   
            }// boolean to prevent player from being able to jump
            else if (hitPos.normal.y > 0f) // check if its collided on top 
            {
                x++;
            }
        }
        if(x > 0)
        {
            isJump = false;
            animator.SetBool("isAir", false);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        Vector3 origin = transform.position;
        if (Physics.Raycast(origin, -Vector3.up, 0.01f))
        {
            isJump = false;
            animator.SetBool("isAir", false);
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        RaycastHit hit;
        Vector3 origin = transform.position;
        if (Physics.SphereCast(origin, 0.3f, -Vector3.up, out hit))
        {
            animator.SetBool("isAir", false);
        }
        if (!isGrounded)
        {
            animator.SetBool("isAir", true);
        }
       
    }

    void FixedUpdate()
    {
        //Debug.Log("movement:" + movement.ReadValue<Vector2>());
        
        if(isGrounded)
        {
           direction = new Vector3(movement.ReadValue<Vector2>().x * walkSpeed, 0, movement.ReadValue<Vector2>().y * walkSpeed).normalized;
        }
        

        if(isGrounded && velo.y < 0)
        {
            velo.y = -1f;
        }



        if (direction.magnitude >= 0.1f)
        {
            isRun = true;
            animator.SetBool("isRun", isRun);
            
           
            if(isGrounded)
            {
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            }
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = walkSpeed * moveDir.normalized;
        }
        else
        {
            isRun = false;
            animator.SetBool("isRun", isRun);
            rb.velocity = Vector3.zero;
        }
        velo.y += gravity * Time.deltaTime;
        rb.velocity += velo * Time.deltaTime;
        
      
        
    }
}
