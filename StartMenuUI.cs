using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class StartMenuUI : MonoBehaviour
{

    [SerializeField] private Animator animator;

    
    void Start()
    {
        
    }

    

    void Update()
    {
        
        //Pseudocode animation implementation:

        //start with default animation (no code needed)

        //if mouse is over ui object but not clicked
            //play selected animation
        if (IsMouseOverUI() && !Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isSelected", true);
            animator.SetBool("isDeselected", false);
            animator.SetBool("isClicked", false);
        }

        //if mouse is over ui object and clicked
            //play clicked animation
        else if (IsMouseOverUI() && Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isSelected", false);
            animator.SetBool("isDeselected", true);
            animator.SetBool("isClicked", false);
        }

        //if mouse is over ui object and is moved away from it
            //play deselected animation
        else if (animator.GetBool("isSelected") && !IsMouseOverUI())
        {
            animator.SetBool("isSelected", false);
            animator.SetBool("isDeselected", false);
            animator.SetBool("isClicked", true);
        }
    }



    public bool IsMouseOverUI() 
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
