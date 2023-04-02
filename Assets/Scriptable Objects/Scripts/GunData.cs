using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Gun/GunData")]
public class GunData : ScriptableObject
{
    [Header("info")]

    public new string name;

    [Header("Setup")]

    public int ammo;
    public int CurrentAmmo { get; set; }

    public float maxDistance;
    public float damage;

    [Header("Others")]

    public float aimSway;
    public float normalSway;
    public float Sway { get; private set; }

    [Header("Shooting")]

    public float fireRate;

    public float backForce = 0.1f;
    [Space(6)]
    public float smoothSpeed = 1f;

    public float rotationX;
    public float rotationY;
    public float rotationZ;

    [Header("Aim")]

    public float aimVelocity;
    public Vector3 aimPosition;

    [Header("Recoil")]
    [Space(8)]
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;
    [Space(8)]
    [SerializeField] private float aimRecoilX;
    [SerializeField] private float aimRecoilY;
    [SerializeField] private float aimRecoilZ;
    [Space(8)]
    [SerializeField] private float snappines;

    [Header("Weapon Bob")]
    public Vector3 restPosition;
    public float transitionSpeed = 3;
    public float n_bobSpeed = 6.3f, s_bobSpeed = 9;
    public float n_bobAmount = 0.015f, s_bobAmount = 0.05f;

    [Header("Weapon Sprint")]
    public float sprintDamp;
    public Vector3 normalRot;
    public Vector3 sprintPos;
    public Vector3 sprintRot;

    public float r_amount = 25f;
    public float r_maxAmount = 45f;
    public float r_smooth = 60;

    public void Recoil(bool isAiming) 
    {
        CameraRecoil.instance.RecoilFire(recoilX, recoilY, recoilZ, aimRecoilX, aimRecoilY, aimRecoilZ, isAiming, snappines);
    }
    public void SwayController(bool isAiming) 
    {
        Sway = isAiming ?
            aimSway : normalSway;
    }
    public void Reload() 
    {
        CurrentAmmo = ammo;
    }
}
