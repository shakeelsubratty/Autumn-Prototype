using UnityEngine;
using System.Collections;

public class Col_Collect : MonoBehaviour {

    public string leafType; //No function yet
    private GameObject CharacterPlayer;



    void Start()
    {
        CharacterPlayer = GameObject.Find("chr_little"); //Find Player
    }
    void OnCollisionEnter()
    {
        AddToInventory(leafType);
        GameObject.Destroy(this.gameObject);
    }

    void AddToInventory(string LeafType)
    {
        chr_TempInv Inv = (chr_TempInv)CharacterPlayer.GetComponent(typeof(chr_TempInv)); // Get a connection between the tempInv Class
        Inv.AddLeaves(1);
    }
}
