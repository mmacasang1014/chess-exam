using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
	[SerializeField] 
	private Material m_freeSquareMaterial;

	[SerializeField] 
	private Material m_enemySquareMaterial;

	[SerializeField] 
	private GameObject m_selectorPrefab;

	private List<GameObject> m_instantiatedSelectors = new List<GameObject>();

	public void ShowSelection(Dictionary<Vector3, bool> pSquareData)
	{
		ClearSelection();
		foreach (var data in pSquareData)
		{
			GameObject selector = Instantiate(m_selectorPrefab, data.Key, Quaternion.identity);
			m_instantiatedSelectors.Add(selector);
			foreach (var setter in selector.GetComponentsInChildren<MaterialSetter>())
			{
				setter.SetSingleMaterial(data.Value ? m_freeSquareMaterial : m_enemySquareMaterial);
			}
		}
	}

	public void ClearSelection()
	{
		for (int i = 0; i < m_instantiatedSelectors.Count; i++)
		{
			Destroy(m_instantiatedSelectors[i]);
		}
	}
}
