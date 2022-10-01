using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] chessPiecePrefabs;

    [SerializeField] 
    private Material blackMaterial;

    [SerializeField] 
    private Material whiteMaterial;

    private Dictionary<string, GameObject> chessPieceDict;

    private void Awake()
    {
        chessPieceDict = new Dictionary<string, GameObject>();

        foreach(var chessPiece in chessPiecePrefabs)
        {
            chessPieceDict.Add(chessPiece.GetComponent<ChessPiece>().GetType().ToString(), chessPiece);
        }
    }

    public GameObject CreateChessPiece(Type pType)
    {
        if(chessPieceDict.ContainsKey(pType.ToString()))
        {
            GameObject piece = Instantiate(chessPieceDict[pType.ToString()]);
            return piece;
        }

        return null;
    }

    public Material GetTeamMaterial(TeamColor pTeamColor)
    {
        return pTeamColor == TeamColor.White ? whiteMaterial : blackMaterial;
    }

}
