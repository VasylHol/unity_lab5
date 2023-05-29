// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     public float speed;
//     public float rotationSpeed;
//     public float jumpSpeed;
//     public float jumpButtonGracePeriod;

//     private Animator animator;
//     private CharacterController characterController;
//     private float ySpeed;
//     private float originalStepOffset;
//     private float? lastGroundedTime;
//     private float? jumpButtonPressedTime;
//     private bool isJumping;
//     private bool isGrounded;

//     void Start()
//     {
//         animator = GetComponent<Animator>();
//         characterController = GetComponent<CharacterController>();
//         originalStepOffset = characterController.stepOffset;
//     }

//     void Update()
//     {
//         float horizontalInput = Input.GetAxis("Horizontal");
//         float verticalInput = Input.GetAxis("Vertical");

//         Vector3 movementDirection = new Vector3(-horizontalInput, 0, -verticalInput);
//         float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
//         movementDirection.Normalize();

//         ySpeed += Physics.gravity.y * Time.deltaTime;

//         if (characterController.isGrounded)
//         {
//             lastGroundedTime = Time.time;
//         }

//         if (Input.GetButtonDown("Jump"))
//         {
//             jumpButtonPressedTime = Time.time;
//         }

//         if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
//         {
//             characterController.stepOffset = originalStepOffset;
//             ySpeed = -0.5f;
//             animator.SetBool("isgrounded",true);
//             isGrounded=true;
//             animator.SetBool("isjumping",false);
//             isJumping=false;
//             animator.SetBool("isfalling",false);

//             if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
//             {
//                 ySpeed = jumpSpeed;
//                 jumpButtonPressedTime = null;
//                 animator.SetBool("isjumping",true);
//                 isJumping = true;
//                 lastGroundedTime = null;
//             }
//         }
//         else
//         {
//             characterController.stepOffset = 0;
//             animator.SetBool("isgrounded",false);
//             isGrounded = false;

//             if(isJumping && ySpeed<0){
//                 animator.SetBool("isfalling",true);
//             }
//         }

//         Vector3 velocity = movementDirection * magnitude;
//         velocity.y = ySpeed;

//         characterController.Move(velocity * Time.deltaTime);

//         if (movementDirection != Vector3.zero)
//         {
//             animator.SetBool("ismoving", true);
//             Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

//             transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
//         }
//         else
//         {
//             animator.SetBool("ismoving", false);
//         }
//     }
// }

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private float jumpHorizontalSpeed;

    [SerializeField]

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }

        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);

        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            animator.SetBool("isgrounded", true);
            isGrounded = true;
            animator.SetBool("isjumping", false);
            isJumping = false;
            animator.SetBool("isfalling", false);
            
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                animator.SetBool("isjumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
            animator.SetBool("isgrounded", false);
            isGrounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("isfalling", true);
            }
        }

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("ismoving", true);

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("ismoving", false);
        }

        if (isGrounded == false)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);
        }
    }

    private void OnAnimatorMove()
    {
        if (isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
