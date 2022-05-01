using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public bool isSelected = false;
    Vector3 currentTransform;
    Vector3 target;

    GameManager gm;

    Animator anim;


    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        target = transform.position;
        anim = GetComponent<Animator>();
    }


    float timeElapsed;
    float lerpDuration = .1f;
    float startValue = 0;
    float endValue = 10;
    float valueToLerp;

    bool pressed = false;
    void Update()
    {
        if (pressed)
        {
            if (timeElapsed < lerpDuration)
            {
                if (isSelected)
                {
                    transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(3.5f, 3.5f, 1), timeElapsed / lerpDuration);
                }
                else
                    transform.localScale = Vector3.Lerp(new Vector3(3.5f, 3.5f, 1), new Vector3(1, 1, 1), timeElapsed / lerpDuration);

                valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                transform.position = Vector3.Lerp(currentTransform, target, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            else pressed = false;
        }

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if ((eventData.button == PointerEventData.InputButton.Right || (isSelected &&eventData.button == PointerEventData.InputButton.Left)) && !GetComponent<Card>().hasBeenClicked && gm.canPlay &&(!gm.cardUp || (gm.cardUp && isSelected)))
        {
            pressed = true;
            Debug.Log("Poop");
            anim.SetTrigger("Toggle");
            gm.ToggleInput();
            if (!isSelected)
            {
                gm.ToggleDeckHolder(GetComponent<Card>().GetCardOwner());
            gm.PlaySelectSound();
                currentTransform = transform.position;
                target = new Vector3(0, 0, 10);
                timeElapsed = 0;
                isSelected = true;
                gameObject.transform.SetAsLastSibling();
                gm.cardUp = true;
            }
            else
            {
                gm.PlayBackSound();
                gm.cardUp = false;
                target = currentTransform;
                isSelected = false;
                timeElapsed = 0;

            }
        }

    }
}
