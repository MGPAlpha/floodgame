using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITarget : MonoBehaviour
{
    public static UITarget Main {get; private set;}

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }
    
    private Image _img;
    private CanvasGroup _cg;
    [SerializeField] private float animSpeed = 1;

    float progress = 0;
    float visibleProgress = 1;
    public bool selectable = false;
    public bool visible = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _img = GetComponent<Image>();
        _img.material.SetFloat("_Progress", progress);
        _cg = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetProgress = selectable ? 1 : 0;

        progress = Mathf.MoveTowards(progress, targetProgress, animSpeed * Time.deltaTime * Mathf.Abs(progress - targetProgress));

        _img.material.SetFloat("_Progress", progress);

        float visibleTarget = visible ? 1 : 0;

        visibleProgress = Mathf.MoveTowards(visibleProgress, visibleTarget, animSpeed * Time.deltaTime * Mathf.Abs(visibleProgress - visibleTarget));

        _img.material.SetFloat("_Alpha", visibleProgress);
    }
}
