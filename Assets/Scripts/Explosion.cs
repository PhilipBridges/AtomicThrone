using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        StartCoroutine(KillTime(gameObject, .4f));
    }

    IEnumerator KillTime(GameObject gameobject, float time)
    {
        yield return new WaitForSeconds(.6f);
        gameobject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(time);
        Destroy(gameobject);
    }
}
