using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class cloudController : MonoBehaviour
{
    static Random rnd = new Random();
    
    public static int score = 0;

    public Vector2 mousePosition = new Vector2(0,0);
    public static int boxLeft = -254;
    public static int boxRight = 255;

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
    
    public static GameObject currentFruit = cherry;


    // Start is called before the first frame update
    void Start() {
        currentFruit = (GameObject) Instantiate(fruits[0], new Vector2(0, 535), Quaternion.identity);
        currentFruit.GetComponent<Rigidbody2D>().gravityScale = 0;
        currentFruit.GetComponent<CircleCollider2D>().enabled = false;
    }



    public void move(float x) {

        // move cloud to mouse
        transform.position = new Vector2(x, 620);

        // add constraints
        if (x < boxLeft) {
            transform.position = new Vector2(boxLeft, 620);
        } 
        else if (x > boxRight){
            transform.position = new Vector2(boxRight, 620);
        }

        // move fruit to arrow
        currentFruit.transform.position = new Vector2(transform.position[0], 535);

        // get size of asset
        Vector2 fruitSize = currentFruit.GetComponent<SpriteRenderer>().bounds.size;

        
        // when press button, drop fruit + generate new fruit
        if (Input.GetMouseButtonDown(0)){
            if (x < boxLeft+(fruitSize[0]/2)){
                currentFruit.transform.position = new Vector2(boxLeft+(fruitSize[0]/2), 535);
            } 
            else if (x > boxRight-(fruitSize[0]/2)){
                currentFruit.transform.position = new Vector2(boxRight-(fruitSize[0]/2), 535);
            }

            currentFruit.GetComponent<CircleCollider2D>().enabled = true;
            currentFruit.GetComponent<Rigidbody2D>().gravityScale = 40;

            currentFruit = (GameObject) Instantiate(fruits[rnd.Next(8)], new Vector2(transform.position[0], 535), Quaternion.identity);
            currentFruit.GetComponent<Rigidbody2D>().gravityScale = 0;
            currentFruit.GetComponent<CircleCollider2D>().enabled = false;
            
        }
    }
}
