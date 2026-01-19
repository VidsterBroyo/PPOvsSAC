using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruitManager : MonoBehaviour
{

    public static List<GameObject> fruits = new List<GameObject>();

    // add fruit to fruits list
    public void addFruit(GameObject newFruit){
        fruits.Add(newFruit);
    }

    // remove fruit from fruits list
    public void removeFruit(GameObject fruitToRemove){
        fruits.Remove(fruitToRemove);
    }

    // clear all fruits
    public void clearFruits(){

        foreach (GameObject obj in fruits) {
            GameObject.Destroy(obj);
        }
        fruits.Clear();

        Debug.Log("fruits cleared!");
    }
}
