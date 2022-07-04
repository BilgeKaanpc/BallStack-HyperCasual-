using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform _ballTransform;
    private Vector3 _offset;
    public float _lerpTime;
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - _ballTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 _newPos = Vector3.Lerp(transform.position, _ballTransform.position + _offset, _lerpTime * Time.deltaTime);
        transform.position = _newPos;
    }
}
