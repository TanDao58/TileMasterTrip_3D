using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPawnable : MonoBehaviour
{
    public Pawn CurrentPawn { get; private set; }
    public bool NotMouseClick { get; set; }
    public bool NotRotate { get; set; }
    public bool StopCheck { get; protected set; }
    public abstract void MovePawn(Vector3 _finishPos);
    public abstract float CalculateAnglePawntoTable(Vector3 _TablePos);

}
