using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FoodMachine : MonoBehaviour
{
    [Header("Food Item")]
    public SpriteRenderer foodHolder;
    public ItemData foodItem;

    [Header("Door")]
    public SpriteRenderer doorRenderer;
    public Sprite doorClosed;
    public Light2D smallLight;

    [Header("Control Machine")]
    public SpriteRenderer controlMachine;
    public Sprite controlMachineOn;
    public Sprite controlMachineOff;
    public Light2D machineLight;

    [Header("Machine")]
    public Light2D machineCoreLight;
    public CircleCollider2D detectionZone;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorRenderer.sprite = doorClosed;
        controlMachine.sprite = controlMachineOff;
        smallLight.enabled = false;
        machineLight.enabled = false;
        machineCoreLight.enabled = false;

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            doorRenderer.sprite = null; // Open the door
            controlMachine.sprite = controlMachineOn; // Turn on the control machine
            smallLight.enabled = true; // Enable small light
            machineLight.enabled = true; // Enable machine light
            machineCoreLight.enabled = true; // Enable core light
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            doorRenderer.sprite = doorClosed; // Close the door
            controlMachine.sprite = controlMachineOff; // Turn off the control machine
            smallLight.enabled = false; // Disable small light
            machineLight.enabled = false; // Disable machine light
            machineCoreLight.enabled = false; // Disable core light
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
