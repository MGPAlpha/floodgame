using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    [SerializeField] private LayerMask selectableLayers;
    [SerializeField] private Transform objectViewPos;
    [SerializeField] private StarterAssets.FirstPersonController controller;
    [SerializeField] private float pickUpTime = .5f;

    bool selectionComplete = false;
    Vector2 selectedRotation = new Vector2();
    SelectableItem selected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 selectedDefaultPos;
    Quaternion selectedDefaultRot;

    IEnumerator SelectItem(SelectableItem target) {
        selected = target;
        controller.enabled = false;
        Transform targetTransform = target.transform;
        selectedDefaultPos = targetTransform.position;
        selectedDefaultRot = targetTransform.rotation;

        float moveTimer = 0;
        while (moveTimer < pickUpTime) {
            moveTimer += Time.deltaTime;
            float moveProgress = moveTimer / pickUpTime;
            targetTransform.position = Vector3.Lerp(selectedDefaultPos, objectViewPos.position, moveProgress);
            targetTransform.rotation = Quaternion.Lerp(selectedDefaultRot, objectViewPos.rotation, moveProgress);
            yield return null;
        }
        targetTransform.position = objectViewPos.position;
        targetTransform.rotation = objectViewPos.rotation;
        selectionComplete = true;
    }

    IEnumerator DeselectItem() {
        if (!selected) yield break;
        selectionComplete = false;
        Transform targetTransform = selected.transform;

        float moveTimer = 0;
        while (moveTimer < pickUpTime) {
            moveTimer += Time.deltaTime;
            float moveProgress = moveTimer / pickUpTime;
            targetTransform.position = Vector3.Lerp(objectViewPos.position, selectedDefaultPos, moveProgress);
            targetTransform.rotation = Quaternion.Lerp(objectViewPos.rotation, selectedDefaultRot, moveProgress);
            yield return null;
        }
        targetTransform.position = selectedDefaultPos;
        targetTransform.rotation = selectedDefaultRot;
        selected = null;
        controller.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        SelectableItem target = null;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 7, selectableLayers)) {
            hit.collider.TryGetComponent<SelectableItem>(out target);
        }
        if (!selected && target && Input.GetMouseButtonDown(0)) {
            StartCoroutine(SelectItem(target));
        }
        if (selectionComplete) {
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
