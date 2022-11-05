using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen
            && !SelectionManager.Instance.isTakeImgVisible)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound, 0.2f);

            animator.SetTrigger("Hit");
        }
    }

    //This function is being called in the middle of hit animation
    public void Hit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);

            selectedTree.GetComponent<ChoppableTree>().GetHit(3);
        }
    }
}
