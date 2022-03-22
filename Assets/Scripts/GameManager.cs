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

    Card cardInPlayP1;
    Card cardInPlayP2;

    public Text deckSizeTextP1;
    public Text deckSizeTextP2;
    public Text discardPileTextP1;
    public Text discardPileTextP2;
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
                        randCard.SetCardOwner(0);
                        deckP1.Remove(randCard);
                        return;
                    }
                }
            }
        }
        if (player == 1 && currentPlayerTurn == 1)
        {
            if (deckP2.Count >= 1)
            {
                Card randCard = deckP2[Random.Range(0, deckP2.Count)];

                for (int i = 0; i < availableCardSlotsP2.Length; i++)
                {
                    if (availableCardSlotsP2[i] == true)
                    {
                        randCard.SetCardOwner(1);
                        randCard.gameObject.SetActive(true);
                        randCard.transform.position = cardSlotsP2[i].position;
                        randCard.hasBeenPlayed = false;
                        randCard.handIndex = i;
                        availableCardSlotsP2[i] = false;
                        deckP2.Remove(randCard);
                        return;
                    }
                }
            }
        }
    }

    public void SetCardInPlay(Card card)
    {
        if (card.GetCardOwner() == 0)
        {
            if (cardInPlayP1 == null)
            {
                cardInPlayP1 = card;
                card.SetCardToPlay();
            }
            else
            {
                cardInPlayP1.RemoveCardFromPlay();
                if (cardInPlayP1 != card) {
                    cardInPlayP1 = card;
                    card.SetCardToPlay();
                }
                else
                cardInPlayP1 = null;

            }
        }
          if (card.GetCardOwner() == 1)
        {
            if (cardInPlayP2 == null)
            {
                cardInPlayP2 = card;
                card.SetCardToPlay();
            }
            else
            {
                cardInPlayP2.RemoveCardFromPlay();
                if (cardInPlayP2 != card) {
                    cardInPlayP2 = card;
                    card.SetCardToPlay();
                }
                else
                cardInPlayP2 = null;

            }
        }
        
    }

    public void Shuffle(int player)
    {
        if (player == 0)
        {
            foreach (Card card in discardPileP1)
            {
                deckP1.Add(card);
            }
            discardPileP1.Clear();
        }
        if (player == 1)
        {
            foreach (Card card in discardPileP2)
            {
                deckP2.Add(card);
            }
            discardPileP2.Clear();
        }
        if (discardPileP1.Count >= 1)
        {
            foreach (Card card in discardPileP1)
            {
                deckP1.Add(card);
            }
            discardPileP1.Clear();
        }
    }

    public void EndTurn()
    {
        if (!cardInPlayP1 && !cardInPlayP2) {
        Shuffle(currentPlayerTurn);
        if (currentPlayerTurn == 0)
        {
            currentPlayerTurn = 1;
        }
        else
            currentPlayerTurn = 0;
        }
    }

    private void Update()
    {
        deckSizeTextP1.text = deckP1.Count.ToString();
        discardPileTextP1.text = discardPileP1.Count.ToString();
        deckSizeTextP2.text = deckP2.Count.ToString();
        discardPileTextP2.text = discardPileP2.Count.ToString();
    }

    public int CurrentPlayerTurn()
    {
        return currentPlayerTurn;
    }
}
