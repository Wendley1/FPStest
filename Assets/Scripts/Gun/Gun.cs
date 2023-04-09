using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private FOVController cameraFovController;
    public GunData data;
    [Space(8)]
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

    public bool IsAiming { get; private set; }

    private float timeToFire = 0f;
    private Vector3 normalPos;

    private void Update()
    {
        Inputs();
        Aim();
        WeaponSway();
        Shoot();
    }

    private void FixedUpdate()
    {
        if(weaponSprint)
            SprintAndRotate();

        if (weaponBob)
            WeaponBob();
    }

    private void SprintAndRotate()
    {
        Quaternion newRot = Quaternion.Euler(playerMovement.Sprinting ? data.sprintRot : data.normalRot);
        gun.localRotation = Quaternion.Slerp(gun.localRotation, newRot, data.sprintDamp * Time.deltaTime);
        normalPos = playerMovement.Sprinting ? data.sprintPos : data.restPosition;

        float r_factorZ = Mathf.Clamp(-Input.GetAxis("Horizontal") * data.r_amount, -data.r_maxAmount, data.r_maxAmount);

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            Vector3 rot = gun.localRotation.eulerAngles;
            rot.z = Mathf.Lerp(rot.z, rot.z + r_factorZ, Time.deltaTime);
            gun.localRotation = Quaternion.Euler(rot);
        }
    }

    private void WeaponBob()
    {
        float bobSpeed;
        float bobAmount;

        if (playerMovement.Moving && !playerMovement.Sprinting)
        {
            bobSpeed = data.n_bobSpeed;
            bobAmount = data.n_bobAmount;
        }
        else if (playerMovement.Sprinting)
        {
            bobSpeed = data.s_bobSpeed;
            bobAmount = data.s_bobAmount;
        }
        else
        {
            gun.localPosition = Vector3.Lerp(gun.localPosition, normalPos, Time.deltaTime * data.transitionSpeed);
            return;
        }

        timer += bobSpeed * Time.deltaTime;

        float x = normalPos.x;
        float y = normalPos.y + Mathf.Abs(Mathf.Sin(timer) * bobAmount);

        if (playerMovement.Sprinting)
        {
            x += Mathf.Cos(timer) * bobAmount;
        }
        else
        {
            x += Mathf.Cos(timer) * bobAmount;
            y += data.yWalkOffSet;
        }

        Vector3 newPosition = new(x, y, normalPos.z);
        gun.localPosition = Vector3.Lerp(gun.localPosition, newPosition, Time.deltaTime * data.transitionSpeed);

        if (timer > Mathf.PI * 2)
        {
            timer = 0;
        }
    }

    private void WeaponSway()
    {
        data.SwayController(IsAiming);

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
            Quaternion randomRotation = arms.localRotation *= Quaternion.Euler(data.rotationX, Random.Range(-data.rotationY, data.rotationY), Random.Range(-data.rotationZ, data.rotationZ));
            arms.localRotation = Quaternion.Slerp(arms.localRotation, randomRotation, data.smoothSpeed * Time.deltaTime);

            data.CurrentAmmo--;
            data.Recoil(IsAiming);

            Physics.Raycast(playerCam.position, playerCam.forward, out RaycastHit hit, data.maxDistance);

            var sphereObj = Instantiate(sphere, hit.point, Quaternion.identity);
            sphereObj.transform.SetParent(hit.transform.GetComponent<Transform>());

            if (hit.transform.GetComponent<Target>() != null)
                hit.transform.GetComponent<Target>().Damage(data.damage);
        }
    }

    private void Inputs()
    {
        IsAiming = Input.GetMouseButton(1);

        if (Input.GetKeyDown(KeyCode.R))
            data.Reload();
    }

    private void Aim()
    {
        Vector3 target = normalPosition;

        cameraFovController.AimFov = data.aimfov;

        if (IsAiming && !playerMovement.Sprinting)
            target = data.aimPosition;

        Vector3 desiredPosition = Vector3.Lerp(arms.localPosition, target, Time.deltaTime * data.aimVelocity);

        arms.localPosition = desiredPosition;
    }
}