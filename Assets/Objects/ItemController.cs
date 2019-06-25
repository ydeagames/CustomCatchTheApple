using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    public float dropSpeed = -.03f;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, this.dropSpeed, 0);
        if (transform.position.y < -1f)
            Destroy(gameObject);
	}

    void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Board").GetComponent<BoardController>().AddStone(gameObject);
    }
}
