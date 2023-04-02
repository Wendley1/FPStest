using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InverseKinematics : MonoBehaviour
{

    [SerializeField] private Transform _upperArmTransform;
    [SerializeField] private Transform _forearmTransform;
    [SerializeField] private Transform _handTransform;
    [Space(20)]
    [SerializeField] private Transform _elbowTransform;
    [SerializeField] private Transform _targetTransform;
    [Space(20)]
    [SerializeField] private Vector3 _upperArmOffsetRotation;
    [SerializeField] private Vector3 _forearmOffsetRotation;
    [SerializeField] private Vector3 _handOffsetRotation;
    [Space(20)]
    [SerializeField] private bool fixedUpperArm = false;
    [SerializeField] private Vector3 _upperArmPosition;
    [Space(20)]
    [SerializeField] private bool _handMatchesTargetRotation = true;
    [Space(20)]
    [SerializeField] private bool _debug;

    private float _upperArmLength;
    private float _forearmLength;
    private float _armLength;
    private float _targetDistance;
    private float _adjacent;

    private void LateUpdate()
    {
        if (_upperArmTransform == null || _forearmTransform == null || _handTransform == null || _elbowTransform == null || _targetTransform == null)
        {
            return;
        }

        _upperArmTransform.LookAt(_targetTransform, _elbowTransform.position - _upperArmTransform.position);
        _upperArmTransform.Rotate(_upperArmOffsetRotation);

        Vector3 cross = Vector3.Cross(_elbowTransform.position - _upperArmTransform.position, _forearmTransform.position - _upperArmTransform.position);

        _upperArmLength = Vector3.Distance(_upperArmTransform.position, _forearmTransform.position);
        _forearmLength = Vector3.Distance(_forearmTransform.position, _handTransform.position);
        _armLength = _upperArmLength + _forearmLength;
        _targetDistance = Vector3.Distance(_upperArmTransform.position, _targetTransform.position);
        _targetDistance = Mathf.Min(_targetDistance, _armLength - _armLength * 0.001f);

        _adjacent = ((_upperArmLength * _upperArmLength) - (_forearmLength * _forearmLength) + (_targetDistance * _targetDistance)) / (2 * _targetDistance);

        float angle = Mathf.Acos(_adjacent / _upperArmLength) * Mathf.Rad2Deg;

        _upperArmTransform.RotateAround(_upperArmTransform.position, cross, -angle);

        _forearmTransform.LookAt(_targetTransform, cross);
        _forearmTransform.Rotate(_forearmOffsetRotation);

        if (_handMatchesTargetRotation)
        {
            _handTransform.rotation = _targetTransform.rotation;
            _handTransform.Rotate(_handOffsetRotation);
        }

        if (_debug)
        {
            Debug.DrawRay(_forearmTransform.position, _elbowTransform.position - _forearmTransform.position, Color.blue);
            Debug.DrawRay(_upperArmTransform.position, _targetTransform.position - _upperArmTransform.position, Color.red);
        }

        if(fixedUpperArm)
            _upperArmTransform.localPosition = _upperArmPosition;
    }

    private void OnDrawGizmos()
    {
        if (!_debug)
        {
            return;
        }

        if (_upperArmTransform == null || _forearmTransform == null || _handTransform == null || _elbowTransform == null || _targetTransform == null)
        {
            return;
        }

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(_upperArmTransform.position, _forearmTransform.position);
        Gizmos.DrawLine(_forearmTransform.position, _handTransform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_upperArmTransform.position, _targetTransform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_forearmTransform.position, _elbowTransform.position);
    }
}
