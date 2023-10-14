using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pawn
{
    public Sprite _pawn_img;
    public int _pawn_id;
}

[CreateAssetMenu(menuName = "Data/List Pawn Image", fileName = "PawnImageDatabase")]
public class PawnDatabase : ScriptableObject
{
    public Pawn[] _list_pawn;
    public int GetListLength()
    {
        return _list_pawn.Length;
    }
    public void AutoAddIDPawn()
    {
        for (int i = 0; i < _list_pawn.Length; i++)
        {
            _list_pawn[i]._pawn_id = i;
        }
    }
}
