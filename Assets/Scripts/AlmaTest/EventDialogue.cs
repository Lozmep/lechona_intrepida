using System.Collections.Generic;
using DialogueSystem;
using EventManager;
using SignalSystem;
using UnityEngine;
using System.IO;

public class EventDialogue : MonoBehaviour
{
    private TakeItemEvent takeItem;

    [Header("Raycast Info")]
    private RaycastHit hit;
    private Vector3 origin;
    private Vector3 direction;
    public float maxDistance = 1f;
    public LayerMask layerMask;
    public float verticalDistance = 0.9f;
    public bool isIndicated;

    [Header("Event Management")]
    public EventManager.EventManager eventManager;

    [Header("Dialogue Management")]
    public DialogueManager dialogueManager;

    [Header("Initial Dialogue information")]
    public List<InitialDialogue> initDialoguePool = new List<InitialDialogue>();

    private void Awake()
    {
        takeItem = GetComponent<TakeItemEvent>();
    }

    void Start()
    {
        //LoadEventsFromFile("Assets/Data/initialeventext.json");
        string path = Path.Combine(Application.streamingAssetsPath, "initialeventext.json");
        LoadEventsFromFile(path);
        isIndicated = false;
    }

    private void LoadEventsFromFile(string path)
    {
        if (File.Exists(path))
        {
            string jsonContent = File.ReadAllText(path);
            initDialoguePool = JsonUtility.FromJson<InitialDialogueList>(jsonContent).initialeventext;
        }
    }
    void Update()
    {
        Vector3 direction = transform.forward;
        Vector3 origin = transform.position + new Vector3(0, verticalDistance, 0);

        if (eventManager.currentEvent == null || eventManager.currentEvent.mainIndicator == IndicatorType.Communication) return;

        if (Input.GetKey(KeyCode.X))
        {
            Debug.DrawRay(origin, direction * maxDistance, Color.red);
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
            {
                NPC npc = hit.collider.GetComponent<NPC>();
                if (npc.ID != eventManager.currentNPC) {
                    if (!dialogueManager.isActive) {
                        dialogueManager.IntTxt();
                    }
                    return;
                }

                if (!dialogueManager.isActive)
                {
                    InitialDialogue compareID = null;
                    foreach (var initialDialogue in initDialoguePool)
                    {
                        if (initialDialogue.id == eventManager.currentEvent.id)
                        {
                            compareID = initialDialogue;
                            StartCoroutine(dialogueManager.Speak(compareID.dialogue.spanish));
                            isIndicated = true;
                            break;
                        }
                    }
                    
                }

            }
        }

    }

}

[System.Serializable]
public class InitialDialogueList
{
    public List<InitialDialogue> initialeventext;
}

[System.Serializable]
public class InitialDialogue
{
    public int id;
    public InitDialogueScript dialogue;
}

[System.Serializable]
public class InitDialogueScript
{
    public InitEventLines[] spanish;
    public InitEventLines[] english;
}

[System.Serializable]
public class InitEventLines
{
    public string character;
    public string[] text;
}


