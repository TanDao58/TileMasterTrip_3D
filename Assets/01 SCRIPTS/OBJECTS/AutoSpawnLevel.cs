using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnLevel : MonoBehaviour
{
    [SerializeField] PawnDatabase pawnData;
    [SerializeField] GameObject _Pawn;
    [SerializeField] float _spawndelta_x, _spawndelta_z;
    [SerializeField] Transform _spawn_trans;

    int _spawnIDcount = 0;

    private void Awake()
    {
        SpawnLevel();
    }

    void SpawnLevel()
    {
        Vector3 _spawn_pos = _spawn_trans.position;
        for (int i = 0; i < pawnData.GetListLength() * 3; i++)
        {
            GameObject g = Instantiate(_Pawn, new Vector3(Random.Range(_spawn_pos.x - _spawndelta_x, _spawn_pos.x + _spawndelta_x), _spawn_pos.y, Random.Range(_spawn_pos.z - _spawndelta_z, _spawn_pos.z + _spawndelta_z)), Quaternion.identity);
            g.GetComponent<PawnManager>().CurrentPawn._pawn_id = _spawnIDcount;
        }
    }
}
