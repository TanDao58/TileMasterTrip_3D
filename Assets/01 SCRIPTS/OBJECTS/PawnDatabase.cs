using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pawn
{
    public Sprite _pawn_img;
}

[CreateAssetMenu(menuName = "Data/List Pawn Image", fileName = "PawnImageDatabase")]
public class PawnDatabase : ScriptableObject
{
    public Pawn[] _list_pawn;

    public void CreateListPawn(int length)
    {
        _list_pawn = new Pawn[length];
        for (int i = 0; i < length; i++)
        {
            _list_pawn[i] = new Pawn();
        }
    }

    public Pawn GetPawnInPos(int position)
    {
        return _list_pawn[position];
    }
    public int GetListLength()
    {
        return _list_pawn.Length;
    }
}
