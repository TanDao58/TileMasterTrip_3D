using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Queue<Pawn> pawns = new Queue<Pawn>(8);

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
            CheckClickPawn(Input.mousePosition);
    }

    public void CheckClickPawn(Vector3 _checkPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(_checkPos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, LayerMask.GetMask("Pawn")))
        {
            IPawnable pawnable = raycastHit.collider.GetComponent<IPawnable>();
            pawns.Enqueue(pawnable.CurrentPawn);
            
            // setup pawn move
            raycastHit.collider.GetComponent<Rigidbody>().isKinematic = true;
            pawnable.NotMouseClick = true;
            pawnable.NotRotate = true;
        }
    }


}
