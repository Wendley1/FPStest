using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [Header("Setup"), Space(8)]

    [SerializeField] private GameObject handObject;

    [SerializeField] private int bonesIgnore;

    [SerializeField] private int fingerBonesPerHand = 4;

    [Header("Fingers Config"), Space(8)]

    [Range(0f,1f),SerializeField] private float CloseValue;

    [Space(8)]

    [SerializeField,Tooltip("Reverse the fingers while closing")] private bool reverse = false;

    [SerializeField] private bool moveAllFingers = true;
    [SerializeField] private bool moveSepFingers = true;
    [SerializeField] private bool moveSingleFingers = false;

    [Header("List"), Space(8)]

    [SerializeField] private int[] canFingerMove;

    [Space(8)]

    [SerializeField] private Finger fingers;

    private void Update()
    {
        UpdateBones();
    }

    public void UpdateBones() 
    {
        if (moveAllFingers && !moveSepFingers)
        {
            for (int i = 0; i < fingers.allFingers.Count; i++)
            {
                fingers.allFingers[i].closeValue = CloseValue;
                fingers.allFingers[i].Update(reverse);
            }
        }
        else if (!moveAllFingers && moveSepFingers)
        {
            for (int i = 0; i < fingers.sepFingers.Count; i++)
            {
                fingers.sepFingers[i].Update();
            }
        }
        else if (moveSepFingers && moveSepFingers)
        {
            for (int i = 0; i < fingers.allFingers.Count; i++)
            {
                fingers.allFingers[i].Update(reverse);
            }
        }
        else if (!moveAllFingers && !moveSepFingers)
        {
            for (int i = 0; i < fingers.allFingers.Count; i++)
            {
                if(!moveSingleFingers)
                    fingers.allFingers[i].closeValue = CloseValue;

                fingers.allFingers[i].Update(reverse);
            }

            for (int i = 0; i < canFingerMove.Length; i++)
            {
                fingers.sepFingers[canFingerMove[i]].Update();
            }
        }
    }

    public void GetBones() 
    {
        Transform[] fingerBones = handObject.GetComponentsInChildren<Transform>();

        fingers.sepFingers = new();
        fingers.allFingers = new();

        int currentHand = 0;
        int caunt = 0;

        fingers.allFingers.Add(new Finger.AllFingers());

        for (int i = bonesIgnore; i < fingerBones.Length; i++)
        {
            if (caunt == fingerBonesPerHand)
            {
                fingers.allFingers.Add(new Finger.AllFingers());
                currentHand++;
                caunt = 1;
            }
            else
            {
                caunt++;
            }

            fingers.allFingers[currentHand].fingers.Add(fingerBones[i]);
        }

        for(int i = 1; i < fingerBones.Length; i++) 
        {
            Finger.SingleFinger f = new()
            {
                finger = fingerBones[i]
            };

            fingers.sepFingers.Add(f);
        }
    }

    public void SetFinger(Finger finger) 
    {
        fingers = finger;
    }
}
