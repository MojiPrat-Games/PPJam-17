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
        isCooking = false;
        cookingTimerText.gameObject.SetActive(false);

        if (ingredientNames.Count > 0)
        {
            string finalDish = GenerateDishName();
            Debug.Log("You made: " + finalDish);

            SpawnDish(finalDish);
        }
        else
        {
            Debug.Log("You didn't add any ingredients!");
        }
        
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
        if (ingredientNames.Count == 1) return ingredientNames[0];
        string prefix = ingredientNames[0]; 
        string suffix = ingredientNames[ingredientNames.Count - 1];
        return prefix + " " + suffix + " Elixir";
    }
}
