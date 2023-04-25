using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChecklist : MonoBehaviour
{

    public static UIItemChecklist Main {get; private set;}

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }

    [SerializeField] private Button finishButton;

    List<ChecklistItem> items = new List<ChecklistItem>();
    CanvasGroup _cg;

    [SerializeField] private List<ChecklistItem> required;

    public void Activate() {
        foreach (ChecklistItem item in items) {
            item.UpdateDisplay();
        }
        _cg.alpha = 1;
        _cg.interactable = true;
        _cg.blocksRaycasts = true;

        finishButton.interactable = true;
        foreach (ChecklistItem item in required) {
            if (!item.IsAcquired()) {
                finishButton.interactable = false;
                break;
            }
        }

        ItemDescriptionPanel.Main.Deactivate();
    }

    public void Deactivate() {
        _cg.alpha = 0;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        items = new List<ChecklistItem>(GetComponentsInChildren<ChecklistItem>(true));
        _cg = GetComponent<CanvasGroup>();
        _cg.alpha = 0;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
