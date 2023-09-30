using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrogInputSystem : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    private bool isGrounded;
    private ConstantForce gravity;
    private Animator animator;
    private Collider[] cols;

    public InputAction playerControls;

    public float jumpHeight;
    public float maxJumpHeight;
    public float rotationSpeed;

    Vector2 rotateDirection = Vector2.zero;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cols = GetComponents<Collider>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.canceled += Jump;
        playerInputActions.Player.Jump.performed += ReadyJump;

        // rb.useGravity = false;
        gravity = gameObject.AddComponent<ConstantForce>();
        updateGravity(new Vector3(0, -1.0f, 0));

        isGrounded = true;
    }

    private void Update()
    {
        rotateDirection = playerControls.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        transform.Rotate(0f, rotateDirection.x * rotationSpeed, rotateDirection.y * rotationSpeed);
    }

    public void ReadyJump(InputAction.CallbackContext context)
    {
        animator.SetBool("isReadyingJump", true);

        //foreach (Collider col in cols)
        //{
        //    col.isTrigger = false;
        //}
    }

    public void Jump(InputAction.CallbackContext context)
    {
        float calculatedJump;

        if (context.canceled && isGrounded)
        {
            isGrounded = false;
            //rb.constraints = RigidbodyConstraints.None;

            // set animation for jump
            animator.SetBool("isFlying", true);
            animator.SetBool("isReadyingJump", false);

            // free rigid body constraints
            rb.constraints = RigidbodyConstraints.FreezeRotation;
           // rb.useGravity = false;

            calculatedJump = (float)context.duration * jumpHeight;

            if (calculatedJump > maxJumpHeight)
            {
                calculatedJump = maxJumpHeight;
            }

            rb.AddForce(calculatedJump * (transform.forward + transform.up), ForceMode.Impulse);
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        //rb.useGravity = true;

        //rb.constraints = RigidbodyConstraints.None;

        // reset animation for jump
        animator.SetBool("isFlying", false);

        if (LayerMask.LayerToName(collision.gameObject.layer) == "Wall")
        {
            StartCoroutine(stickToWall(collision.contacts[0]));
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "NoRotate")
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "ResetRotation")
        {
            transform.rotation = new Quaternion(0, rb.rotation.y, 0, rb.rotation.w);
            rb.velocity = Vector3.zero;
            updateGravity(new Vector3(0, -1.0f, 0));
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            updateGravity(new Vector3(0, -1.0f, 0));
        }
    }

    private IEnumerator stickToWall(ContactPoint contact)
    {
        //if (!animator.GetBool("isReadyingJump")) {
        rb.velocity = Vector3.zero;
            //rb.useGravity = false;
            //var rot = Quaternion.FromToRotation(transform.up, contact.normal);
        
        yield return new WaitForSecondsRealtime(0.18f);
        
        updateGravity(new Vector3(-0.8f * contact.normal.x, -contact.normal.y, -0.8f * contact.normal.z));

        rb.constraints = RigidbodyConstraints.FreezeAll;

            //foreach (Collider col in cols)
            //{
            //    col.isTrigger = true;
            //}
        //}
    }

    private void updateGravity(Vector3 newGravity)
    {
        gravity.force = 9.81f * newGravity;
    }
}
