using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPawnable : MonoBehaviour
{
    public bool NotMouseClick { get; set; }
    public bool NotRotate { get; set; }
    public abstract void MovePawn(Vector3 _finishPos);

}
