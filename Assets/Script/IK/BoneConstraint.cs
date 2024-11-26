using UnityEngine;

public class BoneConstraint : MonoBehaviour
{
    private Vector3 baseEuler;
    public Vector3 BaseEuler { get { return baseEuler; } }

    //Those are liberty degrees for the LOCAL rotation
    [Range(-180f, 180f)]
    public float XMin = -180f, XMax = 180f;

    //Those are liberty degrees for the LOCAL rotation
    [Range(-180f, 180f)]
    public float YMin = -180f, YMax = 180f;

    //Those are liberty degrees for the LOCAL rotation
    [Range(-180f, 180f)]
    public float ZMin = -180f, ZMax = 180f;

    private void Start()
    {
        baseEuler = transform.localEulerAngles;
    }
}
