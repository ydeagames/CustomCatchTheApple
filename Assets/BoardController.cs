using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    GameDirector director;
    AudioSource aud;
    public float speed = .5f;
    Dictionary<Vector3Int, GameObject> stones = new Dictionary<Vector3Int, GameObject>();
    List<Vector3Int> directions = new List<Vector3Int>();
    public AudioClip appleSE;
    public AudioClip bombSE;

    void Start()
    {
        director = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        aud = director.GetComponent<AudioSource>();
        for (int iz = -1; iz <= 1; iz++)
            for (int iy = -1; iy <= 1; iy++)
                for (int ix = -1; ix <= 1; ix++)
                {
                    var pos = new Vector3Int(ix, iy, iz);
                    if (pos != Vector3Int.zero)
                        directions.Add(pos);
                }
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(ray, out float enter))
        {
            var hit = ray.GetPoint(enter);
            float x = Mathf.RoundToInt(hit.x);
            float z = Mathf.RoundToInt(hit.z);

            var center = new Vector3(x, 0, z);

            {
                //var pos = Vector3.Lerp(transform.position, center, speed);
                var pos = center;
                pos.y = transform.position.y;
                transform.position = pos;
            }
        }
    }

    (bool, List<Vector3Int>) Check(Vector3Int pos, string tg)
    {
        foreach (var dir in directions)
        {
            var list = new List<Vector3Int>();
            for (int f = -5; f <= 5; f++)
            {
                var key = pos + dir * f;
                var st = stones.ContainsKey(key) ? stones[key] : null;
                if (st != null && st.tag == tg)
                {
                    list.Add(key);
                }
                else
                {
                    if (list.Count >= 5)
                        return (true, list);
                    else
                        list.Clear();
                }
            }
            if (list.Count >= 5)
                return (true, list);
        }
        return (false, null);
    }

    public void AddStone(GameObject stone)
    {
        var tg = stone.tag;
        stone.GetComponent<ItemController>().enabled = false;
        stone.transform.parent = transform;
        var subpos = stone.transform.position - transform.position;
        var pos = new Vector3Int(
            Mathf.RoundToInt(subpos.x),
            Mathf.CeilToInt(subpos.y),
            Mathf.RoundToInt(subpos.z));
        Debug.Log(pos);
        stone.transform.position = pos + transform.position;
        if (stones.ContainsKey(pos))
            return;
        stones.Add(pos, stone);
        var (check, list) = Check(pos, tg);
        if (check)
        {
            Debug.Log("OK");
            foreach (var key in list)
            {
                for (int i = 0; i < 10; i++)
                {
                    var nkey = key + Vector3Int.up * i;
                    var st = stones.ContainsKey(nkey) ? stones[nkey] : null;
                    if (i == 0)
                    {
                        Destroy(st);
                        stones.Remove(nkey);
                        director.GetPoint();
                        aud.PlayOneShot(appleSE);
                    }
                    else
                        if (st != null)
                        st.GetComponent<ItemController>().enabled = true;
                }
            }
        }
    }
}
