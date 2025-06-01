using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardDragHandler : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler
{

    Transform origanalTransformParent;
    Vector3 origanalPosition;
    Canvas canvas;
    CanvasGroup canvasGroup;
    GameObject tempCard;
    int originalSiblingIndex;
    DeckManager deckManager;

    private void Awake()
    {
     canvas = GetComponentInParent<Canvas>();
        deckManager = FindObjectOfType<DeckManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        origanalTransformParent = transform.parent;
        origanalPosition = transform.localPosition;
        originalSiblingIndex = transform.GetSiblingIndex();

        tempCard = Instantiate(gameObject, canvas.transform);
        tempCard.transform.position = transform.position;

        CanvasGroup tempCanvasGroup = tempCard.GetComponent<CanvasGroup>();
        if (tempCanvasGroup == null)
            tempCanvasGroup = tempCard.AddComponent<CanvasGroup>();

        tempCanvasGroup.blocksRaycasts = false;
        tempCanvasGroup.alpha = 0.7f;


        gameObject.GetComponent<Image>().enabled = false;

        tempCard.transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 uiPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out uiPosition
        );
        
        tempCard.transform.localPosition = uiPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject target=eventData.pointerEnter;
        if (target != null)
        {
            Transform targetparent=target.transform.parent;
            int targetSiblingIndex = target.transform.GetSiblingIndex();

            target.transform.SetParent(origanalTransformParent);
            target.transform.position = origanalPosition;
            target.transform.SetSiblingIndex(originalSiblingIndex);


            transform.SetParent (targetparent);
            transform.SetSiblingIndex(targetSiblingIndex);
        }
        else
        {
            transform.SetParent(origanalTransformParent);
            transform.SetSiblingIndex(originalSiblingIndex);
        }
        gameObject.GetComponent<Image>().enabled = true;
        deckManager.CheckAllPanels();
        Destroy(tempCard);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
