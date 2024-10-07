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
        gameObject.SetActive(true);
        isMoving = true;
    }

    public void StopMoving()
    {
        OnShoot?.Invoke();
        isMoving = false;
        transform.localPosition = Vector2.zero;
    }

    private void OnEnable()
    {
        isMoving = false;
        transform.localPosition = Vector2.zero;
    }
}
