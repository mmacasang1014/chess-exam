using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public Board board { get; set; }
    public Vector2Int occupiedSquare { get; set; }

    public TeamColor teamColor { get; set; }

    public bool hasMove { get; set; }

    public List<Vector2Int> availableMoves;

    private MaterialSetter m_materialSetter;

    private IObjectTweener m_tweener;

    public abstract List<Vector2Int> SelectAvailableSquares();

    private void Awake()
    {
        hasMove = false;
        availableMoves = new List<Vector2Int>();
        m_tweener = GetComponent<IObjectTweener>();
        m_materialSetter = GetComponent<MaterialSetter>();
    }

    public void SetMaterial(Material pMaterial)
    {
        if (m_materialSetter == null)
            m_materialSetter = GetComponent<MaterialSetter>();

        m_materialSetter.SetSingleMaterial(pMaterial);
    }

    public bool IsFromSameTeam(ChessPiece pPiece)
    {
        return teamColor == pPiece.teamColor;
    }

    public bool CanMoveTo(Vector2Int pCoords)
    {
        return availableMoves.Contains(pCoords);
    }

    public virtual void MovePiece(Vector2Int pCoords)
    {
        Vector3 targetPosition = board.CalculatePositionFromCoords(pCoords);
        occupiedSquare = pCoords;
        hasMove = true;
        m_tweener.MoveTo(transform, targetPosition);
    }

    public void SetData(Vector2Int pCoords, TeamColor pTeamColor, Board pBoard)
    {
        occupiedSquare = pCoords;
        teamColor = pTeamColor;
        board = pBoard;

        transform.position = board.CalculatePositionFromCoords(pCoords);
    }

    public bool IsAttackingPieceOfType<T>() where T : ChessPiece
    {
        foreach (var square in availableMoves)
        {
            if (board.GetPieceOnSquare(square) is T)
                return true;
        }
        return false;
    }

    protected void TryToAddMove(Vector2Int coords)
    {
        availableMoves.Add(coords);
    }

    

}
