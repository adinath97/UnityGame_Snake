using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
       if(other.gameObject.tag == "Player") {
           ScoreManager.playerScore++;
           FoodInstantiationManager.instantiateFood = true;
           Destroy(this.gameObject);
       }
    }
}
