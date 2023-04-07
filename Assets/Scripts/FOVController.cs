using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVController : MonoBehaviour
{
    [SerializeField] private PlayerMovement mov;
    [SerializeField] private Camera cam;
    [SerializeField] private Gun gun;
    [Space(5)]
    [SerializeField] private float fovSmooth;
    [Space(5)]
    [SerializeField] private float normalFov;
    [SerializeField] private float sprintFov;
    [SerializeField] private float aimFov;

    private float fov;
    
    private void Start()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (mov.Sprinting) 
        {
            fov = sprintFov;
        }
        else if (gun.IsAiming) 
        {
            fov = aimFov;
        }
        else 
        {
            fov = normalFov;
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * fovSmooth);
    }
}
