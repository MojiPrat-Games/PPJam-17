using UnityEngine;

public class DeliveryZone : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (PickupThrowLogic.Instance.IsHoldingDish())
        {
            string dishName = PickupThrowLogic.Instance.GetHeldDishName();
            OrderManager.Instance.CheckOrder(dishName);
            PickupThrowLogic.Instance.DeliverHeldDish();
        }
    }
}
