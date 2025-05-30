using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private TextMeshProUGUI _coordinate;
    Image _image;
    public bool IsX(int x) => _x == x;
    public bool IsY(int y) => _y == y;
    
    private int _state = 0;
    private int _x;
    private int _y;

    public void ResetCell()
    {
        _state = 0;
        IsSelected = false;
        UpdateStateVisuals();
    }

    private void Start()
    {
        
        _image = GetComponent<Image>();
        _image.color = Color.green;
        _stateText.text = _state.ToString();
    }

    public int State => _state;
    public event Action<int,int> OnToggled;
    
    public void Init(int x, int y)
    {
        _x = x;
        _y = y;
        _coordinate.text = $"{_x},{_y}";
    }
    
    public bool IsSelected = false;

    public void Toggle(bool isClicked = false)
    {
        var duration = isClicked ? 0.1f : 0.1f;
        var punch = isClicked ? 1.1f : 0.9f;
        var elasticity = isClicked ? 2f : 1f;
        //transform.DOPunchScale(Vector3.one * punch, duration, elasticity:elasticity);
        if (_state == 2)
        {
            _state = 0;
        }
        else
        {
            _state +=1;
        }
        
        
        if (isClicked)
            OnToggled?.Invoke(_x, _y);
        
        
        if (!IsSelected && isClicked)
            IsSelected = isClicked;

        UpdateStateVisuals();

    }

    private void UpdateStateVisuals()
    {
        if (State == 0)
        {
            _image.color = Color.green;
        }
        if (State == 1)
        {
            _image.color = Color.yellow;
        }

        if (State == 2)
        {
            _image.color = Color.red;
        }

        if (IsSelected)
        {
            var color = _image.color;
            color.a = 0.25f;
            _image.color = color;
        }
        else
        {
            var color = _image.color;
            color.a = 1f;
            _image.color = color;
        }
        
        _stateText.text = State.ToString();
    }

    
    
    
    
    

 


    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle(true);
    }
}
