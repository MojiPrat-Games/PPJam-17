using UnityEngine;
using System.Collections.Generic;

public class CookingPot : MonoBehaviour
{
    public float cookingTime = 10f; // Time before the dish is finalized
    private List<string> ingredientNames = new List<string>();
    private bool isCooking = false;

    void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();
        
        if(!PickupThrowLogic.Instance.IsHolding())
        {
            if (ingredient != null)
            {
                ingredientNames.Add(ingredient.ingredientName);
                Debug.Log("You added " + ingredient.ingredientName);
                Destroy(other.gameObject); // Remove ingredient once thrown in
            }
        }
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCooking();
        }
    }

    public void StartCooking()
    {
        if (!isCooking && ingredientNames.Count > 0)
        {
            isCooking = true;
            Invoke(nameof(FinishCooking), cookingTime);
        }
    }

    void FinishCooking()
    {
        if (ingredientNames.Count > 0)
        {
            string finalDish = GenerateDishName();
            Debug.Log("You made: " + finalDish);
        }
        else
        {
            Debug.Log("You didn't add any ingredients!");
        }
        isCooking = false;
        ingredientNames.Clear();
    }

    string GenerateDishName()
    {
        if (ingredientNames.Count == 1) return ingredientNames[0] + " Soup";

        string prefix = ingredientNames[0]; // First ingredient
        string suffix = ingredientNames[ingredientNames.Count - 1]; // Last ingredient
        return prefix + " " + suffix + " Stew";
    }
}
