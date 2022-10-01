using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ChessPieceCreator))]
public class ChessGameController : MonoBehaviour
{
    private enum GameState
    {
        Init, Play, Finished
    }


    [SerializeField]
    private BoardLayout m_startBoardLayout;

    [SerializeField]
    private Board m_board;

    private ChessPieceCreator m_pieceCreator;
    private Player m_whitePlayer;
    private Player m_blackPlayer;
    private Player m_activePlayer;

    private GameState m_state;

    public bool IsTeamTurnActive(TeamColor pTeam)
    {
        return m_activePlayer.team == pTeam;
    }

    public void EndTurn()
    {
        GenerateAllPossiblePlayerMoves(m_activePlayer);
        GenerateAllPossiblePlayerMoves(GetOpponentToPlayer(m_activePlayer));

        if (CheckIfGameIsFinished())
        {
            EndGame();
        }
        else
        {
            ChangeActiveTeam();
        }
    }

    private void Awake()
    {
        m_pieceCreator = GetComponent<ChessPieceCreator>();
        CreatePlayers();
    }
    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        SetGameState(GameState.Init);

        CreateChessPiecesFromLayout(m_startBoardLayout);

        m_board.SetController(this);
        m_activePlayer = m_whitePlayer;
        GenerateAllPossiblePlayerMoves(m_activePlayer);
    }

    private void CreatePlayers()
    {
        m_whitePlayer = new Player(TeamColor.White, m_board);
        m_blackPlayer = new Player(TeamColor.Black, m_board);
    }

    private void SetGameState(GameState pState)
    {
        m_state = pState;
    }

    internal bool IsGameInProgress()
    {
        return m_state == GameState.Play;
    }

    private void CreateChessPiecesFromLayout(BoardLayout pBoardLayout)
    {
        for(int x=0; x<pBoardLayout.GetPiecesCount(); x++)
        {
            Vector2Int coords = pBoardLayout.GetSquareCoordsAtIdx(x);
            TeamColor teamColor = pBoardLayout.GetSquareTeamColorAtIdx(x);
            string pieceName = pBoardLayout.GetPieceNameAtIdx(x);

            Type type = Type.GetType(pieceName);
            CreateChessPiece(coords, teamColor, type);
        }
    }

    private void CreateChessPiece(Vector2Int pCoords, TeamColor pTeamColor, Type pType)
    {
        ChessPiece piece = m_pieceCreator.CreateChessPiece(pType).GetComponent<ChessPiece>();
        piece.SetData(pCoords, pTeamColor, m_board);

        Material mat = m_pieceCreator.GetTeamMaterial(pTeamColor);
        piece.SetMaterial(mat);

        m_board.SetPieceOnBoard(pCoords, piece);

        Player currentPlayer = pTeamColor == TeamColor.White ? m_whitePlayer : m_blackPlayer;
        currentPlayer.AddPiece(piece);
    }

    private void GenerateAllPossiblePlayerMoves(Player pPlayer)
    {
        pPlayer.GenerateAllPossibleMoves();
    }

    private void ChangeActiveTeam()
    {
        m_activePlayer = m_activePlayer == m_whitePlayer ? m_blackPlayer : m_whitePlayer;
    }

    private Player GetOpponentToPlayer(Player pPlayer)
    {
        return pPlayer == m_whitePlayer ? m_blackPlayer : m_whitePlayer;
    }

    private bool CheckIfGameIsFinished()
    {
        ChessPiece[] kingAttackingPieces = m_activePlayer.GetPieceAttackingOppositePiceOfType<King>();

        if (kingAttackingPieces.Length > 0)
        {
            Player oppositePlayer = GetOpponentToPlayer(m_activePlayer);
            ChessPiece attackedKing = oppositePlayer.GetPiecesOfType<King>().FirstOrDefault();

            oppositePlayer.RemoveMovesEnablingAttakOnPieceOfType<King>(m_activePlayer, attackedKing);

            int avaliableKingMoves = attackedKing.availableMoves.Count;
            if (avaliableKingMoves == 0)
            {
                bool canCoverKing = oppositePlayer.CanHidePieceFromAttack<King>(m_activePlayer);
                if (!canCoverKing)
                    return true;
            }
        }
        return false;
    }

    private void EndGame()
    {
        SetGameState(GameState.Finished);
    }

    private void DestroyPieces()
    {
        m_whitePlayer.activePieces.ForEach(p => Destroy(p.gameObject));
        m_blackPlayer.activePieces.ForEach(p => Destroy(p.gameObject));
    }

    public void OnPieceRemoved(ChessPiece piece)
    {
        Player pieceOwner = (piece.teamColor == TeamColor.White) ? m_whitePlayer : m_blackPlayer;
        pieceOwner.RemovePiece(piece);
    }

    public void RemoveMovesEnablingAttakOnPieceOfType<T>(ChessPiece piece) where T : ChessPiece
    {
        m_activePlayer.RemoveMovesEnablingAttakOnPieceOfType<T>(GetOpponentToPlayer(m_activePlayer), piece);
    }

}
