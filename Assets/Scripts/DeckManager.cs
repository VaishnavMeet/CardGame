using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public List<CardData> allCards;
    public GameObject cardPrefab;
    public List<Transform> cardPanel;
    public Text[] resultTextBoxes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateCard(4, cardPanel[0]);
        GenerateCard(3, cardPanel[1]);
        GenerateCard(4, cardPanel[2]);
        CheckAllPanels();
    }


    List<CardData> GetCardDatas(Transform panel)
    {
        List<CardData> result = new List<CardData>();
        foreach (Transform child in panel)
        {
            CardInfo cardInfo = child.GetComponent<CardInfo>();
            result.Add(cardInfo.cardData);
        }
        return result;
    }


    string FindPAttern(List<CardData> cards)
    {
        if (cards.Count < 3)
            return "Not enough cards";

        var sorted = cards.OrderBy(c => c.rank).ToList();
        bool allSameRank = cards.All(c => c.rank == cards[0].rank);
        bool allSameSuit = cards.All(c => c.suit == cards[0].suit);
        bool isSequence = true;

        for (int i = 1; i < 3; i++)
        {
            if ((int)sorted[i].rank != (int)sorted[i - 1].rank + 1)
            {
                isSequence = false;
                break;
            }
        }

        var rankGroups = cards.GroupBy(c => c.rank).ToList();
        var countGroups = rankGroups.Select(g => g.Count()).ToList();

        if (allSameRank)
            return "Triple";
        if (isSequence && allSameSuit)
            return "Straight Flush";
        if (isSequence)
            return "Sequence";
        if (countGroups.Contains(3))
            return "Triple";
        if (allSameSuit)
            return "Colour";
        if (countGroups.Contains(2))
            return "Duo";
        

        return "High Card";
    }


    void GenerateCard(int num,Transform transform)
    {
        List<CardData> templist= new List<CardData>(allCards);
        for (int i = 0; i < num; i++)
        {
            if(templist.Count == 0)break;


            int randomIndex=Random.Range(0, templist.Count);
            CardData card = templist[randomIndex];
            GameObject gameObject = Instantiate(cardPrefab,transform);
          
            gameObject.GetComponent<Image>().sprite= card.artwork;
            gameObject.GetComponent<CardInfo>().cardData = card;
            templist.Remove(card);
        }
    }

    public void CheckAllPanels()
    {
        for (int i = 0; i < cardPanel.Count; i++)
        {
            var cards = GetCardDatas(cardPanel[i]);
            string result = FindPAttern(cards);
            resultTextBoxes[i].text = result;
            
        }
    }

}
