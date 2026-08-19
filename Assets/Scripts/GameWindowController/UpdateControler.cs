using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class UpdateControler : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField] private bool controlWindowMode = false;

    [SerializeField] GameObject CRT;
    public bool ControlWindowMode
    {
        get { return controlWindowMode; }
        set { controlWindowMode = value; }
    }

    public static UpdateControler Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        PlayerController.IsRemoteUsed -= OnSwitch;
        PlayerController.IsRemoteUsed += OnSwitch;
    }
    public void OnSwitch(bool value)
    {
        controlWindowMode = value;
        if (CRT != null)
            CRT.SetActive(value);
    }

    private void OnDestroy()
    {
        PlayerController.IsRemoteUsed -= OnSwitch;
    }

}
