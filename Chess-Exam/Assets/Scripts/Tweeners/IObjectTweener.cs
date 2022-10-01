using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal interface IObjectTweener
{
	void MoveTo(Transform pTransform, Vector3 pTargetPosition);
}
