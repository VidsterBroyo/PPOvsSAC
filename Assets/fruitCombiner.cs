using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class fruitCombiner : MonoBehaviour {

    public GameObject cloud;
    public GameObject nextFruit; 
    private bool hasCombined = false;


    // Start is called before the first frame update
    void Start() {
        Transform parentTransform = transform.parent;
        Transform cloudTransform = parentTransform.Find("Cloud");
        cloud = cloudTransform.gameObject;

        cloud.GetComponent<PPOAgent>().addFruit(this.gameObject);
    }

    
    // on fruit collision
    private void OnCollisionEnter2D(Collision2D collision) {

        // prevent collisions when still near cloud
        if (transform.localPosition[1] > 430){
            return;
        }

        // check if fruit is above container height limit
        if (transform.localPosition[1] > 135 && Math.Abs(gameObject.GetComponent<Rigidbody2D>().velocity[1]) < 2){

            // end agent episode
            cloud.GetComponent<PPOAgent>().EndEpisode();

            // clear fruits
            cloud.GetComponent<PPOAgent>().clearFruits();
        }


        // check if fruit collided with the same fruit type
        if (gameObject.tag == collision.gameObject.tag){

            // check that these 2 fruits have not already combined 
            if (hasCombined || collision.gameObject.GetComponent<fruitCombiner>().hasCombined){
                return;
            }

            // if current fruit has the smaller x position (or smaller y position), then current fruit does nothing (return)
            if (transform.position[0] < collision.gameObject.transform.position[0] || (transform.position[0] == collision.gameObject.transform.position[0] && transform.position[1] < collision.gameObject.transform.position[1])){
                return;
            } 

            // calculate the combined position
            Vector2 combinedPosition = (transform.position + collision.gameObject.transform.position) / 2;

            // delete current fruit and merging fruit
            Destroy(gameObject);
            Destroy(collision.gameObject);
            cloud.GetComponent<PPOAgent>().removeFruit(gameObject);
            cloud.GetComponent<PPOAgent>().removeFruit(collision.gameObject);


            // create a new fruit, only if the current fruit is not a watermelon
            if (gameObject.tag != "10"){
                GameObject currentFruit = (GameObject) Instantiate(nextFruit, combinedPosition, Quaternion.identity);
                currentFruit.transform.parent = this.transform.parent;
            }

            hasCombined = true;
            collision.gameObject.GetComponent<fruitCombiner>().hasCombined = true;

            // update score
            cloud.GetComponent<PPOAgent>().score += 2*Int32.Parse(gameObject.tag) + 2;
            cloud.GetComponent<PPOAgent>().AddReward(2*Int32.Parse(gameObject.tag) + 2);
        }
            
    }

}
