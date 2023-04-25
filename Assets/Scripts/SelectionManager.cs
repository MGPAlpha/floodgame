using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager Main {get; private set;}

    public static List<string> takenObjects {get; private set;} = new List<string>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Main = this;
    }

    [SerializeField] private LayerMask selectableLayers;
    [SerializeField] private Transform objectViewPos;
    [SerializeField] private Transform objectTakePos;
    [SerializeField] private StarterAssets.FirstPersonController controller;
    [SerializeField] private float pickUpTime = .5f;
    [SerializeField] private Light flashlight;
    [SerializeField] private Light itemSpotlight;

    bool selectionComplete = false;
    Vector2 selectedRotation = new Vector2();
    SelectableItem selected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 selectedDefaultPos;
    Quaternion selectedDefaultRot;
    Vector3 selectedDefaultScale;
    float defaultLightIntensity;

    IEnumerator SelectItem(SelectableItem target) {
        selected = target;
        controller.enabled = false;
        UITarget.Main.visible = false;
        Transform targetTransform = target.transform;
        selectedDefaultPos = targetTransform.position;
        selectedDefaultRot = targetTransform.rotation;
        defaultLightIntensity = flashlight.intensity;
        selectedDefaultScale = targetTransform.localScale;
        Vector3 targetScale = targetTransform.localScale * target.viewScaleFactor;

        ItemDescriptionPanel.Main.Activate(target);

        float moveTimer = 0;
        while (moveTimer < pickUpTime) {
            float moveProgress = moveTimer / pickUpTime;
            targetTransform.position = Vector3.Lerp(selectedDefaultPos, objectViewPos.position, moveProgress);
            targetTransform.rotation = Quaternion.Lerp(selectedDefaultRot, objectViewPos.rotation, moveProgress);
            targetTransform.localScale = Vector3.Lerp(selectedDefaultScale, targetScale, moveProgress);
            flashlight.intensity = Mathf.Lerp(defaultLightIntensity, 0, moveProgress);
            itemSpotlight.intensity = Mathf.Lerp(0, 1, moveProgress);
            Rect r = new Rect(0, 0, Mathf.Lerp(1, .7f, moveProgress), 1);
            Camera.main.rect = r;
            UIInfoPanel.Main.SetVisibility(moveProgress);
            moveTimer += Time.deltaTime;
            yield return null;
        }
        targetTransform.position = objectViewPos.position;
        targetTransform.rotation = objectViewPos.rotation;
        targetTransform.localScale = targetScale;
        flashlight.intensity = 0;
        itemSpotlight.intensity = 1;
        Camera.main.rect = new Rect(0, 0, .7f, 1);
        UIInfoPanel.Main.SetVisibility(1);
        selectedRotation = Vector2.zero;
        Cursor.lockState = CursorLockMode.None;
        selectionComplete = true;
    }

    IEnumerator DeselectItem() {
        if (!selected) yield break;
        selectionComplete = false;
        Cursor.lockState = CursorLockMode.Locked;
        Transform targetTransform = selected.transform;
        Vector3 startPos = targetTransform.position;
        Quaternion startRot = targetTransform.rotation;
        Vector3 startScale = targetTransform.localScale;

        float moveTimer = 0;
        while (moveTimer < pickUpTime) {
            float moveProgress = moveTimer / pickUpTime;
            targetTransform.position = Vector3.Lerp(startPos, selectedDefaultPos, moveProgress);
            targetTransform.rotation = Quaternion.Lerp(startRot, selectedDefaultRot, moveProgress);
            targetTransform.localScale = Vector3.Lerp(startScale, selectedDefaultScale, moveProgress);
            flashlight.intensity = Mathf.Lerp(0, defaultLightIntensity, moveProgress);
            itemSpotlight.intensity = Mathf.Lerp(1, 0, moveProgress);
            Rect r = new Rect(0, 0, Mathf.Lerp(.7f, 1, moveProgress), 1);
            Camera.main.rect = r;
            UIInfoPanel.Main.SetVisibility(1-moveProgress);
            moveTimer += Time.deltaTime;
            yield return null;
        }
        targetTransform.position = selectedDefaultPos;
        targetTransform.rotation = selectedDefaultRot;
        targetTransform.localScale = selectedDefaultScale;
        flashlight.intensity = defaultLightIntensity;
        itemSpotlight.intensity = 0;
        Camera.main.rect = new Rect(0, 0, 1, 1);
        UIInfoPanel.Main.SetVisibility(0);
        UITarget.Main.visible = true;
        selected = null;
        controller.enabled = true;
    }

    public void TakeItem() {
        StartCoroutine(TakeItemCoroutine());
    }

    private IEnumerator TakeItemCoroutine() {
        if (!selected) yield break;
        selectionComplete = false;
        Cursor.lockState = CursorLockMode.Locked;
        Transform targetTransform = selected.transform;
        Vector3 startPos = targetTransform.position;
        Quaternion startRot = targetTransform.rotation;

        float moveTimer = 0;
        while (moveTimer < pickUpTime) {
            float moveProgress = moveTimer / pickUpTime;
            targetTransform.position = Vector3.Lerp(startPos, objectTakePos.position, moveProgress);
            // targetTransform.rotation = Quaternion.Lerp(startRot, selectedDefaultRot, moveProgress);
            flashlight.intensity = Mathf.Lerp(0, defaultLightIntensity, moveProgress);
            itemSpotlight.intensity = Mathf.Lerp(1, 0, moveProgress);
            Rect r = new Rect(0, 0, Mathf.Lerp(.7f, 1, moveProgress), 1);
            Camera.main.rect = r;
            UIInfoPanel.Main.SetVisibility(1-moveProgress);
            moveTimer += Time.deltaTime;
            yield return null;
        }
        targetTransform.position = selectedDefaultPos;
        // targetTransform.rotation = selectedDefaultRot;
        flashlight.intensity = defaultLightIntensity;
        itemSpotlight.intensity = 0;
        Camera.main.rect = new Rect(0, 0, 1, 1);
        UIInfoPanel.Main.SetVisibility(0);
        UITarget.Main.visible = true;
        takenObjects.Add(selected.ItemName);
        selected.gameObject.SetActive(false);
        selected = null;
        controller.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        SelectableItem target = null;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 4, selectableLayers)) {
            hit.collider.TryGetComponent<SelectableItem>(out target);
        }
        if (target) {
            UITarget.Main.selectable = true;
        } else {
            UITarget.Main.selectable = false;
        }
        if (!selected && target && Input.GetMouseButtonDown(0)) {
            StartCoroutine(SelectItem(target));
        }
        if (selectionComplete && Input.GetMouseButton(0)) {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            selectedRotation += new Vector2(-mouseX, mouseY);
            Quaternion defaultRot = objectViewPos.localRotation;
            defaultRot = Quaternion.Euler(selectedRotation.y, 0, 0) * Quaternion.Euler(0, selectedRotation.x, 0) * defaultRot;
            Quaternion worldRot = objectViewPos.parent.rotation * defaultRot;
            selected.transform.rotation = worldRot;
        }
        if (selectionComplete && Input.GetKeyDown(KeyCode.Escape)) {
            StartCoroutine(DeselectItem());
        }
    }
}
