using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGun : MonoBehaviour
{
    [System.Serializable]
    public class GunSettings
    {
        public GunData gunData;
        public GameObject gun;

        [Header("Targets and poles")]

        [Header("Left")]
        public Vector3 leftTargetPosition;
        public Quaternion leftTargetRotation;
        [Space(5)]
        public Vector3 leftpolePosition;
        [Header("Right")]
        public Vector3 rightTargetPosition;
        public Quaternion rightTargetRotation;
        [Space(5)]
        public Vector3 rightPolePosition;

        [Header("Hand")]

        public Finger leftHandFingerConfig;
        public Finger rightHandFingerConfig;
    }

    [SerializeField] private Hand leftHand;
    [SerializeField] private Hand rightHand;
    [Space(10)]
    [SerializeField] private Transform leftTarget;
    [SerializeField] private Transform leftPole;
    [Space(5)]
    [SerializeField] private Transform rightTarget;
    [SerializeField] private Transform rightPole;
    [Space(10)]
    [SerializeField] private GunSettings[] guns;
    [Space(5)]
    [SerializeField] private Gun gunScript;

    public int currentGun = 0;

    private void Start()
    {
        gunScript.data = guns[currentGun].gunData;

        guns[currentGun].gun.SetActive(true);

        leftTarget.SetLocalPositionAndRotation(guns[currentGun].leftTargetPosition, guns[currentGun].leftTargetRotation);
        leftPole.localPosition = guns[currentGun].leftpolePosition;

        rightTarget.SetLocalPositionAndRotation(guns[currentGun].rightTargetPosition, guns[currentGun].rightTargetRotation);
        rightPole.localPosition = guns[currentGun].rightPolePosition;

        leftHand.SetFinger(guns[currentGun].leftHandFingerConfig);
        rightHand.SetFinger(guns[currentGun].rightHandFingerConfig);
    }
}