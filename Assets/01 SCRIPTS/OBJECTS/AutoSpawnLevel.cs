using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnLevel : MonoBehaviour
{
    //[SerializeField] PawnDatabase pawnData;
    [SerializeField] GameObject _Pawn;
    [SerializeField] float _spawndelta_x, _spawndelta_z;
    [SerializeField] Transform _spawn_trans, _spawn_parrent;

    int _spawnIDcount = 0;

    private void Start()
    {
        //pawnData.CreateListPawn(sprites.Length);
        //for (int i = 0; i < pawnData.GetListLength(); i++)
        //{
        //    pawnData.GetPawnInPos(i)._pawn_img = sprites[i];
        //}
        SpawnLevel();
    }

    void SpawnLevel()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures");

        ScoreManager.Instance.count_img_in_texture = sprites.Length;
        ScoreManager.Instance.count_pawn_in_game = sprites.Length * 4;

        Vector3 _spawn_pos = _spawn_trans.position;
        for (int i = 0; i < sprites.Length * 4; i++)
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
