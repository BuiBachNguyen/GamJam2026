using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Dialog", menuName ="NewDialog")]
public class Dialog : ScriptableObject
{
    [Header("General Info")]
    public string CharName;
    public Sprite CharAvatar;

    [Header("Dialog info")]
    public List<string> lines;
    public float typeSpeed = 0.05f;
    public AudioClip soundEffect;
}
