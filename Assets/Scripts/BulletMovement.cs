using UnityEngine;
using System; 

public class BulletMovement : MonoBehaviour
{
    public float verticalSpeed = 5f;
    public float deactivateAtY = 10f;

    private bool isMoving = false;

    public event Action OnShoot;
    

    void Update()
    {

        if (isMoving)
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);

            if (transform.localPosition.y >= deactivateAtY)
            {
                StopMoving();
            }
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        OnShoot?.Invoke();
        gameObject.SetActive(false);
        isMoving = false;
        transform.localPosition = Vector2.zero;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        isMoving = false;
        transform.localPosition = Vector2.zero;
    }
}
