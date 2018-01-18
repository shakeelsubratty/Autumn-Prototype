//Project Coalesce Veritablegames
//If you have Questions ask Leonoel :)

using UnityEngine;
using System.Collections;

public class chr_Vitality : MonoBehaviour {
    public bool isAlive = false;
    public float playerHealth = 100;
    public float playerProtection = 1.2f; //You can use this for amor multiplier
    public bool disableComponents = true;

	// Use this for initialization
	void Start () {
	    isAlive = true;
        //Player = this.gameObject;

	}
	
	// Update is called once per frame
	void Update () {
        if (playerHealth <= 0){
            playerHealth = 0;
            this.animation.Stop();
            if (isAlive == true)
            {
                CreateCorp();
                DisableControllerComponents();
                isAlive = false;
            }
            //Destroy(Player);
            this.animation.Play("playerDie");
        }
        if(Input.GetButton("Fire1"))
        {
            //test function
            DealDamage(2);
        }

	}
    //this function allows to deal damage
    public void DealDamage(float damage){
        float appliedDamage = damage / playerProtection;
        playerHealth = playerHealth - appliedDamage;
    }
    public void CreateCorp(){
	
			// PlyPos.x = Player.transform.position.x;
			
        this.transform.Rotate(90,0,0);
    }
    public void DisableControllerComponents(){
        if (disableComponents == true)
        {
            MonoBehaviour[] comps = GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour c in comps)
            {
                c.enabled = false;
            }
        }
    }
}
