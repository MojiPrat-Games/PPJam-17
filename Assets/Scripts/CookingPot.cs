using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CookingPot : MonoBehaviour
{
    public float cookingTime = 10f;
    private List<string> ingredientNames = new List<string>();
    private bool isCooking = false;
    public TextMeshProUGUI cookingTimerText;
    private float currentTime = 0f;

    public GameObject dishPrefab; // assign a dish prefab in inspector
    public Transform spawnPoint; // set a spawn position for finished dish

    [SerializeField] private float throwForce;

    void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();
        
        if(!PickupThrowLogic.Instance.IsHolding())
        {
            if (ingredient != null)
            {
                ingredientNames.Add(ingredient.ingredientName);
                Debug.Log("You added " + ingredient.ingredientName);
                Destroy(other.gameObject);
            }
        }
    }

    void Update()
    {
        if (isCooking)
        {
            currentTime -= Time.deltaTime;
            cookingTimerText.text = "Cooking: " + Mathf.Ceil(currentTime).ToString() + "s";

            if (currentTime <= 0)
            {
                FinishCooking();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCooking();
        }
    }

    public void StartCooking()
    {
        if (!isCooking && ingredientNames.Count > 0)
        {
            isCooking = true;
            currentTime = cookingTime;
            cookingTimerText.gameObject.SetActive(true);
        }
    }

    void FinishCooking()
    {
        if (ingredientNames.Count > 0)
        {
            string finalDish = GenerateDishName();
            Debug.Log("You made: " + finalDish);
            OrderManager.Instance.CheckOrder(finalDish);
        }
        else
        {
            Debug.Log("You didn't add any ingredients!");
        }
        isCooking = false;
        ingredientNames.Clear();
    }


    void SpawnDish(string dishName)
    {
        GameObject dish = Instantiate(dishPrefab, spawnPoint.position, Quaternion.identity);
        dish.GetComponent<Rigidbody>().AddForce(Vector3.up * throwForce);
        dish.name = dishName;
        dish.GetComponent<FinalProduct>().finalProductName = dishName;
    }

    string GenerateDishName()
    {
        return string.Join(" ", ingredientNames) + " Elixir"; // Exact match with OrderManager
    }
}
