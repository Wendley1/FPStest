using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public PlayerMovement player;

    public GunData data;

    public Transform arms;
    public Transform gun;

    public GameObject sphere;
    public Transform playerCam;

    [Header("Weapon Sway")]

    [SerializeField] private float smooth;

    [Header("Aim")]

    [SerializeField] private Vector3 normalPosition;

    [Header("Movement")]

    [SerializeField] private bool weaponBob;
    [SerializeField] private bool weaponSprint;

    //privates

    private float timer = Mathf.PI / 2;
    private float bobSpeed;
    private float bobAmount;

    private bool aim;
    private float timeToFire = 0f;
    private Vector3 normalPos;

    private void Update()
    {
        Inputs();
        Aim();
        WeaponSway();
        Shoot();

        if (weaponSprint)
            SprintAndRotate();
    }

    private void FixedUpdate()
    {
        if (weaponBob)
            WeaponBob();
    }

    private void SprintAndRotate() 
    {
        if (player.Sprinting)
        {
            Quaternion newRot = Quaternion.Euler(data.sprintRot.x, data.sprintRot.y, data.sprintRot.z);
            gun.localRotation = Quaternion.Slerp(gun.localRotation, newRot, data.sprintDamp * Time.deltaTime);
            normalPos = data.sprintPos;
        }
        else
        {
            Quaternion newRot = Quaternion.Euler(data.normalRot.x, data.normalRot.y, data.normalRot.z);
            gun.localRotation = Quaternion.Slerp(gun.localRotation, newRot, data.sprintDamp * Time.deltaTime);
            normalPos = data.restPosition;
        }

        float r_factorZ = -(Input.GetAxis("Horizontal")) * data.r_amount;
        if (r_factorZ > data.r_maxAmount)
            r_factorZ = data.r_maxAmount;

        if (r_factorZ < -data.r_maxAmount)
            r_factorZ = -data.r_maxAmount;

        if ((Input.GetAxis("Horizontal")) != 0)
        {
            Vector3 rot = gun.localRotation.eulerAngles;
            rot.z = Mathf.Lerp(rot.z, rot.z + r_factorZ, Time.deltaTime);
            gun.localRotation = Quaternion.Euler(rot);
        }
    }

    private void WeaponBob() 
    {
        if (player.Moving && !player.Sprinting)
        {
            bobSpeed = data.n_bobSpeed;
            bobAmount = data.n_bobAmount;

            timer += bobSpeed * Time.deltaTime;

            Vector3 newPosition = new(Mathf.Cos(timer) * bobAmount, normalPos.y + Mathf.Abs((Mathf.Sin(timer) * bobAmount)), normalPos.z);
            gun.localPosition = Vector3.Lerp(gun.localPosition, newPosition, Time.deltaTime * data.transitionSpeed);
        }
        else if (player.Sprinting) 
        {
            bobSpeed = data.s_bobSpeed;
            bobAmount = data.s_bobAmount;

            timer += bobSpeed * Time.deltaTime;

            Vector3 newPosition = new(normalPos.x + Mathf.Cos(timer) * bobAmount, normalPos.y + Mathf.Abs((Mathf.Sin(timer) * bobAmount)), normalPos.z);
            gun.localPosition = Vector3.Lerp(gun.localPosition, newPosition, Time.deltaTime * data.transitionSpeed);
        }
        else
        {
            timer = Mathf.PI / 2;

            Vector3 newPosition = new(normalPos.x, normalPos.y, normalPos.z);
            gun.localPosition = Vector3.Lerp(gun.localPosition, newPosition, Time.deltaTime * data.transitionSpeed);
        }

        if (timer > Mathf.PI * 2)
            timer = 0;
    }

    private void WeaponSway()
    {
        data.SwayController(aim);

        float mouseX = Input.GetAxisRaw("Mouse X") * data.Sway;
        float mouseY = Input.GetAxisRaw("Mouse Y") * data.Sway;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        arms.localRotation = Quaternion.Slerp(arms.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire && data.CurrentAmmo > 0)
        {
            timeToFire = Time.time + (1f / data.fireRate);

            arms.localPosition -= Vector3.forward * data.backForce;
            Quaternion targetRotation = arms.localRotation *= Quaternion.Euler(data.rotationX, Random.Range(-data.rotationY, data.rotationY), Random.Range(-data.rotationZ, data.rotationZ));
            arms.localRotation = Quaternion.Slerp(arms.localRotation, targetRotation, data.smoothSpeed * Time.deltaTime);

            data.CurrentAmmo--;
            data.Recoil(aim);

            Physics.Raycast(playerCam.position, playerCam.forward, out RaycastHit hit, data.maxDistance);

            Instantiate(sphere, hit.point, Quaternion.identity, null);
        }
    }
    private void Inputs()
    {
        aim = Input.GetMouseButton(1);
        if (Input.GetKeyDown(KeyCode.R))
        {
            data.Reload();
        }
    }

    private void Aim()
    {
        Vector3 target = normalPosition;

        if (aim && !player.Sprinting)
            target = data.aimPosition;

        Vector3 desiredPosition = Vector3.Lerp(arms.localPosition, target, Time.deltaTime * data.aimVelocity);

        arms.localPosition = desiredPosition;
    }

}