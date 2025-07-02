using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    Animator animator;
    CharacterController controller;

    public float moveSpeed = 5f;
    public float turnSpeed = 140f; // degrees per second, high for fast turns
    public float gravity = -9.81f;
    private Vector3 velocity;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool forward = Input.GetKey(KeyCode.W);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        // Rotate
        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        else if (Input.GetKey(KeyCode.D)) turn = 1f;

        float actualTurn = turn * turnSpeed * Time.deltaTime;

        transform.Rotate(0f, actualTurn, 0f);

        // Move forward
        Vector3 move = Vector3.zero;
        if (forward)
        {
            move = transform.forward;
        }

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animations
        animator.SetFloat("Vertical", forward ? 1f : 0f);
        animator.SetBool("running", Input.GetKey(KeyCode.LeftShift));

        if (Input.GetKey(KeyCode.L))
            animator.SetTrigger("lying");
        else if (Input.GetKeyDown(KeyCode.Space))
            animator.SetTrigger("jump");
        else if (Input.GetKeyDown(KeyCode.K))
            animator.SetTrigger("knockdown");
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            animator.SetTrigger("punch_L");
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            animator.SetTrigger("punch_R");

        animator.SetBool("sidefix", Input.GetKey(KeyCode.LeftControl));
    }
}
