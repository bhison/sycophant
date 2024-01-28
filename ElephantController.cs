using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantController : MonoBehaviour
{
    public static ElephantController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        } 
    }

    
}
