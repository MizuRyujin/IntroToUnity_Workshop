using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Tooltip("The distance tolerance before moving with player")]
    [SerializeField] private int _aimDeadZone;
    private Camera _cam;
    private Bounds _camBounds;
    private Vector2 max;
    private Vector2 min;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _camBounds = GetCamBounds();

        max = LevelManager.Instance.LevelBounds.max;
        min = LevelManager.Instance.LevelBounds.min;
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    /// <summary>
    /// Method to create the camera viewport bounds.
    /// </summary>
    /// <returns> Viewport bounds. </returns>
    private Bounds GetCamBounds()
    {
        Bounds bounds;
        Vector2 extents;

        //* Here we get the size of the viewport...
        float vertExtent = _cam.orthographicSize;
        float horizontalExtent = vertExtent * (Screen.width / Screen.height);

        //* ... create a vector with the bounds extents value...
        extents = new Vector2(horizontalExtent, vertExtent);

        //* ... instantiate a new bound and return it afterwards
        bounds = new Bounds(transform.position, (Vector3)extents);

        return bounds;
    }

    /// <summary>
    /// Method to move the camera with the player with smooth movement
    /// </summary>
    private void MoveCamera()
    {
        //* Have the bounds center with camera current position
        _camBounds.center = transform.position;

        //* Get the players position and check if he is too far from
        //* the center of the camera
        Vector2 targetPos = LevelManager.Instance.Player.transform.position;

        if (Vector2.Distance(_camBounds.center, targetPos) > _aimDeadZone)
        {
            //* If it is true, move the camera smoothly
            Vector3 moveDir = new Vector3(targetPos.x, targetPos.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, moveDir,
                                                1 * Time.deltaTime);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_camBounds.center, _camBounds.extents);
    }
}
