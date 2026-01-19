using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PPOAgent : Agent {

    static Random rnd = new Random();    
    public static int boxLeft = -254;
    public static int boxRight = 255;

    public int score = 0;

    public static GameObject cherry; 
    public static GameObject strawberry; 
    public static GameObject grape; 
    public static GameObject dekopon; 
    public static GameObject orange; 
    public static GameObject apple; 
    public static GameObject pear; 
    public static GameObject peach; 
    public static GameObject pineapple; 
    public static GameObject melon; 
    public static GameObject watermelon; 
    public GameObject[] fruits = {cherry, strawberry, grape, dekopon, orange, apple, pear, peach, pineapple, melon, watermelon}; 
    
    public GameObject currentFruit = cherry;

    public static float cloudX = 0;

    
    private List<GameObject> allFruits = new List<GameObject>();


    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1;
        InvokeRepeating("RequestDecision", 3, 1f);
    }

    // when a new episode begins
    public override void OnEpisodeBegin() {

        // reset score
        Debug.Log("FINAL SCORE: "+score);
        score = 0;

        // create new fruit
        currentFruit = (GameObject) Instantiate(fruits[0], transform.position + new Vector3(0, -85, 0), Quaternion.identity);
        currentFruit.transform.parent = this.transform.parent;
        currentFruit.GetComponent<Rigidbody2D>().gravityScale = 0;
        currentFruit.GetComponent<CircleCollider2D>().enabled = false;
    }


    // collect environment observations
    public override void CollectObservations(VectorSensor sensor){
        Debug.Log("getting observations...");

        // observe current fruit type
        sensor.AddObservation(Int32.Parse(currentFruit.tag));

        // observe each fruit's coordinates + fruit type
        foreach (GameObject obj in allFruits) {
            sensor.AddObservation(obj.transform.localPosition);
            sensor.AddObservation(Int32.Parse(obj.tag));
        }
    }
    
    // when agent acts
    public override void OnActionReceived(ActionBuffers actions) {

        // get x position of cloud
        cloudX = actions.ContinuousActions[0] * 255;

        Debug.Log("raw X position: " + actions.ContinuousActions[0]);
        Debug.Log("generated X position: " + cloudX);

        // move the cloud
        move(cloudX);
    }


    public void move(float x) {

        // move arrow to generatedX
        transform.localPosition = new Vector2(x, 620);

        // constrain movement
        if (x < boxLeft) {
            transform.localPosition = new Vector2(boxLeft, 620);
        } 
        else if (x > boxRight){
            transform.localPosition = new Vector2(boxRight, 620);
        }


        // move fruit to arrow
        currentFruit.transform.localPosition = new Vector2(x, 535);

        // get size of asset
        Vector2 fruitSize = currentFruit.GetComponent<SpriteRenderer>().bounds.size;

        
        // drop fruit + generate new fruit
        if (x < boxLeft+(fruitSize[0]/2)){
            currentFruit.transform.localPosition = new Vector2(boxLeft+(fruitSize[0]/2), 535);
        } 
        else if (x > boxRight-(fruitSize[0]/2)){
            currentFruit.transform.localPosition = new Vector2(boxRight-(fruitSize[0]/2), 535);
        }

        currentFruit.GetComponent<CircleCollider2D>().enabled = true;
        currentFruit.GetComponent<Rigidbody2D>().gravityScale = 40;


        currentFruit = (GameObject) Instantiate(fruits[rnd.Next(5)], transform.position + new Vector3(0, -85, 0), Quaternion.identity);

        // put fruit in same environment
        currentFruit.transform.parent = transform.parent;

        currentFruit.GetComponent<Rigidbody2D>().gravityScale = 0;
        currentFruit.GetComponent<CircleCollider2D>().enabled = false;
            
    }


    public void addFruit(GameObject newFruit){
        allFruits.Add(newFruit);
    }


    public void removeFruit(GameObject fruitToRemove){
        allFruits.Remove(fruitToRemove);
    }


    public void clearFruits(){

        foreach (GameObject obj in allFruits) {
            GameObject.Destroy(obj);
        }
        allFruits.Clear();

        Debug.Log("fruits cleared!");
    }

}
