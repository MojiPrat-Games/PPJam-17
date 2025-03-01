using UnityEngine;

public class PickupDrop : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupRange = 3f;
    public LayerMask pickupLayer;
    private GameObject heldItem;
    private GameObject highlightedItem;

    void Update()
    {
        HandleHighlight();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null) TryPickup();
            else DropItem();
        }
    }

    void HandleHighlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            if (highlightedItem != hit.collider.gameObject)
            {
                if (highlightedItem != null) ResetHighlight();
                highlightedItem = hit.collider.gameObject;
                Debug.Log("Holding " + highlightedItem.GetComponent<Ingredient>().ingredientName);
                ToggleOutline(highlightedItem, true);
            }
        }
        else if (highlightedItem != null)
        {
            ResetHighlight();
        }
    }

    void ResetHighlight()
    {
        if (highlightedItem != null) ToggleOutline(highlightedItem, false);
        highlightedItem = null;
    }

    void ToggleOutline(GameObject obj, bool state)
    {
        Transform outline = obj.transform.Find("Outline_Mesh");
        if (outline) outline.gameObject.SetActive(state);
    }

    void TryPickup()
    {
        if (highlightedItem == null) return;
        heldItem = highlightedItem;
        ResetHighlight(); 
        heldItem.transform.SetParent(holdPoint);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
    }

    void DropItem()
    {
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.transform.SetParent(null);
        heldItem = null;
    }
}
