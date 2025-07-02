using UnityEngine;

public class MazeDoor : MazePassage
{
    private MazeDoor OtherSideOfDoor
    {
        get
        {
            return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
        }
    }

    public Transform hinge;
    public float openAngle = 90f;
    public float speed = 2f;
    private bool opening = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = hinge.localRotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * closedRotation;
    }

    void Update()
    {
        // Simple example: press E to toggle (could be replaced with proximity checks later)
        if (Input.GetKeyDown(KeyCode.E))
        {
            opening = !opening;
        }

        if (opening)
        {
            hinge.localRotation = Quaternion.Lerp(hinge.localRotation, openRotation, Time.deltaTime * speed);
        }
        else
        {
            hinge.localRotation = Quaternion.Lerp(hinge.localRotation, closedRotation, Time.deltaTime * speed);
        }
    }

    public override void Initialize(MazeCell primary, MazeCell other, MazeDirection direction)
    {
        base.Initialize(primary, other, direction);
        if (OtherSideOfDoor != null)
        {
            hinge.localScale = new Vector3(-1f, 1f, 1f);
            Vector3 p = hinge.localPosition;
            p.x = -p.x;
            hinge.localPosition = p;
        }
    }
}
