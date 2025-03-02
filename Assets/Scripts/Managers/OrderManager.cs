using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    [SerializeField] private List<string> possibleIngredients = new List<string> { "Dragon Essence", "Frosthorn Leaf", "Lunar Crystal", "Sap Potion", "Thunderwin Twig" };

    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float orderTime = 30f;
    [SerializeField] private int score = 0;

    private List<string> currentOrder = new List<string>();
    private float timeLeft;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateNewOrder();
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Time Left: " + Mathf.Ceil(timeLeft);
        }
        else
        {
            Debug.Log("Order failed!");
            GenerateNewOrder(); // Move to next order when time runs out
        }
    }

    void GenerateNewOrder()
    {
        currentOrder.Clear();
        int ingredientCount = Random.Range(2, 4); // Orders have 2-3 ingredients

        for (int i = 0; i < ingredientCount; i++)
        {
            string ingredient = possibleIngredients[Random.Range(0, possibleIngredients.Count)];
            if (!currentOrder.Contains(ingredient)) // Prevent duplicates
                currentOrder.Add(ingredient);
        }

        orderText.text = "Order: " + string.Join(" + ", currentOrder);
        timeLeft = orderTime;
    }

    public void CheckOrder(string cookedDish)
    {
        if (cookedDish == GenerateOrderName()) // Compare with expected dish name
        {
            score += 10;
            Debug.Log("Correct Order! Score: " + score);
        }
        else
        {
            Debug.Log("Wrong Order! No points.");
        }
        GenerateNewOrder();
    }

    public string GenerateOrderName()
    {
        return currentOrder[0] + " " + currentOrder[currentOrder.Count - 1] + " Stew";
    }
}
