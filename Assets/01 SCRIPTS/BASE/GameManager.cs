using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArray
{
    public int[] _count_pawn_in_pos = new int[100];
    public int[] _start_pos_type_pawn = new int[100];

    public IPawnable[] _list_pawn_switching = new IPawnable[100];
    public Transform[] _list_pos_switching = new Transform[100];
}

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform _finish_parent_trans;
    private readonly CheckArray _check_array = new CheckArray();
    private IPawnable[] _list_Pos_inWinPos;
    int _countPawninWinPos = 0;

    IPawnable pawnable;

    private bool NotMouseClick;
    private bool NotRotate;
    Transform _win_pos_move;

    private bool IsLoseGame = false;
    private void Start()
    {
        //Setup android
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //Setup check array
        _list_Pos_inWinPos = new IPawnable[_finish_parent_trans.childCount - 1];
    }

    private void Update()
    {
        if (!IsLoseGame)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckClickPawn(Input.mousePosition);
            }
            for (int i = 0; i < _check_array._list_pawn_switching.Length; i++)
            {
                if (_check_array._list_pawn_switching[i] != null && _check_array._list_pawn_switching[i].IsSwitchingPos)
                {
                    if (Vector3.Distance(_check_array._list_pawn_switching[i].transform.position, _check_array._list_pos_switching[i].position) > 0.01f)
                        _check_array._list_pawn_switching[i].MovePawn(_check_array._list_pos_switching[i].position);
                    else
                    {
                        _check_array._list_pawn_switching[i].IsSwitchingPos = false;
                        _check_array._list_pawn_switching[i] = null;
                        _check_array._list_pos_switching[i] = null;
                    }
                }
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
                        ScalePawn(pawnable, 2f);
                        pawnable.IsInWinPos = true;
                        NotMouseClick = false;
                    }
                }

            }
        }
    }

    public void CheckClickPawn(Vector3 _checkPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(_checkPos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, LayerMask.GetMask("Pawn")))
        {
            if (_list_Pos_inWinPos[_list_Pos_inWinPos.Length - 1] == null)
            {
                pawnable = raycastHit.collider.transform.GetComponentInChildren<IPawnable>();
                NumAddPawnToWinPos(pawnable);
            }
            else
            {
                IsLoseGame = true;
                return;
            }

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
            for (int i = _list_Pos_inWinPos.Length - 1; i > _check_array._start_pos_type_pawn[pawnable.PawnID] + _check_array._count_pawn_in_pos[pawnable.PawnID]; i--)
            {
                if (_list_Pos_inWinPos[i] != null)
                {
                    return;
                }
                else
                {
                    if (_list_Pos_inWinPos[i - 1] != null)
                    {
                        _list_Pos_inWinPos[i] = _list_Pos_inWinPos[i - 1];

                        _check_array._list_pawn_switching[i - 1] = _list_Pos_inWinPos[i - 1];
                        _check_array._list_pos_switching[i - 1] = _finish_parent_trans.GetChild(i);
                        _list_Pos_inWinPos[i - 1].IsSwitchingPos = true;
                        _list_Pos_inWinPos[i - 1] = null;
                    }
                }
            }
            _list_Pos_inWinPos[_check_array._start_pos_type_pawn[pawnable.PawnID] + _check_array._count_pawn_in_pos[pawnable.PawnID]] = pawnable;
            for (int i = 0; i < _check_array._start_pos_type_pawn.Length; i++)
            {
                if (_check_array._start_pos_type_pawn[i] != 0 && _check_array._start_pos_type_pawn[i] > _check_array._start_pos_type_pawn[pawnable.PawnID])
                {
                    _check_array._start_pos_type_pawn[i]++;
                }
            }
        }
        _win_pos_move = _finish_parent_trans.GetChild(_check_array._start_pos_type_pawn[pawnable.PawnID] + _check_array._count_pawn_in_pos[pawnable.PawnID]);
        _check_array._count_pawn_in_pos[pawnable.PawnID]++;
        _countPawninWinPos++;
    }
}
