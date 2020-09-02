using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RGNikObject : MonoBehaviour
{
    protected  float movementDuration = 1f;
    protected void OnEnable()
    {
        transform.DOScale(new Vector3(1,1,1), movementDuration);
    }
    protected void OnDisable()
    {
        transform.DOScale(new Vector3(0, 0, 0), movementDuration);
    }
    public void TranslateScaleRotate(Vector3 position, Vector3 scale, Vector3 rotation)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(position, movementDuration));
        sequence.Join(transform.DOScale(scale, movementDuration));
        sequence.Join(transform.DORotate(rotation, movementDuration));
        sequence.Play();
    }
    public void ScaleAndTranslate(Vector3 position, Vector3 scale) 
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(position, movementDuration));
        sequence.Join(transform.DOScale(scale, movementDuration));
        sequence.Play();
    }
    public void Translate(Vector3 position)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(position, movementDuration));
        sequence.Play();
    }
}
