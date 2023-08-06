using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapReceiver : MonoBehaviour
{
    public GameObject[] spawn;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        GestureManager.Instance.OnTap += OntapHere;

    }

    private void OnDisable()
    {
        GestureManager.Instance.OnTap -= OntapHere;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OntapHere(object sender, TapEventArgs args)
    {
        if (args.HitObject == null)
        {
            Ray r = Camera.main.ScreenPointToRay(args.TapPosition);
            Vector3 pos = r.GetPoint(10);
            SpawnObject(pos);

        }
        else
        {
            Debug.Log($"Hit: {args.HitObject.name}");
        }

    }

    public void SpawnObject(Vector3 pos)
    {
        Instantiate(spawn[count % 2], pos, Quaternion.identity);
        count++;
    }
}
