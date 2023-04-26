using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _radius = 3f;
    [SerializeField] private LayerMask _interactableLayerMask = 6;

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _interactableLayerMask);
            foreach (Collider hit in hits)
            {
                hit.GetComponent<Interactable>().Interact();
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
