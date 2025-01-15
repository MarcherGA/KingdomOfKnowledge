using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] Camera _cam;
    [SerializeField] Transform _followTarget;


    Vector2 _initialPosition;
    Vector2 _newPosition;
    Vector3 _newPositionVec3 = Vector3.zero;
    Vector2 _camMoveSinceStart => (Vector2) _cam.transform.position - _initialPosition;
    float _zDistanceFromTarget => transform.position.z - _followTarget.transform.position.z;
    float _clippingPlane => _cam.transform.position.z + (_zDistanceFromTarget > 0 ? _cam.farClipPlane : _cam.nearClipPlane);
    float _parallaxFactor => Mathf.Abs(_zDistanceFromTarget) / _clippingPlane;
    float _initialZ;


    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        _initialZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        _newPosition = _initialPosition + _camMoveSinceStart * _parallaxFactor;
        _newPositionVec3.Set(_newPosition.x, _newPosition.y, _initialZ);
        transform.position = _newPositionVec3;
    }
}