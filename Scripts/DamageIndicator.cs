using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageIndicator : MonoBehaviour
{ //Tutorial found: https://www.youtube.com/watch?v=I2j6mQpCrWE by Bimzy Dev "How to make DAMAGE POPUPS in 5 Minutes - Unity" uploaded Jan 22 2021
    public TMP_Text text;
    public float lifeTime;
    public float minDist;
    public float maxDist;

    public float timer;
    public Vector3 targetPos;
    public Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position); // will look at the camera
        float direction = Random.rotation.eulerAngles.z;
        initialPosition = transform.position;
        float dist = Random.Range(minDist, maxDist);
        targetPos = initialPosition + (Quaternion.Euler(0, 0, direction) * new Vector3(dist, dist));
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer > lifeTime)
        {
            Destroy(gameObject);
        }
        //COULD POTENTIALLY DO A FADE OUT
        transform.localPosition = Vector3.Lerp(initialPosition, targetPos, Mathf.Sin(timer / lifeTime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifeTime));
    }
    public void SetDamageText(float damage)//sets the damage text
    {
        text.text = damage.ToString();
    }
}
