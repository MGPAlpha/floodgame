using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableItem : MonoBehaviour
{
    [field: SerializeField] public string ItemName {get; private set;} = "default";
    [field: TextArea(5, 10)][field: SerializeField] public string ItemDescription {get; private set;} = "default";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
