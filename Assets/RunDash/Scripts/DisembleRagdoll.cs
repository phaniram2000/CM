using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisembleRagdoll : MonoBehaviour
{
    private RagdollDismembermentVisual dismemberment;
    private CharacterJoint joint;
    private Rigidbody Character;

    private void Awake()
    {
        Character = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        dismemberment = GetComponentInParent<RagdollDismembermentVisual>();
        joint = GetComponent<CharacterJoint>();
        joint.breakForce = 0;
    }

    private void OnJointBreak(float breakForce)
    {
        dismemberment.Dismember("Head");
        dismemberment.Dismember("LeftHip");
        dismemberment.Dismember("RightHip");
        dismemberment.Dismember("LeftLeg");
        dismemberment.Dismember("RightLeg");
        dismemberment.Dismember("Spine");
        dismemberment.Dismember("LeftArm");
        dismemberment.Dismember("LeftElbow");
        dismemberment.Dismember("RightArm");
        dismemberment.Dismember("RightElbow");
        Character.AddForce(new Vector3(0, 1, 1) * 15, ForceMode.Impulse);
    }
}
