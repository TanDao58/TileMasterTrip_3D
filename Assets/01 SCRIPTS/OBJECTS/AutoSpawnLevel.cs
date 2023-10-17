using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnLevel : MonoBehaviour
{
    [SerializeField] LevelDatabase levelDatabase;
    [SerializeField] GameObject _Pawn;
    [SerializeField] float _spawndelta_x, _spawndelta_z;
    [SerializeField] Transform _spawn_trans, _spawn_parrent;
    int _spawnIDcount = 0;
    private void Start()
    {
        SpawnLevel(levelDatabase.GetLevelAtPos(PlayerData.Instance.GetCurrentLevel()));
    }
    public void SpawnLevel(int level)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures");

        ScoreManager.Instance.count_img_in_texture = sprites.Length;
        ScoreManager.Instance.count_pawn_in_game = level * 3;

        Vector3 _spawn_pos = _spawn_trans.position;
        for (int i = 0; i < ScoreManager.Instance.count_pawn_in_game; i++)
        {
            GameObject g = Instantiate(_Pawn, new Vector3(Random.Range(_spawn_pos.x - _spawndelta_x, _spawn_pos.x + _spawndelta_x), _spawn_pos.y, Random.Range(_spawn_pos.z - _spawndelta_z, _spawn_pos.z + _spawndelta_z)), Quaternion.identity, _spawn_parrent);

            g.GetComponent<PawnManager>().PawnID = _spawnIDcount;
            g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[_spawnIDcount];

            if ((i + 1) % 3 == 0)
                _spawnIDcount++;
            if (_spawnIDcount >= sprites.Length)
                _spawnIDcount = 0;
        }
    }
}
