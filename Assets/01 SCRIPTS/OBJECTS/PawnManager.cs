using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : IPawnable
{
    [SerializeField] float speed;
    [SerializeField] Transform _testPos;
    Rigidbody _rb;
    bool _notRun = false;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!_notRun)
            if (Vector3.Distance(transform.position, _testPos.position) > 0.01f)
                MovePawn(_testPos.position);
            else
            {
                Vector3 _current_Scale = transform.localScale;
                this.transform.localScale = new Vector3(_current_Scale.x / 1.5f, _current_Scale.y / 1.5f, _current_Scale.z / 1.5f);
                _notRun = true;
            }
        
    }

    private void LateUpdate()
    {
        if (transform.rotation.y != 0)
        {
            Vector3 direction = _testPos.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), speed);
        }
    }

    public override void MovePawn(Vector3 _finishPos)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, _finishPos, speed * Time.deltaTime);
    }
}
