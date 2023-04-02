using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public static CameraRecoil instance;

    [SerializeField] private Vector3 currentRotation;
    [SerializeField] private Vector3 targetRotation;

    [SerializeField] private float returnSpeed;

    private float snappines;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);

        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappines * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);    
    }

    public void RecoilFire(float recoilX, float recoilY, float recoilZ, float aimRecoilX, float aimRecoilY, float aimRecoilZ, bool isAiming, float snappines) 
    {
        this.snappines = snappines;

        targetRotation += isAiming ?
            new Vector3(aimRecoilX, Random.Range(-aimRecoilY, aimRecoilY), Random.Range(-aimRecoilZ, aimRecoilZ)) :
            new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    public void RecoilFire(float recoilX, float recoilY, float recoilZ, float snappines)
    {
        this.snappines = snappines;

        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
