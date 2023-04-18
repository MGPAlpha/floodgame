using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogAdjuster : MonoBehaviour
{
    
    [SerializeField] private Transform waterLevel;
    
    [SerializeField] private float minFogHeight = 0;
    [SerializeField] private float maxFogHeight = 15;
    [SerializeField] private float minFogDensity = 0;
    [SerializeField] private float maxFogDensity = 15;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waterLevel) {
            RenderSettings.fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, Mathf.InverseLerp(minFogHeight, maxFogHeight, transform.position.y - waterLevel.position.y));
        }
    }
}
