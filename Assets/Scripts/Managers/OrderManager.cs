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
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI feedbackText;
    
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] ingredientPrefabs;

    private Dictionary<string, GameObject> ingredientDictionary = new Dictionary<string, GameObject>();
    private List<string> currentOrder = new List<string>();
    private float timeLeft;
    private int score = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (GameObject prefab in ingredientPrefabs)
        {
            ingredientDictionary[prefab.name] = prefab;
        }
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
            ShowFeedback("Order Timeout!", Color.red);
            GenerateNewOrder();
        }
    }

    void GenerateNewOrder()
    {
        RespawnUsedIngredients();
        
        currentOrder.Clear();
        int ingredientCount = Random.Range(2, 4);

        for (int i = 0; i < ingredientCount; i++)
        {
            string ingredient = possibleIngredients[Random.Range(0, possibleIngredients.Count)];
            if (!currentOrder.Contains(ingredient))
                currentOrder.Add(ingredient);
        }

        orderText.text = "Order: " + string.Join(" ", currentOrder);
        timeLeft = 30f;
    }

    public void CheckOrder(string cookedDish)
    {
        if (cookedDish == GenerateOrderName())
        {
            score += 10;
            scoreText.text = score.ToString();
            ShowFeedback("Order Delivered!", Color.green);
        }
        else
        {
            ShowFeedback("Wrong Order!", Color.red);
        }
        GenerateNewOrder();
    }

    void ShowFeedback(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;
        StartCoroutine(ClearFeedback());
    }

    IEnumerator ClearFeedback()
    {
        yield return new WaitForSeconds(2f);
        feedbackText.text = "";
    }

    public string GenerateOrderName()
    {
        List<string> cleanedNames = new List<string>();

        foreach (string ingredient in currentOrder)
        {
            string[] words = ingredient.Split(' ');
            cleanedNames.Add(words[0]);
        }

        return string.Join(" ", cleanedNames) + " Elixir";
    }

    void RespawnUsedIngredients()
    {
        foreach (string ingredientName in currentOrder)
        {
            if (ingredientDictionary.ContainsKey(ingredientName))
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(ingredientDictionary[ingredientName], spawnPoints[randomIndex].position, Quaternion.identity);
            }
        }
    }
}
