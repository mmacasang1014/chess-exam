using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantTweener : MonoBehaviour, IObjectTweener
{
    public void MoveTo(Transform pTransform, Vector3 pTargetPosition)
    {
        pTransform.position = pTargetPosition;
    }
}
