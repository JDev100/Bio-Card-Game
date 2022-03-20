using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Turn System
    int currentPlayerTurn = 0;

    public List<Card> deckP1 = new List<Card>();
    public List<Card> deckP2 = new List<Card>();
    public List<Card> discardPileP1 = new List<Card>();
    public List<Card> discardPileP2 = new List<Card>();
    public Transform[] cardSlotsP1;
    public Transform[] cardSlotsP2;
    public bool[] availableCardSlotsP1;
    public bool[] availableCardSlotsP2;

    public Text deckSizeText;
    public Text discardPileText;
    public void DrawCard(int player)
    {
        if (player == 0 && currentPlayerTurn == 0)
        {
            if (deckP1.Count >= 1)
            {
                Card randCard = deckP1[Random.Range(0, deckP1.Count)];

                for (int i = 0; i < availableCardSlotsP1.Length; i++)
                {
                    if (availableCardSlotsP1[i] == true)
                    {
                        randCard.gameObject.SetActive(true);
                        randCard.transform.position = cardSlotsP1[i].position;
                        randCard.hasBeenPlayed = false;
                        randCard.handIndex = i;
                        availableCardSlotsP1[i] = false;
                        deckP1.Remove(randCard);
                        return;
                    }
                }
            }
        }
    }

    public void Shuffle()
    {
        if (discardPileP1.Count >= 1)
        {
            foreach (Card card in discardPileP1)
            {
                deckP1.Add(card);
            }
            discardPileP1.Clear();
        }
    }

    private void Update()
    {
        deckSizeText.text = deckP1.Count.ToString();
        discardPileText.text = discardPileP1.Count.ToString();
    }
}
