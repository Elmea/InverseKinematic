using System;
using System.Collections.Generic;
using UnityEngine;


public class InverseKinematic : MonoBehaviour
{
    enum IKAlgorithm
    {
        CCD
    }

    [SerializeField] Transform EndEffector;
    [SerializeField] Transform IKTarget;
    [SerializeField] IKAlgorithm algorithm;
    [SerializeField] int chainLenght = 1;
    [SerializeField] int iteration;
    [SerializeField] bool useTargetRotation;

    private bool isChainInitilazed;
    Action selectedAlgorithm;

    List<Transform> IKChain;
    List<BoneConstraint> Constraints;

    #region Initializing Methods
    void InitIKChain()
    {
        Transform curr = this.gameObject.transform;
        IKChain = new List<Transform>();
        Constraints = new List<BoneConstraint>();
        for (int i = 0; i < chainLenght; i++)
        {
            IKChain.Add(curr);
            Constraints.Add(curr.GetComponent<BoneConstraint>());
            curr = curr.transform.parent;

            if (curr == null)
                break;
        }

        isChainInitilazed = true;
    }

    private void SelectMethod()
    {
        switch (algorithm)
        {
            case IKAlgorithm.CCD:
                selectedAlgorithm = CCDAlgorithm;
                break;
            
            default:
                selectedAlgorithm = emptyMethod;
                break;
        }
    }
    #endregion

    void ApplyConstraint(Transform bone, BoneConstraint constraint)
    {
        Vector3 eulerAngle = bone.localRotation.eulerAngles;
        // Using (eulerAngle + 180) % 360 - 180 to convert from range [0, 360] to range [-180, 180]
        bone.localRotation = Quaternion.Euler(Mathf.Clamp((eulerAngle.x + 180) % 360 - 180, constraint.XMin + constraint.BaseEuler.x, constraint.XMax + constraint.BaseEuler.x),
                                                Mathf.Clamp((eulerAngle.y + 180) % 360 - 180, constraint.YMin + constraint.BaseEuler.y, constraint.YMax + constraint.BaseEuler.y),
                                                Mathf.Clamp((eulerAngle.z + 180) % 360 - 180, constraint.ZMin + constraint.BaseEuler.z, constraint.ZMax + constraint.BaseEuler.z));
    }

    #region IKAlgo
    void emptyMethod()
    {
        return;
    }

    void CCDAlgorithm()
    {   
        for (int i = 0; i < iteration; i++) 
        { 
            for (int b = 0; b < chainLenght; b++)
            {
                if (b == 0 && useTargetRotation)
                {
                    Quaternion rotation = IKTarget.rotation;
                    IKChain[b].rotation = IKChain[b].rotation * rotation;
                }
                else
                {
                    Quaternion rotation = Quaternion.FromToRotation(EndEffector.position - IKChain[b].position, IKTarget.position - IKChain[b].position);
                    IKChain[b].rotation = rotation * IKChain[b].rotation;
                }
                
                if (Constraints[b] != null)
                    ApplyConstraint(IKChain[b].transform, Constraints[b]);
            }
        } 
    }
    
    public void CalcIK()
    {
        if (!isChainInitilazed)
            return;

        SelectMethod();
        selectedAlgorithm();
    }
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        InitIKChain();
        SelectMethod();
    }


    void LateUpdate()
    {
        CalcIK();
    }
    #endregion
}
