using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : IPawnable
{
    [SerializeField] float _speed;
    [SerializeField] float _radius_check;
    [SerializeField] Transform _testPos;

    private void Start()
    {

    }
    private void Update()
    {
        Debug.Log(this.transform.rotation.z);
        if (this.transform.rotation.z == 90 || this.transform.rotation.x == 90)
        {
            this.transform.Rotate(new Vector3(0, this.transform.rotation.y, 0));
            Debug.Log(this.transform.rotation.z);
        }
        if (NotMouseClick)
        {
            if (Vector3.Distance(transform.position, _testPos.position) > 0.01f)
            {
                MovePawn(_testPos.position);
            }
            else
            {
                Vector3 _current_Scale = transform.localScale;
                this.transform.localScale = new Vector3(_current_Scale.x / 1.5f, _current_Scale.y / 1.5f, _current_Scale.z / 1.5f);
                NotMouseClick = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (transform.rotation.y != 0 && NotRotate)
        {
            Vector3 direction = _testPos.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), _speed * 10);
        }
    }

    public override void MovePawn(Vector3 _finishPos)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, _finishPos, _speed * Time.deltaTime);
    }
}
