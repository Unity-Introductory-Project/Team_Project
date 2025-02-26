using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource buttonEffect;

    public void ButtonEffect()
    {
        buttonEffect.Play();
    }
}
