using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGun : MonoBehaviour
{
    [System.Serializable]
    public class GunSettings
    {
        public GunData gunData;
        public Transform gun;

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

        public void GunActive(bool value) 
        {
            gun.gameObject.SetActive(value);
        }
    }

    [SerializeField] private Transform targetsTransform;
    [Space(5)]
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

    public int selectedWeapon = 0;

    bool a;

    private void Start()
    {
        SelectedWeapon();
    }

    private void Update()
    {
        int previusSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            if (selectedWeapon >= guns.Length - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        leftTarget.SetLocalPositionAndRotation(guns[selectedWeapon].leftTargetPosition, guns[selectedWeapon].leftTargetRotation);
        leftPole.localPosition = guns[selectedWeapon].leftpolePosition;

        rightTarget.SetLocalPositionAndRotation(guns[selectedWeapon].rightTargetPosition, guns[selectedWeapon].rightTargetRotation);
        rightPole.localPosition = guns[selectedWeapon].rightPolePosition;

        leftHand.SetFinger(guns[selectedWeapon].leftHandFingerConfig);
        rightHand.SetFinger(guns[selectedWeapon].rightHandFingerConfig);

        if (previusSelectedWeapon != selectedWeapon) 
        {
            SelectedWeapon();
        }
    }

    private void SelectedWeapon() 
    {
        int i = 0;

        foreach(GunSettings s in guns) 
        {
            if (i == selectedWeapon)
                s.GunActive(true);
            else
                s.GunActive(false);

            i++;
        }

        gunScript.data = guns[selectedWeapon].gunData;

        leftTarget.SetLocalPositionAndRotation(guns[selectedWeapon].leftTargetPosition, guns[selectedWeapon].leftTargetRotation);
        leftPole.localPosition = guns[selectedWeapon].leftpolePosition;

        rightTarget.SetLocalPositionAndRotation(guns[selectedWeapon].rightTargetPosition, guns[selectedWeapon].rightTargetRotation);
        rightPole.localPosition = guns[selectedWeapon].rightPolePosition;

        leftHand.SetFinger(guns[selectedWeapon].leftHandFingerConfig);
        rightHand.SetFinger(guns[selectedWeapon].rightHandFingerConfig);

        targetsTransform.SetParent(guns[selectedWeapon].gun, true);

    }
}