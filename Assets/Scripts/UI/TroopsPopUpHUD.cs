using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TroopsPopUpHUD : MonoBehaviour
{
    public float lifespan = 1.0f;
    public float upSpeed = 1.0f;
    public float rotationSpeed = 90.0f;
    private TMP_Text text;
    private float spawnTime;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void Initialise(int change)
    {
        if (change > 0)
        {
            text.text = "+" + change;
            text.color = Color.cyan;
        }

        if (change < 0)
        {
            text.text = change.ToString();
            text.color = Color.red;
        }

        spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time - spawnTime > lifespan) Destroy(this.gameObject);
        else
        {
            transform.position += upSpeed * Time.deltaTime * Vector3.up;
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        }
    }
}
