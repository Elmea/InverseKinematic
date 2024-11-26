using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterIKBody : MonoBehaviour
{
    [Header("InverseKinematics")]
    [SerializeField] InverseKinematic LHandIK;
    [SerializeField] InverseKinematic RHandIK;
    [SerializeField] InverseKinematic LLegIK;
    [SerializeField] InverseKinematic RLegIK;

    [Header("Hand")]
    [SerializeField] float HandOffset = 0.15f;
    [SerializeField] Transform LHand;
    [SerializeField] Transform RHand;

    [Header("Shoulders")]
    [SerializeField] Transform LShoulder;
    [SerializeField] Transform RShoulder;

    [Header("Leg")]
    [SerializeField] float FootOffset;
    [SerializeField] Transform LUpperLeg;
    [SerializeField] Transform RUpperLeg;

    [Header("Targets")]
    [SerializeField] Transform LHandIKTarget;
    [SerializeField] Transform RHandIKTarget;
    [SerializeField] Transform LLegIKTarget;
    [SerializeField] Transform RLegIKTarget;

    private SphereCollider HandSphereCollider;

    [Header("Animation & Controls")]
    [SerializeField] float MaxHeightAdjustement = 0.5f;
    [SerializeField] float travelTime = 0.5f;
    [SerializeField] Transform armature;
    [SerializeField] Animator animator;
    [SerializeField] CharacterControls controls;

    [Header("debug")]
    [SerializeField] bool useBody = true;
    private float RLerpTime;
    private float LLerpTime;
    private Vector3 RLerpTarget;
    private Vector3 LLerpTarget;

    // Start is called before the first frame update
    void Start()
    {
        LHandIK.enabled = false;
        RHandIK.enabled = false;
        LLegIK.enabled = false;
        RLegIK.enabled = false;

        LHandIKTarget.gameObject.SetActive(false);
        RHandIKTarget.gameObject.SetActive(false);
        LLegIKTarget.gameObject.SetActive(false);
        RLegIKTarget.gameObject.SetActive(false);

        RLerpTarget = new Vector3();
        LLerpTarget = new Vector3();

        HandSphereCollider = GetComponent<SphereCollider>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 SphereCenter = transform.position + HandSphereCollider.center;
        Vector3 direction = other.ClosestPoint(SphereCenter) - SphereCenter;

        if (Vector3.Dot(direction, transform.right) > 0)
        {
            RHandIKTarget.transform.position = RHand.position;
        }
        else
        {
            LHandIKTarget.transform.position = LHand.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 SphereCenter = transform.position + HandSphereCollider.center;
        Vector3 direction = other.ClosestPoint(SphereCenter) - SphereCenter;
        float dot = Vector3.Dot(direction, transform.right);

        if (dot > 0)
        {
            RHandIKTarget.gameObject.SetActive(true);
            RHandIK.enabled = true;
            RaycastHit hit;
            // Layer 6 is wall layer
            if (Physics.Raycast(RShoulder.transform.position, direction, out hit, 0.75f, 1 << 6))
            {
                RLerpTarget = hit.point + HandOffset * -direction;
                RHandIKTarget.transform.position = Vector3.Lerp(RHandIKTarget.transform.position, RLerpTarget, RLerpTime / travelTime);
            }
            RLerpTime += Time.deltaTime;
        }
        if (dot < 0)
        {
            LHandIKTarget.gameObject.SetActive(true);   
            LHandIK.enabled = true;
            RaycastHit hit;
            if (Physics.Raycast(LShoulder.transform.position, direction, out hit, 0.75f, 1 << 6))
            {
                LLerpTarget = hit.point + HandOffset * -direction;
                LHandIKTarget.transform.position = Vector3.Lerp(LHandIKTarget.transform.position, LLerpTarget, LLerpTime / travelTime);
            }
            LLerpTime += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LHandIK.enabled = false;
        RHandIK.enabled = false;
        LHandIKTarget.gameObject.SetActive(false);
        RHandIKTarget.gameObject.SetActive(false);
        RLerpTime = 0;
        LLerpTime = 0;
    }

    private void Update()
    {
        if (!useBody)
        {
            RHandIK.enabled = true;
            LHandIK.enabled = true;
            LHandIKTarget.gameObject.SetActive(true);
            RHandIKTarget.gameObject.SetActive(true);

            LLegIK.enabled = true;
            RLegIK.enabled = true;
            LLegIKTarget.gameObject.SetActive(true);
            RLegIKTarget.gameObject.SetActive(true);

            return;
        }

        if (animator)
            animator.SetBool("Walking", controls.IsWalking);
    }

    void LateUpdate()
    {
        if (!controls.IsWalking)
        {
            armature.localPosition = Vector3.zero;
            Vector3 reposition = new Vector3(0, -Mathf.Abs(LLegIKTarget.localPosition.y - RLegIKTarget.localPosition.y), 0);
            if (Mathf.Abs(reposition.y) > MaxHeightAdjustement)
                reposition.y = 0;

            armature.localPosition = armature.localPosition + reposition;

            LLegIKTarget.gameObject.SetActive(true);
            RLegIKTarget.gameObject.SetActive(true);

            LLegIK.enabled = true;
            RLegIK.enabled = true;

            RaycastHit hit;
            Vector3 offset = transform.up * FootOffset;
            Physics.Raycast(LUpperLeg.transform.position, transform.up * (-1.0f), out hit);
            LLegIKTarget.position = hit.point + offset;

            Physics.Raycast(RUpperLeg.transform.position, transform.up * (-1.0f), out hit);
            RLegIKTarget.position = hit.point + offset;
        }
        else
        {
            armature.localPosition = Vector3.zero;
            LLegIKTarget.gameObject.SetActive(false);
            RLegIKTarget.gameObject.SetActive(false);

            LLegIK.enabled = false;
            RLegIK.enabled = false;

            armature.localPosition = Vector3.zero;
        }
    }
}
