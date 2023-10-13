using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float _radius_check;
    private void Awake()
    {
        //Setup android
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            CheckClickPawn(Camera.main.ScreenToWorldPoint(Input.mousePosition), _radius_check);
    }
    public void CheckClickPawn(Vector3 _checkPos, float radius)
    {
        Vector3 _convertMouseToCurrent = new Vector3(_checkPos.x, this.transform.position.y, _checkPos.z);
        Collider[] _checkCol = Physics.OverlapSphere(_convertMouseToCurrent, radius, LayerMask.GetMask("Pawn"));
        if( _checkCol.Length > 0)
        {
            IPawnable pawnable = _checkCol[0].GetComponent<IPawnable>();
            _checkCol[0].GetComponent<Rigidbody>().isKinematic = true;
            pawnable.NotMouseClick = true;
            pawnable.NotRotate = true;
        }
    }
}
