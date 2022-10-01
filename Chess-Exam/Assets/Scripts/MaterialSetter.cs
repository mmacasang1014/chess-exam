using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialSetter : MonoBehaviour
{
    private MeshRenderer m_meshRenderer;

	private MeshRenderer meshRenderer
	{
		get
		{
			if (m_meshRenderer == null)
				m_meshRenderer = GetComponent<MeshRenderer>();
			return m_meshRenderer;
		}
	}

	public void SetSingleMaterial(Material pMaterial)
	{
		meshRenderer.material = pMaterial;
	}
}
