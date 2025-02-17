using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddListenerToDownButton : MonoBehaviour
{
    public GameObject player;
    private EventTrigger trigger;

    void Start()
    {
        trigger = GetComponent<EventTrigger>(); // Cache EventTrigger
        if (trigger == null)
        {
            Debug.LogError("EventTrigger component is missing!");
            return;
        }

        StartCoroutine(FindAndAssignPlayer());
    }

    IEnumerator FindAndAssignPlayer()
    {
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Debug.Log("Player found: " + player.name);
                AddEventListeners();
                break; // Exit loop once found
            }

            yield return new WaitForSeconds(0.5f); // Retry every 0.5s
        }
    }

    void AddEventListeners()
    {
        if (trigger == null || player == null)
        {
            Debug.LogWarning("EventTrigger or Player is null. Cannot add listeners.");
            return;
        }

        trigger.triggers.Clear();

        // Create PointerDown event
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDownEntry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });

        // Create PointerUp event
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data); });

        // Add events to the EventTrigger component
        trigger.triggers.Add(pointerDownEntry);
        trigger.triggers.Add(pointerUpEntry);

        Debug.Log("Event listeners added successfully.");
    }

    public void RemoveEventListeners()
    {
        if (trigger != null)
        {
            trigger.triggers.Clear();
            Debug.Log("Event listeners removed.");
        }
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        if (player == null) return;

        Debug.Log("OnPointerDownDelegate called.");
        player.GetComponent<ForkController>().StartMovingDown();
    }

    public void OnPointerUpDelegate(PointerEventData data)
    {
        if (player == null) return;

        Debug.Log("OnPointerUpDelegate called.");
        player.GetComponent<ForkController>().StopMovingDown();
    }
}
