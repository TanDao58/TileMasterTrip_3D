using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPawnable : MonoBehaviour
{
    public int PawnID { get; set; }
    public bool IsInWinPos { get; set; }
    public bool IsSwitchingPos { get; set; }
    public abstract void MovePawn(Vector3 _finishPos);
    public abstract void ScalePawn(float value);
    public abstract void RotatePawn(Vector3 _pos);
    public abstract float CalculateAnglePawntoTable(Vector3 _TablePos);

}
