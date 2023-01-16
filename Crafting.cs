using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{

    public bool isInRange;
    private bool displayingCraftingAbility;
    public bool isCrafting;

    [SerializeField] Transform craftingCheck;
    [SerializeField] LayerMask playerMask;
    [SerializeField] Transform playerTransform;
    GameObject craftingAbilityDisplay;

    float playerDistance = 1.75f;

    [SerializeField] public GameObject craftingUI;

    void Start() 
    {
        craftingAbilityDisplay = transform.Find("Crafting Ability Display Holder").gameObject;
        craftingAbilityDisplay.SetActive(false);
    }
    
    void Update()
    {
        isInRange = Physics.CheckSphere(craftingCheck.position, playerDistance, playerMask);

        AbilityDisplayer();

        CraftingMenu();
    }



    void AbilityDisplayer()
    {
        if (isInRange)
        {
            if (!displayingCraftingAbility)
            {
                displayingCraftingAbility = true;
                Debug.Log("Displaying");

                craftingAbilityDisplay.SetActive(true);
            }

            craftingAbilityDisplay.transform.LookAt(playerTransform);

        }

        else if (!isInRange)
        {
            if (displayingCraftingAbility)
            {
                displayingCraftingAbility = false;
                Debug.Log("Not Displaying");

                craftingAbilityDisplay.SetActive(false);
            }
        }
    }



    void CraftingMenu()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.G))
        {
            if (isCrafting)
            {
                CraftStart();
            }
            else
            {
                CraftEnd();
            }
        }
    }



    private void CraftStart()
    {
        craftingUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        
    }



    private void CraftEnd()
    {
        craftingUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1f;

        
    }
}
