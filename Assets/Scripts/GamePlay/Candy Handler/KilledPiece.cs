using H2910.GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledPiece : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public bool falling;

    float speed = 2.5f;
    Vector2 moveDir;

    internal void Initialize(Sprite sprite)
    {
        falling = true;

        moveDir.x = Random.Range(-1f, 1f);
        moveDir *= speed / 2;

        this.sprite.sprite = sprite;
    }
    private IEnumerator PieceDespawn()
    {
        yield return new WaitForSeconds(4f);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
        falling = false;
    }
    private void OnEnable()
    {
        //Ticker.OnTickAction += Tick;
        StartCoroutine(PieceDespawn());
    }
    private void OnDisable()
    {
        //Ticker.OnTickAction -= Tick;
    }
    private void Update()
    {
        //update alternative for optimization - will run every 0.1 seconds
        if (!falling) return;

        moveDir.x = Mathf.Lerp(moveDir.x, 0f, Time.deltaTime);
        transform.position += (Vector3)moveDir * Time.deltaTime * speed;
    }
}
