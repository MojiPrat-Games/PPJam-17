using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickupThrowLogic : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupRange = 3f;
    public LayerMask pickupLayer;
    public float minThrowForce = 5f;
    public float maxThrowForce = 20f;
    public float chargeSpeed = 10f;
    public Image powerBar;

    private GameObject heldItem;
    private GameObject highlightedItem;
    private bool isCharging = false;
    private float throwForce;
    private Coroutine followCoroutine;
    
    public static PickupThrowLogic Instance { get; private set; }

    private bool isHolding = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        HandleHighlight();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null) TryPickup();
            else StartChargingThrow();
        }

        if (isCharging) ChargeThrow();

        if (Input.GetKeyUp(KeyCode.E) && isCharging) ThrowItem();
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
        isHolding = true;

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = false;  
        rb.isKinematic = false;

        followCoroutine = StartCoroutine(FollowHoldPoint(rb));
    }

    IEnumerator FollowHoldPoint(Rigidbody rb)
    {
        while (heldItem != null)
        {
            Vector3 targetPos = holdPoint.position;
            rb.linearVelocity = (targetPos - rb.position) * 10f; 
            yield return null;
        }
    }

    void StartChargingThrow()
    {
        isCharging = true;
        throwForce = minThrowForce;
        powerBar.gameObject.SetActive(true);
    }

    void ChargeThrow()
    {
        throwForce += chargeSpeed * Time.deltaTime;
        throwForce = Mathf.Clamp(throwForce, minThrowForce, maxThrowForce);
        powerBar.fillAmount = (throwForce - minThrowForce) / (maxThrowForce - minThrowForce);
    }

    void ThrowItem()
    {
        isCharging = false;
        isHolding = false;
        powerBar.gameObject.SetActive(false);

        if (heldItem != null)
        {
            StopCoroutine(followCoroutine);
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true; 
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            heldItem.transform.SetParent(null);
            rb.constraints = RigidbodyConstraints.None;
            heldItem = null;
        }
    }

    public string GetHighligtedItemName()
    {
        return highlightedItem.GetComponent<Ingredient>().ingredientName;
    }

    public bool IsHolding()
    {
        return isHolding;
    }
}
