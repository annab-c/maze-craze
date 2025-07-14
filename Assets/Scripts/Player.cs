using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    Animator animator;
    CharacterController controller;

    public float moveSpeed = 5f;
    public float turnSpeed = 140f;
    public float gravity = -9.81f;
    private Vector3 velocity;

    [HideInInspector]
    public TextMeshProUGUI eText; 
    private MazeDoor nearbyDoor;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (eText != null)
            eText.enabled = false;
    }

    void Update()
    {
        MoveLogic();
        DoorLogic();
    }

    void MoveLogic()
    {
        bool forward = Input.GetKey(KeyCode.W);
        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        else if (Input.GetKey(KeyCode.D)) turn = 1f;

        transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f);

        Vector3 move = forward ? transform.forward : Vector3.zero;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded)
            velocity.y = -2f;
        else
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("Vertical", forward ? 1f : 0f);
        animator.SetBool("running", Input.GetKey(KeyCode.LeftShift));

        if (Input.GetKey(KeyCode.L)) animator.SetTrigger("lying");
        else if (Input.GetKeyDown(KeyCode.Space)) animator.SetTrigger("jump");
        else if (Input.GetKeyDown(KeyCode.K)) animator.SetTrigger("knockdown");
        else if (Input.GetKeyDown(KeyCode.Mouse0)) animator.SetTrigger("punch_L");
        else if (Input.GetKeyDown(KeyCode.Mouse1)) animator.SetTrigger("punch_R");

        animator.SetBool("sidefix", Input.GetKey(KeyCode.LeftControl));
    }

    void DoorLogic()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.4f);
        nearbyDoor = null;

        foreach (Collider hit in hits)
        {
            MazeDoor door = hit.GetComponentInParent<MazeDoor>();
            if (door != null)
            {
                nearbyDoor = door;
                break;
            }
        }

        if (nearbyDoor != null)
        {
            if (eText != null)
                eText.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                nearbyDoor.ToggleDoor();
            }
        }
        else
        {
            if (eText != null)
                eText.enabled = false;
        }
    }
}
