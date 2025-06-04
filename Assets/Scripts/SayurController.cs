using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class SayurController : MonoBehaviour
{
    public SayurEnum SayurEnum;
    private Vector2 _originalPosition;
    SayurContainer _sayurContainer;
    public Action OnSayurMatch;
    public Action OnSayurMismatch;
    public bool IsMatched;
    Transform _basketTransform;

    void Start()
    {
        _originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {

    }

    public void OnMouseDrag()
    {
        if (IsMatched)
        {
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    public void OnMouseUp()
    {
        if (IsMatched)
        {
            return;
        }
        if (_sayurContainer != null)
        {
            transform.position = _sayurContainer.transform.position;
            IsMatched = true;
            OnSayurMatch?.Invoke();
            Destroy(_sayurContainer.gameObject);
            ShrinkSayur();
            StartCoroutine(SayurMoveAnimation());
        }
        else
        {
            transform.position = _originalPosition;
            OnSayurMismatch?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SayurContainer"))
        {
            _sayurContainer = collision.gameObject.GetComponent<SayurContainer>();
            if (_sayurContainer.SayurEnum != SayurEnum)
            {
                _sayurContainer = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SayurContainer"))
        {
            _sayurContainer = null;
        }
    }

    private void ShrinkSayur()
    {
        Vector3 scale = transform.localScale;
        scale.x /= 2f;
        scale.y /= 2f;

        transform.localScale = scale;
    }

    public void SetBasketTransform(Transform basketTransform)
    {
        _basketTransform = basketTransform;
    }

    IEnumerator SayurMoveAnimation()
    {
        Vector3 targetPosition = _basketTransform.position;
        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
