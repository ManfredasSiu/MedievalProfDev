using Cinemachine;
using UnityEngine;

public class CameraPanAndZoom : MonoBehaviour
{
    [SerializeField]
    float panSpeed = 2f;
    
    [SerializeField]
    float zoomSpeed = 2f;

    [SerializeField]
    float zoomInMax = 2f;
    
    [SerializeField]
    float zoomOutMax = 20f;
    
    
    CinemachineInputProvider m_CinemachineInputProvider;
    CinemachineVirtualCamera m_VirtualCamera;
    Transform m_CameraTransform;

    void Awake()
    {
        m_CinemachineInputProvider = GetComponent<CinemachineInputProvider>();
        m_VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        m_CameraTransform = m_VirtualCamera.gameObject.transform;
    }

    void Update()
    {
        var xInput = m_CinemachineInputProvider.GetAxisValue(0);
        var yInput = m_CinemachineInputProvider.GetAxisValue(1);
        var zInput = m_CinemachineInputProvider.GetAxisValue(2);

        if (xInput != 0 || yInput != 0)
        {
            PanCamera(xInput, yInput);
        }
        if(zInput !=0)
        {
            ZoomCamera(-zInput);
        }
    }

    public void ZoomCamera(float increment)
    {
        var fov = m_VirtualCamera.m_Lens.OrthographicSize;

        var target = Mathf.Clamp(fov * increment, zoomInMax, zoomOutMax);

        if ((increment > 0 && zoomOutMax - fov >= 1f) ||
            (increment < 0 && fov - zoomInMax >= 1f))
        {
            m_VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(fov, target, zoomSpeed * Time.deltaTime);
        }
    }

    public Vector2 PanDirection(float xCoordinate, float yCoordinate)
    {
        var direction = Vector2.zero;
        if (yCoordinate >= Screen.height * 0.99f)
        {
            direction.y += 1;
        }
        if (yCoordinate <= Screen.height * 0.01f)
        {
            direction.y -= 1;
        }
        if (xCoordinate >= Screen.width * 0.99f)
        {
            direction.x += 1;
        }
        if (xCoordinate <= Screen.width * 0.01f)
        {
            direction.x -= 1;
        }

        return direction;
    }

    public void PanCamera(float xCoordinate, float yCoordinate)
    {
        var direction = PanDirection(xCoordinate, yCoordinate);
        m_CameraTransform.position = Vector3.Lerp(m_CameraTransform.position, m_CameraTransform.position + (Vector3) direction, panSpeed *Time.deltaTime);
    }
}
