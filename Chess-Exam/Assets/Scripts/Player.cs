using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Player
{
    public TeamColor team { get; set; }
    public Board board { get; set; }
    public List<ChessPiece> activePieces { get; set; }

	public Player(TeamColor pTeam, Board pBoard)
	{
		activePieces = new List<ChessPiece>();
		board = pBoard;
		team = pTeam;
	}

	public void AddPiece(ChessPiece pPiece)
	{
		if (!activePieces.Contains(pPiece))
			activePieces.Add(pPiece);
	}

	public void RemovePiece(ChessPiece pPiece)
	{
		if (activePieces.Contains(pPiece))
			activePieces.Remove(pPiece);
	}

	public void GenerateAllPossibleMoves()
	{
		foreach (var piece in activePieces)
		{
			if (board.HasPiece(piece))
				piece.SelectAvailableSquares();
		}
	}
	public ChessPiece[] GetPieceAttackingOppositePiceOfType<T>() where T : ChessPiece
	{
		return activePieces.Where(p => p.IsAttackingPieceOfType<T>()).ToArray();
	}

	public ChessPiece[] GetPiecesOfType<T>() where T : ChessPiece
	{
		return activePieces.Where(p => p is T).ToArray();
	}

	public void RemoveMovesEnablingAttakOnPieceOfType<T>(Player pOpponent, ChessPiece selectedPiece) where T : ChessPiece
	{
		List<Vector2Int> coordsToRemove = new List<Vector2Int>();

		coordsToRemove.Clear();
		foreach (var coords in selectedPiece.availableMoves)
		{
			ChessPiece pieceOnCoords = board.GetPieceOnSquare(coords);
			board.UpdateBoardOnPieceMove(coords, selectedPiece.occupiedSquare, selectedPiece, null);

			pOpponent.GenerateAllPossibleMoves();

			if (pOpponent.CheckIfIsAttackingPiece<T>())
				coordsToRemove.Add(coords);

			board.UpdateBoardOnPieceMove(selectedPiece.occupiedSquare, coords, selectedPiece, pieceOnCoords);
		}
		foreach (var coords in coordsToRemove)
		{
			selectedPiece.availableMoves.Remove(coords);
		}

	}

	private bool CheckIfIsAttackingPiece<T>() where T : ChessPiece
	{
		foreach (var piece in activePieces)
		{
			if (board.HasPiece(piece) && piece.IsAttackingPieceOfType<T>())
				return true;
		}
		return false;
	}

	public bool CanHidePieceFromAttack<T>(Player pOpponent) where T : ChessPiece
	{
		foreach (var piece in activePieces)
		{
			foreach (var coords in piece.availableMoves)
			{
				ChessPiece pieceOnCoords = board.GetPieceOnSquare(coords);
				board.UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);

				pOpponent.GenerateAllPossibleMoves();

				if (!pOpponent.CheckIfIsAttackingPiece<T>())
				{
					board.UpdateBoardOnPieceMove(piece.occupiedSquare, coords, piece, pieceOnCoords);
					return true;
				}

				board.UpdateBoardOnPieceMove(piece.occupiedSquare, coords, piece, pieceOnCoords);
			}
		}
		return false;
	}

	private void OnGameRestarted()
	{
		activePieces.Clear();
	}
}
