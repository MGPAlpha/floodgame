using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionPanel : MonoBehaviour
{

    public static ItemDescriptionPanel Main {get; private set;}

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescText;

    private CanvasGroup _cg;

    public void Activate(SelectableItem item) {
        itemNameText.text = item.ItemName;
        itemDescText.text = item.ItemDescription;
        _cg.alpha = 1;
        _cg.interactable = true;
    }

    public void Deactivate() {
        _cg.alpha = 0;
        _cg.interactable = false;
    }

    public void TakeItemButton() {
        _cg.interactable = false;
        SelectionManager.Main.TakeItem();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _cg = GetComponent<CanvasGroup>();
        _cg.alpha = 0;
        _cg.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
