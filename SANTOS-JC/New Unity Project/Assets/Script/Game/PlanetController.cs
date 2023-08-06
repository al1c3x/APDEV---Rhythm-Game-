using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public GameObject[] Planets;

    Queue<GameObject> availablePlanet = new Queue<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        availablePlanet.Enqueue(Planets[0]);
        availablePlanet.Enqueue(Planets[1]);
        availablePlanet.Enqueue(Planets[2]);

        InvokeRepeating("MovePlanetDown", 0, 20.0f);
    }

    // Update is called once per frame
    void MovePlanetDown()
    {
        EnqueuePlanets();
        if(availablePlanet.Count == 0)
        {
            return;
        }
        GameObject aPlanet = availablePlanet.Dequeue();
        aPlanet.GetComponent<Planet>().IsMoving = true;
    }

    void EnqueuePlanets()
    {
        foreach(GameObject aplanet in Planets)
        {
            if((aplanet.transform.position.y  < 0 ) && (!aplanet.GetComponent<Planet>().IsMoving))
            {
                aplanet.GetComponent<Planet>().ResetPos();

                availablePlanet.Enqueue(aplanet);
            }
        }
    }
}
