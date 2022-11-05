using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    Animator animator;

    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    public bool isPlayerInRange;
    public bool canBeChopped;

    void Start()
    {
        animator = transform.parent.transform.parent.GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if(canBeChopped)
        {
            GlobalState.Instance.SetHealth(maxHealth, currentHealth);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Name.TagPlayer))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(Name.TagPlayer))
        {
            isPlayerInRange = false;
        }
    }

    public void GetHit(float amount)
    {
        animator.SetTrigger("Shake_Trig");

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            KillTree();
        }
    }

    void KillTree()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        SpawnChoppedTree(treePosition);
    }

    void SpawnChoppedTree(Vector3 spawnPosition)
    {
        Instantiate(Resources.Load<GameObject>(Name.ResourceChoppedTree), spawnPosition, Quaternion.Euler(0, 0, 0));
    }
}
