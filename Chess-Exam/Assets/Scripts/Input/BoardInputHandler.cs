using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Board))]
public class BoardInputHandler : MonoBehaviour, IInputHandler
{
    private Board m_board;
    public void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action onClick)
    {
        m_board.OnSquareSelected(inputPosition);
    }

    private void Awake()
    {
        m_board = GetComponent<Board>();
    }
}
