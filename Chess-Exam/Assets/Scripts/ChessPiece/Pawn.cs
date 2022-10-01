﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();

        Vector2Int direction = teamColor == TeamColor.White ? Vector2Int.up : Vector2Int.down;

        float range = hasMove ? 1 : 2;

        for (int i = 1; i <= range; i++)
        {
            Vector2Int nextCoords = occupiedSquare + direction * i;
            ChessPiece piece = board.GetPieceOnSquare(nextCoords);

            if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                break;
            if (piece == null)
                TryToAddMove(nextCoords);
            else
                break;
        }

        Vector2Int[] takeDirections = new Vector2Int[] { new Vector2Int(1, direction.y), new Vector2Int(-1, direction.y) };
        for (int i = 0; i < takeDirections.Length; i++)
        {
            Vector2Int nextCoords = occupiedSquare + takeDirections[i];
            ChessPiece piece = board.GetPieceOnSquare(nextCoords);

            if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                continue;
            if (piece != null && !piece.IsFromSameTeam(this))
            {
                TryToAddMove(nextCoords);
            }
        }
        return availableMoves;
    }
}
