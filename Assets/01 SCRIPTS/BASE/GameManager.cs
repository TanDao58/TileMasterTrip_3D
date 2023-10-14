using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArray
{
    public int[] _count_pawn_in_pos = new int[100];
    public int[] _start_pos_type_pawn = new int[100];
}

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform _finish_parent_trans;
    private readonly CheckArray _check_array = new CheckArray();
    private IPawnable[] _list_Pos_inWinPos;
    int _countPawninWinPos = 0;

    IPawnable pawnable, pawnable_switching;
    private bool NotMouseClick;
    private bool NotRotate;
    Transform _win_pos_move, _switching_pos_move;

    private void Start()
    {
        //Setup android
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //Setup check array
        _list_Pos_inWinPos = new IPawnable[_finish_parent_trans.childCount];

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckClickPawn(Input.mousePosition);
        }
        if (pawnable_switching != null && pawnable_switching.IsSwitchingPos)
        {
            if (Vector3.Distance(pawnable_switching.transform.position, _switching_pos_move.position) > 0.01f)
                pawnable_switching.MovePawn(_switching_pos_move.position);
            else
                pawnable_switching.IsSwitchingPos = false;
        }
        if (NotMouseClick)
        {
            if (pawnable != null && !pawnable.IsInWinPos)
            {
                if (Vector3.Distance(pawnable.transform.position, _win_pos_move.position) > 0.01f)
                {
                    MovePawn(pawnable, _win_pos_move.position);
                }
                else
                {
                    ScalePawn(pawnable, 1.25f);
                    pawnable.IsInWinPos = true;
                    NotMouseClick = false;
                }
            }

        }


    }

    public void CheckClickPawn(Vector3 _checkPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(_checkPos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, LayerMask.GetMask("Pawn")))
        {
            pawnable = raycastHit.collider.GetComponent<IPawnable>();
            NumAddPawnToWinPos(pawnable);


            NotRotate = true;
            NotMouseClick = true;
            // setup pawn move
            raycastHit.collider.GetComponent<Rigidbody>().isKinematic = true;
            raycastHit.collider.enabled = false;

        }
    }
    private void LateUpdate()
    {
        if (NotRotate)
        {
            RotatePawn(pawnable, _win_pos_move.position);
        }
    }
    public void MovePawn(IPawnable pawnable, Vector3 finishpos) => pawnable.MovePawn(finishpos);
    public void ScalePawn(IPawnable pawnable, float scale) => pawnable.ScalePawn(scale);
    public void RotatePawn(IPawnable pawnable, Vector3 pos) => pawnable.RotatePawn(pos);
    public void NumAddPawnToWinPos(IPawnable pawnable)
    {
        if (_check_array._count_pawn_in_pos[pawnable.PawnID] == 0)
        {
            _list_Pos_inWinPos[_countPawninWinPos] = pawnable;
            _check_array._start_pos_type_pawn[pawnable.PawnID] = _countPawninWinPos;
        }
        else
        {
            int _sum_count_before_pawn = 0;
            for (int i = 0; i < _check_array._count_pawn_in_pos.Length; i++)
            {
                if (_check_array._start_pos_type_pawn[i] < _check_array._start_pos_type_pawn[pawnable.PawnID])
                    _sum_count_before_pawn += _check_array._count_pawn_in_pos[i];
            }
            Debug.Log(_sum_count_before_pawn);
            for (int i = _list_Pos_inWinPos.Length - 1; i > _check_array._start_pos_type_pawn[pawnable.PawnID] + _check_array._count_pawn_in_pos[pawnable.PawnID] + _sum_count_before_pawn; i--)
            {
                if (_list_Pos_inWinPos[i] != null)
                {
                    //Thua
                    return;
                }
                else
                {
                    if (_list_Pos_inWinPos[i - 1] != null)
                    {
                        _list_Pos_inWinPos[i] = _list_Pos_inWinPos[i - 1];
                        pawnable_switching = _list_Pos_inWinPos[i - 1];
                        _switching_pos_move = _finish_parent_trans.GetChild(i);
                        pawnable_switching.IsSwitchingPos = true;
                    }
                }
            }
            _list_Pos_inWinPos[_check_array._start_pos_type_pawn[pawnable.PawnID] + _check_array._count_pawn_in_pos[pawnable.PawnID]] = pawnable;
            for (int i = 0; i < _check_array._start_pos_type_pawn.Length; i++)
            {
                if (_check_array._start_pos_type_pawn[i] != 0 && _check_array._start_pos_type_pawn[i] > _check_array._start_pos_type_pawn[pawnable.PawnID])
                    _check_array._start_pos_type_pawn[i]++;
            }
        }
        _win_pos_move = _finish_parent_trans.GetChild(_check_array._start_pos_type_pawn[pawnable.PawnID] + _check_array._count_pawn_in_pos[pawnable.PawnID]);
        _check_array._count_pawn_in_pos[pawnable.PawnID]++;
        _countPawninWinPos++;
    }

}
