using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interacty : MonoBehaviour
{
    private GameObject[] allCollectibles;
    private int collectedCount;
    public string collectibleTag = "Gem";
    public bool allCollected = false;
    public Player player;

    // public GameObject collection;
    public TextMeshProUGUI gemsCollectedText;

    // Start is called before the first frame update
    void Start()
    {
        // gemsCollectedText = collection.GetComponent<TextMeshProUGUI>();
        allCollectibles = GameObject.FindGameObjectsWithTag(collectibleTag);
        gemsCollectedText.SetText("0/" + allCollectibles.Length.ToString());
        player = gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // if(collectedCount == allCollectibles.Length && !allCollected){
        //     allCollected = true;
        //     Debug.Log("Collected em all!");
        // }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == collectibleTag){
            Debug.Log("Found "+col.gameObject.name);
            col.gameObject.SetActive(false);
            collectedCount++;
            AudioManager.audioManager.Play("Coin");
            gemsCollectedText.SetText(collectedCount.ToString() + "/" + allCollectibles.Length.ToString());
            if(collectedCount == allCollectibles.Length && !allCollected){
                 allCollected = true;
                 Door.thisDoor.Enable();
                Debug.Log("Collected em all!");
            }
        }
        if(col.gameObject.tag == "Finish" && allCollected){
            AudioManager.audioManager.Play("Warp");
            StateManager.smInstance.SetState(StateManager.State.Win);
            gameObject.SetActive(false);
        }
        if(col.gameObject.tag == "Ouchies"){
            player.KO();
        }
    }


}
