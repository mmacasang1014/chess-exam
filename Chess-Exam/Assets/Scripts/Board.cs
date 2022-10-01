using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;

    [SerializeField] 
    private Transform m_bottomLeftSquareTransform;

    [SerializeField] 
    private float m_squareSize;

    [SerializeField]
    private GameObject m_gridDebugger;

    private ChessPiece[,] m_grid;
    private ChessPiece m_selectedPiece;
    private ChessGameController m_chessController;

    public void SetController(ChessGameController pChessController)
    {
        m_chessController = pChessController;
    }

    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return m_bottomLeftSquareTransform.position + new Vector3(coords.x * m_squareSize, 0f, coords.y * m_squareSize);
    }

    public void OnSquareSelected(Vector3 inputPosition)
    {
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        ChessPiece piece = GetPieceOnSquare(coords);
        if(piece != null)
        {
            Debug.LogWarning(piece.GetType().ToString() + coords);
        }
        else
        {
            Debug.LogWarning(coords);
        }

        if (m_selectedPiece)
        {
            if (piece != null && m_selectedPiece == piece)
                DeselectPiece();
            else if (piece != null && m_selectedPiece != piece && m_chessController.IsTeamTurnActive(piece.teamColor))
                SelectPiece(piece);
            else if (m_selectedPiece.CanMoveTo(coords))
                OnSelectedPieceMoved(coords, m_selectedPiece);
        }
        else
        {
            if (piece != null && m_chessController.IsTeamTurnActive(piece.teamColor))
                SelectPiece(piece);
        }
    }

    public ChessPiece GetPieceOnSquare(Vector2Int pCoords)
    {
        if (CheckIfCoordinatesAreOnBoard(pCoords))
            return m_grid[pCoords.x, pCoords.y];

        return null;
    }

    public bool CheckIfCoordinatesAreOnBoard(Vector2Int pCoords)
    {
        if (pCoords.x < 0 || pCoords.y < 0 || pCoords.x >= BOARD_SIZE || pCoords.y >= BOARD_SIZE)
            return false;

        return true;
    }

    public bool HasPiece(ChessPiece pPiece)
    {
        for (int x = 0; x < BOARD_SIZE; x++)
        {
            for (int y = 0; y < BOARD_SIZE; y++)
            {
                if (m_grid[x, y] == pPiece)
                    return true;
            }
        }
        return false;
    }

    public void SetPieceOnBoard(Vector2Int pCoords, ChessPiece pPiece)
    {
        if (CheckIfCoordinatesAreOnBoard(pCoords))
        {
            m_grid[pCoords.x, pCoords.y] = pPiece;

            m_gridDebugger.transform.position = CalculatePositionFromCoords(pCoords);

            Instantiate(m_gridDebugger);

            Debug.LogWarning(pCoords);
        }
        else
        {
            Debug.LogError("Coords outside of Board" + pPiece.GetType().ToString());
        }
    }

    public void UpdateBoardOnPieceMove(Vector2Int pNewCoords, Vector2Int pOldCoords, ChessPiece pNewPiece, ChessPiece pOldPiece)
    {
        m_grid[pOldCoords.x, pOldCoords.y] = pOldPiece;
        m_grid[pNewCoords.x, pNewCoords.y] = pNewPiece;
    }

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        m_grid = new ChessPiece[BOARD_SIZE, BOARD_SIZE];
    }

    private Vector2Int CalculateCoordsFromPosition(Vector3 pInputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(pInputPosition).x / m_squareSize) + BOARD_SIZE / 2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(pInputPosition).z / m_squareSize) + BOARD_SIZE / 2;
        return new Vector2Int(x, y);
    }

    private void SelectPiece(ChessPiece pPiece)
    {
        m_chessController.RemoveMovesEnablingAttakOnPieceOfType<King>(pPiece);
        m_selectedPiece = pPiece;

        List<Vector2Int> selection = m_selectedPiece.availableMoves;
    }

    private void DeselectPiece()
    {
        m_selectedPiece = null;
    }

    private void OnSelectedPieceMoved(Vector2Int pCoords, ChessPiece pPiece)
    {
        TryToTakeOppositePiece(pCoords);
        UpdateBoardOnPieceMove(pCoords, pPiece.occupiedSquare, pPiece, null);

        m_selectedPiece.MovePiece(pCoords);

        DeselectPiece();
        EndTurn();
    }

    private void EndTurn()
    {
        m_chessController.EndTurn();
    }

    private void TryToTakeOppositePiece(Vector2Int pCoords)
    {
        ChessPiece piece = GetPieceOnSquare(pCoords);
        if (piece && !m_selectedPiece.IsFromSameTeam(piece))
        {
            TakePiece(piece);
        }
    }

    private void TakePiece(ChessPiece pPiece)
    {
        if (pPiece)
        {
            m_grid[pPiece.occupiedSquare.x, pPiece.occupiedSquare.y] = null;
            m_chessController.OnPieceRemoved(pPiece);
            Destroy(pPiece.gameObject);
        }
    }

}
