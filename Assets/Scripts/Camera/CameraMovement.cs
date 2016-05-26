using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Vector3 MovementSpeed;

    // Update is called once per frame

    Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        // Keyboard
        Vector3 movementDiff = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            movementDiff.x -= MovementSpeed.x;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementDiff.x += MovementSpeed.x;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementDiff.y -= MovementSpeed.y;
        }
        if (Input.GetKey(KeyCode.W))
        {
            movementDiff.y += MovementSpeed.y;
        }

        // Transform is implicit for the camera
        transform.position += movementDiff;

        var scroll = Input.mouseScrollDelta;
        if ((scroll.x != 0) || (scroll.y != 0))
        {
            _camera.orthographicSize -= MovementSpeed.z * scroll.y;
        }

        // Quick quit
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
