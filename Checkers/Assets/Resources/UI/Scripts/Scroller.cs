using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x, _y;

    void Update()
{
    float scrollSpeedFactor = 0.1f; 
    _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * (scrollSpeedFactor * Time.deltaTime), _img.uvRect.size);
}
}