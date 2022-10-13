using UnityEngine;

public class JCJwelleryItemProperty : MonoBehaviour
{
    [SerializeField] private bool isReal;
    [SerializeField] private int itemBlinkingRange;

    public int ItemBlinkingRange => itemBlinkingRange;

    public bool IsReal => isReal;
}
