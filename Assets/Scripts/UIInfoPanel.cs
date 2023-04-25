using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour
{

    public static UIInfoPanel Main {get; private set;}

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }

    private RectTransform rectTransform;

    public void SetVisibility(float progress) {
        rectTransform.anchorMin = new Vector2(Mathf.Lerp(1, .7f, progress), 0);
        rectTransform.anchorMax = new Vector2(Mathf.Lerp(1.3f, 1, progress), 1);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        SetVisibility(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
