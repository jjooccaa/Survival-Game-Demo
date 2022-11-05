using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : Singleton<CraftingSystem>
{
    [Header("UI")]
    [SerializeField] GameObject craftingUI;
    [SerializeField] GameObject toolsUI;
    [SerializeField] GameObject survivalUI;
    [SerializeField] GameObject processUI;

    //Category Buttons
    Button toolsButton;
    Button survivalButton;
    Button processButton;
    //Craft Buttons
    Button craftAxeButton;
    Button craftPlankButton;
    //Requirement Text
    TextMeshProUGUI axeReq1, axeReq2;
    TextMeshProUGUI plankReq1;

    List<string> inventoryItemList = new List<string>();

    [HideInInspector] public bool isOpen;

    //Blueprints
    public ItemBlueprint AxeBlueprint = new ItemBlueprint(Name.ResourceAxe, 1, 2, Name.ResourceRock, 3, Name.ResourceStick, 3);
    public ItemBlueprint PlankBlueprint = new ItemBlueprint(Name.ResourcePlank, 2, 1, Name.ResourceLog, 1);
    
    void Start()
    {
        isOpen = false;

        toolsButton = craftingUI.transform.Find("Tools_Button").GetComponent<Button>();
        toolsButton.onClick.AddListener(delegate { OpenToolsCategory(); });

        survivalButton = craftingUI.transform.Find("Survival_Button").GetComponent<Button>();
        survivalButton.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        processButton = craftingUI.transform.Find("Process_Button").GetComponent<Button>();
        processButton.onClick.AddListener(delegate { OpenProcessCategory(); });

        //Axe
        axeReq1 = toolsUI.transform.Find("Axe").transform.Find("Req1").GetComponent<TextMeshProUGUI>();
        axeReq2 = toolsUI.transform.Find("Axe").transform.Find("Req2").GetComponent<TextMeshProUGUI>();

        craftAxeButton = toolsUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftItem(AxeBlueprint); });

        //Plank
        plankReq1 = processUI.transform.Find("Plank").transform.Find("Req1").GetComponent<TextMeshProUGUI>();

        craftPlankButton = processUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankButton.onClick.AddListener(delegate { CraftItem(PlankBlueprint); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            OpenCraftingScreen();
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            CloseCraftingScreen();
        }
    }

    void OpenToolsCategory()
    {
        craftingUI.SetActive(false);
        toolsUI.SetActive(true);
    }

    private void OpenSurvivalCategory()
    {
        craftingUI.SetActive(false);
        survivalUI.SetActive(true);
    }

    private void OpenProcessCategory()
    {
        craftingUI.SetActive(false);
        processUI.SetActive(true);
    }

    void CraftItem(ItemBlueprint itemBlueprint)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        for (int i = itemBlueprint.numberOfItemsToSpawn; i > 0; i--)
        {
            InventorySystem.Instance.AddItemToInventory(itemBlueprint.itemName);
        }

        if (itemBlueprint.numberOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItemFromInventory(itemBlueprint.req1, itemBlueprint.req1Amount);
        } 
        else if(itemBlueprint.numberOfRequirements == 2)
        {

            InventorySystem.Instance.RemoveItemFromInventory(itemBlueprint.req1, itemBlueprint.req1Amount);
            InventorySystem.Instance.RemoveItemFromInventory(itemBlueprint.req2, itemBlueprint.req2Amount);
        }

        StartCoroutine(Calculate());
    }

    IEnumerator Calculate()
    {
        yield return 0;

        InventorySystem.Instance.RefreshItemList();
        RefreshNeededItems();
    }

    public void RefreshNeededItems()
    {
        int stoneCount = 0;
        int stickCount = 0;
        int logCount = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case Name.ResourceRock:
                    stoneCount++;
                    break;

                case Name.ResourceStick:
                    stickCount++;
                    break;

                case Name.ResourceLog:
                    logCount++;
                    break;
            }
        }

        // Axe
        axeReq1.text = "3 rock [" + stoneCount + "]";
        axeReq2.text = "3 stick [" + stickCount + "]";
        
        if(stoneCount >= 3 && stickCount >= 3 && InventorySystem.Instance.CheckAvailableSlots(1))
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }

        // Plank x2
        plankReq1.text = "1 Log [" + logCount + "]";

        if (logCount >= 1 && InventorySystem.Instance.CheckAvailableSlots(2))
        {
            craftPlankButton.gameObject.SetActive(true);
        }
        else
        {
            craftPlankButton.gameObject.SetActive(false);
        }
    }

    void OpenCraftingScreen()
    {
        craftingUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();

        isOpen = true;
    }

    void CloseCraftingScreen()
    {
        craftingUI.SetActive(false);
        toolsUI.SetActive(false);
        survivalUI.SetActive(false);
        processUI.SetActive(false);

        if (!InventorySystem.Instance.isOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
        }
        isOpen = false;
    }
}
