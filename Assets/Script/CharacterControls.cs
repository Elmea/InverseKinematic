using UnityEngine;

public struct Inputs
{
    public bool MoveForward;
    public bool MoveBackward;
    public bool MoveLeft;
    public bool MoveRight;

    public void Reset()
    {
        MoveForward = false;
        MoveBackward = false;
        MoveLeft = false;
        MoveRight = false;
    }

    public bool Any()
    {
        return MoveForward || MoveBackward || MoveLeft || MoveRight;
    }
}

[RequireComponent (typeof(Rigidbody))]
public class CharacterControls : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Camera m_camera;
    [SerializeField] Transform CameraParent;
    [SerializeField] float lookSpeed = 2;
    [SerializeField] float lookXLimit = 60;
    [SerializeField] float scrollDelta = 0.5f;
    [SerializeField] float scrollMax = -0.1f;

    [SerializeField] float heightStep = 0.01f;
    [SerializeField] float maxHeight = 1.3f;
    [SerializeField] float minHeight = 0.1f;


    Vector2 camRotation = new Vector2();
    Vector3 camPos = new Vector3();

    [Header("Movement")]
    [SerializeField] float moveSpeed = 0.1f;

    private Inputs inputs;
    private Rigidbody body;
    private bool isWalking;
    public bool IsWalking { get { return isWalking; } }


    void UpdateInputs()
    {
        inputs.Reset();

        inputs.MoveForward = Input.GetKey(KeyCode.W);
        inputs.MoveBackward = Input.GetKey(KeyCode.S);
        inputs.MoveLeft = Input.GetKey(KeyCode.A);
        inputs.MoveRight = Input.GetKey(KeyCode.D);
    }

    #region Camera
    private void CameraMovement()
    {
        float scroolValue = Input.mouseScrollDelta.y * scrollDelta;
        if (scroolValue + camPos.z < scrollMax)
            camPos.z += scroolValue;
        
        m_camera.transform.localPosition = camPos;
        CameraSnapBack();

        camRotation.y += Input.GetAxis("Mouse X") * lookSpeed;
        camRotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
        camRotation.x = Mathf.Clamp(camRotation.x, -lookXLimit, lookXLimit);
        CameraParent.localRotation = Quaternion.Euler(camRotation.x, camRotation.y, 0);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (CameraParent.transform.localPosition.y + heightStep < maxHeight)
                CameraParent.transform.localPosition = CameraParent.transform.localPosition + new Vector3(0, heightStep, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (CameraParent.transform.localPosition.y - heightStep > minHeight)
                CameraParent.transform.localPosition = CameraParent.transform.localPosition + new Vector3(0, -heightStep, 0);
        }
    }

    private void CameraSnapBack()
    {
        Vector3 dir = m_camera.transform.position - CameraParent.transform.position;
        RaycastHit hit = new RaycastHit();

        // Layer 6 is wall layer
        if (Physics.Raycast(CameraParent.position, dir, out hit, Mathf.Abs(camPos.z), 1 << 6))
        {
            float dist = Vector3.Distance(hit.point, CameraParent.position);
            Vector3 SnappedCamPos = new Vector3(camPos.x, camPos.y, camPos.z);
            SnappedCamPos.z = -dist;
            m_camera.transform.localPosition = SnappedCamPos;
        }
    }
    #endregion

    #region Movement
    void RotateToCameraRot()
    {
        Vector3 camForward = CameraParent.transform.forward;

        transform.forward = new Vector3(camForward.x, 0, camForward.z);
        camRotation.y = 0;
    }

    Vector3 MoveDir()
    {
        Vector3 result = new Vector3();

        if (inputs.MoveForward)
            result += transform.forward;

        if (inputs.MoveBackward)
            result += -transform.forward;

        if (inputs.MoveRight)
            result += transform.right;

        if (inputs.MoveLeft)
            result += -transform.right;

        return result.normalized;
    }

    void UpdateMovement()
    {
        if (inputs.Any())
        {
            isWalking = true;
            RotateToCameraRot();

            float YVel = body.velocity.y;
            body.velocity = MoveDir() * moveSpeed;
            body.velocity = body.velocity + new Vector3(0, YVel, 0);
        }
        else
        {
            isWalking = false;
            body.velocity = new Vector3(0, body.velocity.y, 0);
        }
    }
    #endregion

    #region MonoBehavior
    // Start is called before the first frame update
    void Start()
    {
        camPos = m_camera.transform.localPosition;
        inputs = new Inputs();
        inputs.Reset();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        UpdateInputs();
        UpdateMovement();
    }
    #endregion
}
