using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInstantiationManager : MonoBehaviour
{
    [SerializeField] BoxCollider2D instantiationRange;
    [SerializeField] GameObject foodObject;
    [SerializeField] LayerMask gameLayer;

    public static bool instantiateFood, foundSpot, obstaclesNotPresent;

    void Update()
    {
        if(instantiateFood) {
            //Debug.Log("HELLO!!!!!");
            instantiateFood = false;
            foundSpot = false;
            RandomPosition();
        }
    }

    private void RandomPosition() {
        while(!foundSpot) {
            Debug.Log("HELLO!!!!!!!");
            Bounds bounds = this.instantiationRange.bounds;
            float randX = Mathf.RoundToInt(Random.Range(bounds.min.x,bounds.max.x));
            float randY = Mathf.RoundToInt(Random.Range(bounds.min.y,bounds.max.y));
            Vector3 newPosition = new Vector3(randX,randY,0f);
            Collider2D[] intersections = Physics2D.OverlapCircleAll(new Vector2(randX,randY),.1f,gameLayer);
            if(intersections.Length == 0) {
                foundSpot = true;
                /*
                Debug.Log(intersections.Length);
                foreach(Collider2D coll in intersections) {
                    Debug.Log(coll.gameObject.name);
                }
                */
                GameObject foodInstance = Instantiate(foodObject,newPosition,Quaternion.identity) as GameObject;
            } 
        }
    }
}
