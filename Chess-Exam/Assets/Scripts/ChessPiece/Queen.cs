using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    private Vector2Int[] m_directions = new Vector2Int[]
    {
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1,- 1),
    };

    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();

        float range = Board.BOARD_SIZE;
        foreach (var direction in m_directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = occupiedSquare + direction * i;
                ChessPiece piece = board.GetPieceOnSquare(nextCoords);

                if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                    break;

                if (piece == null)
                {
                    TryToAddMove(nextCoords);
                }
                else if (!piece.IsFromSameTeam(this))
                {
                    TryToAddMove(nextCoords);
                    break;
                }
                else if (piece.IsFromSameTeam(this))
                {
                    break;
                }
            }
        }
        return availableMoves;
    }
}
