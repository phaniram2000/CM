using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class BossController : MonoBehaviour
{
    public static BossController instance;
    public GameObject dummy;
    public List<GameObject> agents;
    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        charactersCount = UnityEngine.Random.Range(10, 15);
        triangularGridSetup();
    }

  
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Input.GetKeyDown(KeyCode.A))
            IncrementingAgents();
        if (Input.GetKeyDown(KeyCode.S))
            DecrementingAgents();
       // charactersCount = CrowdController.instance.crowd.Count;
    }

    int columns; int rows; public int charactersCount; int extraAgents=99;float xOffset;
    int closestTriangularNumber = 0;public int value; public float spacing; int extraRows=1; float  firstRowOffset=0;
    void triangularGridSetup()
    {
     
        for (int i = 0; closestTriangularNumber <= charactersCount; i++)//getting the closest triangular number and storing the extra value
        {
            closestTriangularNumber = i * (i + 1) / 2;

            if (closestTriangularNumber <= charactersCount)
            {
                value = i;
                extraAgents = charactersCount - closestTriangularNumber;
            }
        }
        rows = value; columns = value;

        if (value % 2 == 0)
            xOffset = spacing / 2;


        if (extraAgents == 0)// here our triangular number is perfect with our desired charactersCount.
        {
            for (int i = 0; i < rows; i++)//horizontal rows
            {
                for (int j = 0; j < columns; j++)//vertical columns
                {
                    GameObject obj = Instantiate(dummy, new Vector3((transform.position.x + ((-value)/ 2+j)*spacing+i*spacing/2+ xOffset), transform.position.y,
                         transform.position.z +(i*spacing)), Quaternion.identity);
                    agents.Add(obj);
                    obj.transform.parent = transform;
                }
                columns--;//decrementing the columns row after row
            }
        }
        else// here our triangular number is not equal to our charactersCount, so will take row for the extra value of the charactersCount
        {
            if (extraAgents % 2 == 0)// checking if extra value is even 
            {
                firstRowOffset = 1; 
                for (int k = 0; k < extraAgents + firstRowOffset; k++)// so even, we need increment the extra value by adding 1.
                {
                    if ((int)(extraAgents + firstRowOffset) / 2 != k)//getting the midvalue of new extra value, and we ignore that value while insantating the objects
                    {
                        GameObject obj = Instantiate(dummy, new Vector3((transform.position.x + ((-extraAgents / 2) + k) * spacing),
                  transform.position.y, transform.position.z), Quaternion.identity);
                        agents.Add(obj);
                        obj.transform.parent = transform;
                    }
                }
            }
            else
            {
                for (int k = 0; k < extraAgents; k++)// extra value is odd , so no changes in this value, directly instantating
                {
                    GameObject obj = Instantiate(dummy, new Vector3((transform.position.x + ((-extraAgents / 2) + k) * spacing),
              transform.position.y, transform.position.z), Quaternion.identity);
                    agents.Add(obj);
                    obj.transform.parent = transform;
                }
            }
               

            for (int i = extraRows; i < rows + extraRows; i++)//instantating from 2nd row to get the triangular pattern
            {
                for (int j = 0; j < columns; j++)
                {
                    GameObject obj = Instantiate(dummy, new Vector3((transform.position.x + ((-value) / 2 + j) * spacing + (i-1) * spacing / 2 + xOffset),
                transform.position.y, transform.position.z + (i * spacing)), Quaternion.identity);
                    agents.Add(obj);
                    obj.transform.parent = transform;
                }
                columns--;
            }
        }
    }

    int childIndex; public float agentMoveDuration =3;
    void rePositioning()
    {
        for (int i = 0; closestTriangularNumber <= charactersCount; i++)//reforming the new triangular pattern
        {
            closestTriangularNumber = i * (i + 1) / 2;

            if (closestTriangularNumber <= charactersCount)
            {
                value = i;
                extraAgents = charactersCount - closestTriangularNumber;
            }
        }
        rows = value; columns = value;

        if (value % 2 == 0)// checking if value is even , if it is 0. we will modify the xValue by adding xOffset
            xOffset = spacing / 2;


        if (extraAgents == 0)
        {
            for (int i = 0; i < rows; i++)//horizontal rows
            {
                for (int j = 0; j < columns; j++)//vertical columns
                {
                    agents[childIndex].transform.DOMove(new Vector3(transform.position.x + ((-value) / 2 + j) * spacing + i * spacing/2 + xOffset,
                        transform.position.y, transform.position.z + (i * spacing)), agentMoveDuration);
                    childIndex++;
                }
                columns--;//decrementing the columns row after row
            }
        }

        else
        {
            if (extraAgents % 2 == 0)// checking if extra value is even 
            {
                firstRowOffset = 1;
                for (int k = 0; k < extraAgents + firstRowOffset; k++)// so even, we need increment the extra value by adding 1.
                {
                    if ((int)(extraAgents + firstRowOffset) / 2 != k)//getting the midvalue of new extra value, and we ignore that value while insantating the objects
                    {
                        agents[childIndex].transform.DOMove(new Vector3((transform.position.x + ((-extraAgents / 2) + k) * spacing),
                  transform.position.y, transform.position.z),agentMoveDuration);
                        childIndex++;
                    }
                }
            }
            else
            {
                for (int k = 0; k < extraAgents; k++)// extra value is odd , so no changes in this value, directly instantating
                {
                    agents[childIndex].transform.DOMove(new Vector3((transform.position.x + ((-extraAgents / 2) + k) * spacing),
              transform.position.y, transform.position.z),agentMoveDuration);
                    childIndex++;
                }
            }


            for (int i = extraRows; i < rows + extraRows; i++)//instantating from 2nd row to get the triangular pattern
            {
                for (int j = 0; j < columns; j++)
                {
                    agents[childIndex].transform.DOMove(new Vector3((transform.position.x + ((-value) / 2 + j) * spacing + (i - 1) * spacing/2 + xOffset),
                transform.position.y, transform.position.z + (i * spacing)),agentMoveDuration);
                    childIndex++;
                }
                columns--;
            }
        }
    }
    int incrementCharactersCount =4,decrementCharactersCount=10;int newAgents;
    void DecrementingAgents()
    {
        for (int z = 0; z < decrementCharactersCount; z++)//removing the collided number of objects
        {
            if (agents.Count >0 )
            {
                agents[0].SetActive(false);
                agents[0].transform.parent = null;
                agents.Remove(agents[0]);
            }
        }
        resetValues();
        rePositioning();
    }
    void IncrementingAgents()
    {
        for (int i = 1; i < value+firstRowOffset; i++)//adding the powerUp value number of objects
        {
            for (int j = 0; j < value; j++)
            {
                if (newAgents < incrementCharactersCount)
                {
                    GameObject obj = Instantiate(dummy, new Vector3((transform.position.x + ((-incrementCharactersCount) / 2 + (j)) * spacing + i * spacing / 2 + xOffset),
                transform.position.y, transform.position.z + ((i+ value) * spacing) ), Quaternion.identity);
                    agents.Add(obj);
                    agents[agents.Count].transform.DOMove(transform.position + new Vector3(-1.1f, -1f, 1000f), 4f);
                    obj.transform.parent = transform;
                    newAgents++;
                }
            }
        }
        resetValues();
        rePositioning();
        if (value == 0)
        {
            resetValues();
            charactersCount = incrementCharactersCount;
            triangularGridSetup();
        }
    }
    void resetValues()// resetting all the values       
    {
        charactersCount = agents.Count;//reassigning the charactercount
        closestTriangularNumber = 0;
        extraAgents = 99;
        childIndex = 0;
        firstRowOffset = 0;
        xOffset = 0;
        newAgents = 0;
    }
}