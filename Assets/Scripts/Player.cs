using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public int health = 30;
    public Text healthText;
    public int playerIndex = 0;
    public GameObject deathEffect;

    private GameManager gm;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();
        healthText.text = health.ToString();
    }

    private void OnMouseDown()
    {
            //TakeDamage(1);

        Debug.Log("Plyaerr");
        if (gm.IsOpponentInPlay(playerIndex))
        {
            TakeDamage(gm.GetAttackDamage(playerIndex));
           gm.UseCardInPlay();
        }
    }
    public void TakeDamage(int atkPoints)
    {
        anim.SetTrigger("Hurt");
        health -= atkPoints;
        healthText.text = health.ToString();
        healthText.color = Color.red;

        if (health > 0)
        Invoke("RestoreToNeutral", 2f);
        else 
        Invoke("Death", 1f);

    }

    void RestoreToNeutral() {
        healthText.color = Color.white;

    }

    void Death() {
        Instantiate(deathEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        gm.PlayExplodeSound();
        anim.SetTrigger("Death");
        FindObjectOfType<CameraShake>().ShakeCamera();
        gm.WinGame(playerIndex + 1);
    }
}
