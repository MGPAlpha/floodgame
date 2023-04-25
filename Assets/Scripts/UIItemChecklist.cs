using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    private TextMeshProUGUI finishButtonText;

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
                finishButtonText.text = "Recover All Required Items";
                break;
            }
        }

        if (finishButton.interactable) {
            //Try if player is over boat
            finishButton.interactable = false;
            StarterAssets.FirstPersonController player = GameObject.FindObjectOfType<StarterAssets.FirstPersonController>();
            if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 5, LayerMask.GetMask(new string[]{"Default"}))) {
                if (hit.collider.tag == "Finish") {
                    finishButton.interactable = true;
                }
            }

            if (finishButton.interactable) {
                finishButtonText.text = "Finish Recovery";
            } else {
                finishButtonText.text = "Return to Boat to Finish Recovery";
            }
        }

        

        ItemDescriptionPanel.Main.Deactivate();
    }

    public void Deactivate() {
        _cg.alpha = 0;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
    }

    public void MainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }

    // Start is called before the first frame update
    void Start()
    {
        items = new List<ChecklistItem>(GetComponentsInChildren<ChecklistItem>(true));
        _cg = GetComponent<CanvasGroup>();
        _cg.alpha = 0;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;

        finishButtonText = finishButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
