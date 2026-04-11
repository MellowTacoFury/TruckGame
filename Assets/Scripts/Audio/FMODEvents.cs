using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: SerializeField] public EventReference barrelExplosion { get; private set; }
    [field: SerializeField] public EventReference backgroundMusic { get; private set; }
    [field: SerializeField] public EventReference carCrash { get; private set; }
    [field: SerializeField] public EventReference buttonClick { get; private set; }
    [field: SerializeField] public EventReference horns { get; private set; }
    [field: SerializeField] public EventReference drifting { get; private set; }
    [field: SerializeField] public EventReference driving { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}