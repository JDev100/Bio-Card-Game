using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public int health = 30;
    public Text healthText;
    public int playerIndex = 0;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        healthText.text = health.ToString();
    }

    private void OnMouseDown()
    {
            //TakeDamage(1);

        Debug.Log("Plyaerr");
        if (gm.IsOpponentInPlay(playerIndex))
        {
            TakeDamage(5);
           gm.UseCardInPlay();
        }
    }
    public void TakeDamage(int atkPoints)
    {
        health -= atkPoints;
        healthText.text = health.ToString();
        healthText.color = Color.red;

        Invoke("RestoreToNeutral", 2f);

    }

    void RestoreToNeutral() {
        healthText.color = Color.white;

    }
}
