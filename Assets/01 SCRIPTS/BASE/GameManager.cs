using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckArray
{
    public int[] _count_pawn_in_pos;
    public int[] _start_pos_type_pawn;

    public IPawnable[] _list_pawn_switching;
    public Transform[] _list_pos_switching;
}

public class GameManager : MonoBehaviour
{
    [Header("Stars Setup")]
    [SerializeField] UnityEngine.UI.Text _StarText;
    [SerializeField] Transform _StarText_pos;
    [SerializeField] GameObject _Star_prefabs;
    [SerializeField] int _star_count;

    [Space(20)]
    [SerializeField] Transform _finish_parent_trans;
    [SerializeField] AutoSpawnLevel _spawn_level;

    private CheckArray _check_array;
    private IPawnable[] _list_Pos_inWinPos;
    int _countPawninWinPos = 0;

    IPawnable pawnable;

    private bool IsMouseStartHold = false;
    IPawnable previous_pawnable;

    private bool NotMouseClick;
    Transform _win_pos_move;

    private bool IsCalculateScore = false;
    private int _count_delete_objects = 0;

    private bool _ready_lose_game = false;
    private void Start()
    {
        //Setup android
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //Setup check array
        _list_Pos_inWinPos = new IPawnable[_finish_parent_trans.childCount - 1];
        _check_array = new CheckArray
        {
            _count_pawn_in_pos = new int[_spawn_level.GetLengthSprites()],
            _start_pos_type_pawn = new int[_spawn_level.GetLengthSprites()],
            _list_pawn_switching = new IPawnable[_spawn_level.GetLengthSprites()],
            _list_pos_switching = new Transform[_spawn_level.GetLengthSprites()]
        };
        AnimateCoins.Instance.PrepareCoins(_Star_prefabs, _star_count);
    }
    private void Update()
    {
        if (_count_delete_objects != 0 && _count_delete_objects == ScoreManager.Instance.count_pawn_in_game)
        {
            ScoreManager.Instance.IsWinGame = true;
            return;
        }
        if (!ScoreManager.Instance.IsLoseGame)
        {
            if (Input.GetMouseButtonDown(0))
                IsMouseStartHold = true;
            if (IsMouseStartHold)
                PlaySoundWhenPawnHold();
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
            for (int i = 0; i < _check_array._list_pawn_switching.Length; i++)
            {
                if (_check_array._list_pawn_switching[i] != null && _check_array._list_pawn_switching[i].IsSwitchingPos)
                    return;
            }
            if (Input.GetMouseButtonUp(0) && !IsCalculateScore)
            {
                CheckClickPawn(Input.mousePosition);
                IsMouseStartHold = false;
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
            else
            {
                if (_ready_lose_game)
                    ScoreManager.Instance.IsLoseGame = true;
            }
        }
    }
    public void PlaySoundWhenPawnHold()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, LayerMask.GetMask("Pawn")))
        {
            IPawnable temp = raycastHit.collider.transform.GetComponentInChildren<IPawnable>();
            if (temp != null && previous_pawnable != temp)
            {
                SoundManager.Instance.PlaySound("click_pawn");
                previous_pawnable = temp;
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
                if (_check_array._count_pawn_in_pos[pawnable.PawnID] < 2 && _list_Pos_inWinPos[_list_Pos_inWinPos.Length - 2] != null)
                    _ready_lose_game = true;
                NumAddPawnToWinPos(pawnable);
            }
            NotMouseClick = true;
            // setup pawn move
            raycastHit.collider.GetComponent<Rigidbody>().isKinematic = true;
            raycastHit.collider.enabled = false;
        }
    }
    private void LateUpdate()
    {
        if (pawnable != null)
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
                if (_list_Pos_inWinPos[i] == null)
                {
                    SwitchPositionPawn(i - 1, i);
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
        if (_check_array._count_pawn_in_pos[pawnable.PawnID] == 3)
        {
            StartCoroutine(DeletePawnAfterScore(pawnable.PawnID, _check_array._start_pos_type_pawn[pawnable.PawnID]));
            ScoreManager.Instance.SetScorePlayer(3 + ScoreManager.Instance.GetComboPlayer());
        }
    }
    public void SwitchPositionPawn(int currentpos, int nextpos)
    {
        if (_list_Pos_inWinPos[currentpos] != null)
        {
            _list_Pos_inWinPos[nextpos] = _list_Pos_inWinPos[currentpos];
            _check_array._list_pawn_switching[currentpos] = _list_Pos_inWinPos[currentpos];
            _check_array._list_pos_switching[currentpos] = _finish_parent_trans.GetChild(nextpos);
            _list_Pos_inWinPos[currentpos].IsSwitchingPos = true;
            _list_Pos_inWinPos[currentpos] = null;
        }
    }
    public IEnumerator DeletePawnAfterScore(int pawnID, int pos_delete)
    {
        IsCalculateScore = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(_list_Pos_inWinPos[pos_delete].gameObject);
        Destroy(_list_Pos_inWinPos[pos_delete + 1].gameObject);
        Destroy(_list_Pos_inWinPos[pos_delete + 2].gameObject);

        SoundManager.Instance.PlaySound("add_score");
        if (PlayerData.Instance.GetVibrationStatus())
            Handheld.Vibrate();
        AnimateCoins.Instance.AddCoins(_StarText, _StarText_pos.position, _finish_parent_trans.GetChild(pos_delete + 1).position, 3 + ScoreManager.Instance.GetComboPlayer());

        _list_Pos_inWinPos[pos_delete] = _list_Pos_inWinPos[pos_delete + 1] = _list_Pos_inWinPos[pos_delete + 2] = null;
        for (int i = pos_delete; i < _list_Pos_inWinPos.Length - 3; i++)
        {
            SwitchPositionPawn(i + 3, i);
        }
        for (int i = 0; i < _check_array._start_pos_type_pawn.Length; i++)
        {
            if (_check_array._start_pos_type_pawn[i] != 0 && _check_array._start_pos_type_pawn[i] > _check_array._start_pos_type_pawn[pawnID])
            {
                _check_array._start_pos_type_pawn[i] -= 3;
            }
        }
        _check_array._count_pawn_in_pos[pawnID] = 0;
        _countPawninWinPos -= 3;
        _count_delete_objects += 3;
        ScoreManager.Instance.SetComboPlayer();
        IsCalculateScore = false;
    }
    public void ResetData()
    {
        _countPawninWinPos = 0;
        _count_delete_objects = 0;
        for (int i = 0; i < _list_Pos_inWinPos.Length; i++)
        {
            _list_Pos_inWinPos[i] = null;
        }
        for (int i = 0; i < _check_array._count_pawn_in_pos.Length; i++)
        {
            _check_array._count_pawn_in_pos[i] = 0;
        }
        for (int i = 0; i < _check_array._start_pos_type_pawn.Length; i++)
        {
            _check_array._start_pos_type_pawn[i] = 0;
        }
        _ready_lose_game = false;
    }
}
