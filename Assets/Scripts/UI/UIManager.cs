using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Sprite[] toggleSprites;
    public Image musicToggle;
    AudioSource audioSource;

    public Dictionary<string, inventory_UI> inventoryUIByName = new Dictionary<string, inventory_UI>();
    public List<inventory_UI> inventoryUIs;

    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

    public GameObject inventoryPanel;
    public Button openIventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            dragSingle=true;
        }
        else{
            dragSingle=false;
        }
    }

    private void Awake()
    {
        Initialize();
        if(inventoryPanel!=null){
            inventoryPanel.SetActive(false);
            openIventory.onClick.AddListener(ToggleInventory);
        }
    }

    public void ToggleInventory(){
        if(inventoryPanel !=null)
        {
            if(!inventoryPanel.activeSelf){
                inventoryPanel.SetActive(true);
                RefreshInventoryUI("Backpack");
            }
            else{
                inventoryPanel.SetActive(false);
            }
        }
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if(inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    public void RefreshAll(){
        foreach(KeyValuePair<string, inventory_UI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }

    public inventory_UI GetInventoryUI(string inventoryName)
    {
        if(inventoryUIByName.ContainsKey(inventoryName)){
            return inventoryUIByName[inventoryName];
        }
        Debug.LogWarning("There is not inventory ui for " + inventoryName);
        return null;
    }

    void Initialize()
    {
        foreach(inventory_UI ui in inventoryUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName)){
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }

    private void SpriteChange() {
        if (musicToggle.sprite == toggleSprites[0]) { // turn off
            musicToggle.sprite = toggleSprites[1];
            return;
        }
        musicToggle.sprite = toggleSprites[0]; // turn on
    }

    public void OnMusicToggleButtonClick() {
        SpriteChange();
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
        else {
            audioSource.Pause();
        }

    }
}
