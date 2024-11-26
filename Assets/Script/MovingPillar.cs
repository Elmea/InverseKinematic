using UnityEngine;

public class MovingPillar : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float timeToTarget = 1;

    Vector3 startPos;
    float timer;
    bool timeDown;

    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        timer = 0;
    }

    void UpdateTimer()
    {
        if (timeDown)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                timeDown = false;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer / timeToTarget > 1)
                timeDown = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        transform.position = Vector3.Lerp(startPos, target.position, timer / timeToTarget);
    }
}
