using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnLevel : MonoBehaviour
{
    [SerializeField] GameObject _Pawn;
    [SerializeField] float _spawnpos_x, _spawnpos_z;
    [SerializeField] Transform _spawn_trans;

    private void Awake()
    {
        SpawnLevel();
    }

    void SpawnLevel()
    {
        for (int i = 0; i < 20; i++)
        {
            Instantiate(_Pawn, new Vector3(Random.Range(-_spawnpos_x, _spawnpos_x), _spawn_trans.localPosition.y, Random.Range(-_spawnpos_z, _spawnpos_z)), Quaternion.identity);
        }
    }
}
