using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnLevel : MonoBehaviour
{
    [SerializeField] LevelDatabase levelDatabase;
    [SerializeField] GameObject _Pawn;
    [SerializeField] float _spawndelta_x, _spawndelta_z;
    [SerializeField] Transform _spawn_trans, _spawn_parrent;
    public Sprite[] sprites;
    int _spawnIDcount = 0;
    private void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Textures");
    }
    private void Start()
    {
        ScoreManager.Instance.count_img_in_texture = sprites.Length;
    }
    public int GetLengthSprites() => sprites.Length;
    public void SpawnLevel(int level)
    {
        ScoreManager.Instance.count_pawn_in_game = level * 3;
        Vector3 _spawn_pos = _spawn_trans.position;
        ResetLevel();
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
    private void ResetLevel()
    {
        if (_spawn_parrent != null && _spawn_parrent.childCount > 0)
        {
            for (int i = 0; i < _spawn_parrent.childCount; i++)
            {
                Destroy(_spawn_parrent.GetChild(i).gameObject);
            }
        }
    }
}
