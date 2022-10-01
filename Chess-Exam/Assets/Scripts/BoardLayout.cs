using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Board/Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]
    public class BoardSquareSetup
    {
        public Vector2Int position;
        public PieceType pieceType;
        public TeamColor teamColor;
    }

    [SerializeField]
    private BoardSquareSetup[] m_boardSquareSetup;

    public int GetPiecesCount()
    {
        return m_boardSquareSetup.Length;
    }

    public Vector2Int GetSquareCoordsAtIdx(int pIdx)
    {
        if(pIdx >= m_boardSquareSetup.Length)
        {
            return new Vector2Int(-1, -1);
        }

        return new Vector2Int(m_boardSquareSetup[pIdx].position.x - 1, m_boardSquareSetup[pIdx].position.y - 1);
    }

    public string GetPieceNameAtIdx(int pIdx)
    {
        if (pIdx >= m_boardSquareSetup.Length)
        {
            return string.Empty;
        }

        return m_boardSquareSetup[pIdx].pieceType.ToString();
    }

    public TeamColor GetSquareTeamColorAtIdx(int pIdx)
    {
        if (pIdx >= m_boardSquareSetup.Length)
        {
            return TeamColor.Black;
        }

        return m_boardSquareSetup[pIdx].teamColor;
    }
}
