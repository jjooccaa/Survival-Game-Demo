using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : Singleton<SelectionManager>
{
    [HideInInspector] public bool inOnTarget;
    [HideInInspector] public GameObject selectedObject;
    [HideInInspector] public GameObject selectedTree;
    [HideInInspector] public bool isTakeImgVisible;

    [Header("UI")]
    [SerializeField] GameObject interactionInfoUI;
    TextMeshProUGUI interactionText;
    [SerializeField] Image selectImage;
    [SerializeField] Image takeImage;
    public GameObject chopHolder;

    void Start()
    {
        inOnTarget = false;
        interactionText = interactionInfoUI.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactableObject = selectionTransform.GetComponent<InteractableObject>();

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            if (choppableTree && choppableTree.isPlayerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            if (interactableObject && interactableObject.isPlayerInRange)
            {
                inOnTarget = true;
                selectedObject = interactableObject.gameObject;
                interactionText.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interactionInfoUI.SetActive(true);

                if(interactableObject.CompareTag("Pickable"))
                {
                    selectImage.gameObject.SetActive(false);
                    takeImage.gameObject.SetActive(true);

                    isTakeImgVisible = true;
                }
                else
                {
                    takeImage.gameObject.SetActive(false);
                    selectImage.gameObject.SetActive(true);

                    isTakeImgVisible = false;
                }
            }
            else
            {
                inOnTarget = false;

                interactionInfoUI.SetActive(false);

                takeImage.gameObject.SetActive(false);
                selectImage.gameObject.SetActive(true);

                isTakeImgVisible = false;
            }
        }
        else
        {
            interactionInfoUI.SetActive(false);

            takeImage.gameObject.SetActive(false);
            selectImage.gameObject.SetActive(true);

            isTakeImgVisible = false;
        }
    }

    public void DisableSelection()
    {
        selectImage.enabled = false;
        takeImage.enabled = false;
        interactionInfoUI.SetActive(false);

        selectedObject = null;
        
        GetComponent<SelectionManager>().enabled = false;
    }

    public void EnableSelection()
    {
        selectImage.enabled = true;
        takeImage.enabled = true;
        interactionInfoUI.SetActive(true);

        GetComponent<SelectionManager>().enabled = true;
    }

}
