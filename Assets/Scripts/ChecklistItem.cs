using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChecklistItem : MonoBehaviour
{
    
    [SerializeField] private Image checkmark;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private bool hideWhenNotAcquired = false;

    [SerializeField] private SelectableItem linkedItem;
    
    public void UpdateDisplay() {
        bool acquired = IsAcquired();
        if (acquired) {
            label.fontStyle = FontStyles.Strikethrough;
            checkmark.enabled = true;
        } else {
            label.fontStyle = FontStyles.Normal;
            checkmark.enabled = false;
        }
        if (hideWhenNotAcquired && !acquired) {
            gameObject.SetActive(false);
        } else {
            gameObject.SetActive(true);
        }
    }

    public bool IsAcquired() {
        return SelectionManager.takenObjects.Contains(linkedItem);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
