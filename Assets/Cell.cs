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
    public int _x;
    public int _y;

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
        //_coordinate.text = $"{_x},{_y}";
    }
    
    public bool IsSelected ;

    public void Highlight(int count)
    {
        _coordinate.text = count.ToString();
    }

    public void Toggle(bool isClicked = false, bool isUser = false)
    {
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
        
        
        if (!IsSelected && isClicked && isUser)
            IsSelected = isClicked;

        UpdateStateVisuals();

    }

    private void UpdateStateVisuals()
    {
        _coordinate.text = string.Empty;
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
        Toggle(true, true);
    }
}
