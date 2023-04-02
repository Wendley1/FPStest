using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Finger
{
    public List<AllFingers> allFingers = new();
    public List<SingleFinger> sepFingers = new();

    [System.Serializable]
    public class SingleFinger
    {
        public Transform finger;
        [Range(-180f, 180f)] public float xValue;
        [Range(-180f, 180f)] public float yValue;
        [Range(-180f, 180f)] public float zValue;

        public void Update()
        {
            Quaternion targetRotation = Quaternion.Euler(xValue, yValue, zValue);

            finger.localRotation = Quaternion.Lerp(finger.localRotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    [System.Serializable]
    public class AllFingers
    {
        public List<Transform> fingers = new();
        [Range(0f, 1f)] public float closeValue;
        [Range(-180f, 180f)] public float yRotateValue;
        [Range(-180f, 180)] public float xRotateValue;

        public bool move = true;

        public void Update(bool reverse)
        {
            if (!move) return;

            for (int i = 1; i < fingers.Count; i++)
            {
                Quaternion targetRotation = reverse
                    ? Quaternion.Euler(0, 0, -closeValue * 100)
                    : Quaternion.Euler(0, 0, closeValue * 100);

                fingers[i].localRotation = Quaternion.Lerp(fingers[i].localRotation, targetRotation, Time.deltaTime * 10f);
                
                targetRotation = reverse
                    ? Quaternion.Euler(xRotateValue , yRotateValue, -closeValue * 100)
                    : Quaternion.Euler(xRotateValue, yRotateValue, closeValue * 100);

                fingers[0].localRotation = Quaternion.Lerp(fingers[0].localRotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}

