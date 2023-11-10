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
    private Vector3 originalPos;

    //public InputAction playerControls;

    public float jumpHeight;
    public float maxJumpHeight;
    public float rotationSpeed;
    public float minJumpHeight;

    Vector2 rotateDirection = Vector2.zero;

    private void OnEnable()
    {
        //playerControls.Enable();
    }

    private void OnDisable()
    {
        //playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cols = GetComponents<Collider>();
        //playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        //PlayerInputActions playerInputActions = new PlayerInputActions();
        //playerInputActions.Player.Enable();
        //playerInputActions.Player.Jump.canceled += Jump;
        //playerInputActions.Player.Jump.performed += ReadyJump;

        // Lock cursor to screen
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        gravity = gameObject.AddComponent<ConstantForce>();
        updateGravity(new Vector3(0, -1.0f, 0));

        isGrounded = true;
    }

    private void Update()
    {
        StartCoroutine(reset());

        if (Input.GetKeyUp("1"))
        {
            transform.position = new Vector3(2.38f, 0.099f, -98.54f);
            transform.rotation = Quaternion.identity;
            rb.constraints = RigidbodyConstraints.None;
        }
        if (Input.GetKeyUp("2"))
        {
            transform.position = new Vector3(3.16f, 16.015f, -57.21f);
            transform.rotation = Quaternion.identity;
            rb.constraints = RigidbodyConstraints.None;
        }
        if (Input.GetKeyUp("3"))
        {
            transform.position = new Vector3(3.16f, 41.278f, -20.75f);
            transform.rotation = Quaternion.identity;
            rb.constraints = RigidbodyConstraints.None;
        }
        if (Input.GetKeyUp("4"))
        {
            transform.position = new Vector3(4.301f, 52.899f, -8.81f);
            transform.rotation = Quaternion.identity;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    void FixedUpdate()
    {
        transform.Rotate(0f, rotateDirection.x * rotationSpeed, rotateDirection.y * rotationSpeed);
    }

    public void ReadyJump(InputAction.CallbackContext context)
    {
        animator.SetBool("isReadyingJump", true);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        float calculatedJump;

        animator.SetBool("isReadyingJump", true);

        if (context.canceled && isGrounded)
        {
            isGrounded = false;

            originalPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            // set animation for jump
            animator.SetBool("isFlying", true);
            animator.SetBool("isReadyingJump", false);

            // free rigid body constraints
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            calculatedJump = (float)context.duration * jumpHeight;

            if (calculatedJump > maxJumpHeight)
            {
                calculatedJump = maxJumpHeight;
            }
            else if (calculatedJump< minJumpHeight) {
                calculatedJump = minJumpHeight;
            }

            rb.AddForce(calculatedJump * (transform.forward + transform.up), ForceMode.Impulse);
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        rotateDirection = context.ReadValue<Vector2>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;

        rb.constraints = RigidbodyConstraints.None;

        // reset animation for jump
        animator.SetBool("isFlying", false);

        if (LayerMask.LayerToName(collision.gameObject.layer) == "Wall")
        {
            StartCoroutine(stickToWall(collision.contacts[0]));
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Powerup")
        {
            collision.gameObject.SetActive(false);
            maxJumpHeight = 8.5f;

            StartCoroutine(powerupTimer());
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Trap")
        {
            StartCoroutine(reactivateTrap(collision.gameObject));
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.velocity = Vector3.zero;
        }
        else if (LayerMask.LayerToName(collision.gameObject.layer) == "NoRotate")
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.velocity = Vector3.zero;
            //updateGravity(new Vector3(0, -1.0f, 0));
        }
        else if (LayerMask.LayerToName(collision.gameObject.layer) == "ResetRotation")
        {
            transform.rotation = new Quaternion(0, rb.rotation.y, 0, rb.rotation.w);
            rb.velocity = Vector3.zero;
            updateGravity(new Vector3(0, -1.0f, 0));
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else if (LayerMask.LayerToName(collision.gameObject.layer) == "BridgeEdge")
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        else if (LayerMask.LayerToName(collision.gameObject.layer) == "Cloud")
        {
            updateGravity(new Vector3(0, -1.0f, 0));
            StartCoroutine(stickToCloud(collision.contacts[0]));
        }
        else
        {
            updateGravity(new Vector3(0, -1.0f, 0));
        }
    }

    private IEnumerator stickToWall(ContactPoint contact)
    {
        rb.velocity = Vector3.zero;

        yield return new WaitForSecondsRealtime(0.2f);

        updateGravity(new Vector3(-0.8f * transform.up.x, -transform.up.y, -0.8f * transform.up.z));

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private IEnumerator stickToCloud(ContactPoint contact)
    {
        rb.velocity = Vector3.zero;

        yield return new WaitForSecondsRealtime(0.2f);

        updateGravity(new Vector3(-0.6f * transform.up.x, -0.4f * transform.up.y, -0.6f * transform.up.z));

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private IEnumerator reactivateTrap(GameObject trap)
    {
        yield return new WaitForSecondsRealtime(6f);

        trap.SetActive(true);
    }

    private IEnumerator powerupTimer()
    {
        yield return new WaitForSecondsRealtime(10);

        maxJumpHeight = 5f;
    }

    private IEnumerator reset()
    {
        int count = 0;
        int counter = 0;

        for (int i = 0; i < 25; i++)
        {
            if (animator.GetBool("isFlying"))
            {
                count++;
            }
            else if (!isGrounded)
            {
                counter++;
            }
            yield return new WaitForSeconds(0.1f);
        }

        if (count == 25 || counter == 25)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = originalPos + new Vector3(0, .2f, 0);
            transform.rotation = new Quaternion(transform.rotation.x/2, transform.rotation.y, transform.rotation.z/2, transform.rotation.w);
            animator.SetBool("isFlying", false);
            animator.SetBool("isReadyingJump", false);

            updateGravity(new Vector3(0, -1f, 0));
            rb.constraints = RigidbodyConstraints.None;
        }

        bool xl = transform.rotation.x > 0.6;
        bool zl = transform.rotation.z > 0.6;
        bool xs = transform.rotation.x < -0.6;
        bool zs = transform.rotation.z < -0.6;

        if (xl)
        {
            transform.rotation = new Quaternion(0.4f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            transform.position = transform.position + new Vector3(0, .01f, 0);
            //updateGravity(new Vector3(0, -1f, 0));
            //rb.constraints = RigidbodyConstraints.None;
        }
        if (zl)
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0.4f, transform.rotation.w);
            transform.position = transform.position + new Vector3(0, .015f, 0);
            //updateGravity(new Vector3(0, -1f, 0));
            //rb.constraints = RigidbodyConstraints.None;
        }
        if (xs)
        {
            transform.rotation = new Quaternion(-0.4f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            transform.position = transform.position + new Vector3(0, .015f, 0);
            //updateGravity(new Vector3(0, -1f, 0));
            //rb.constraints = RigidbodyConstraints.None;
        }
        if (zs)
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, -0.4f, transform.rotation.w);
            transform.position = transform.position + new Vector3(0, .015f, 0);
            //updateGravity(new Vector3(0, -1f, 0));
            //rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void updateGravity(Vector3 newGravity)
    {
        gravity.force = 9.81f * newGravity;
    }
}
