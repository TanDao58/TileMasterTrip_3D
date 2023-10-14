using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : IPawnable
{
    [SerializeField] float _speed;
    Vector3 _current_Scale;

    private void Start()
    {
        _current_Scale = transform.localScale;
    }
    public override void MovePawn(Vector3 _finishPos)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, _finishPos, _speed * Time.deltaTime);
    }
    public override void ScalePawn(float value)
    {
        this.transform.localScale = new Vector3(_current_Scale.x / value, _current_Scale.y / value, _current_Scale.z / value);
    }
    public override void RotatePawn(Vector3 _pos)
    {
        if (transform.rotation.y != 0)
        {
            Vector3 direction = _pos - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), _speed * 10);
        }
    }
    public override float CalculateAnglePawntoTable(Vector3 _TablePos)
    {
        Vector3 direction = transform.position - _TablePos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }
}
