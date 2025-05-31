using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SecureBox : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Cell _prefab;
    [SerializeField] Button _button;
    [SerializeField] Button _shuffleButton;
    [SerializeField] Button _solution;
    
    private List<Cell> cells = new ();

    private void Start()
    {
        _button.onClick.AddListener(ResetCells);
        _shuffleButton.onClick.AddListener(Shuffle);
        _solution.onClick.AddListener(()=> FindingByMaxAlgorithm(0));
        var x = Random.Range(5, 8);
        var y = Random.Range(5, 8);
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = x;

        var count = x * y;

        for (int i = 0; i < count; i++)
        {
            var instance = Instantiate(_prefab, gridLayoutGroup.transform);
            var indexX = i % x;
            var indexY = (i - indexX)/x;
            instance.Init(indexX, indexY);
            cells.Add(instance);
            instance.OnToggled+= InstanceOnOnToggled;
        }
        
        
    }

    private int getNextState(int state)
    {
        if (state == 2)
        {
            return 0;
        }

        return state + 1;
    }

    async Task MaxAlgo(int state)
    {
        var duration = 10;
        var count = cells.Count(cell => cell.State == state);

        if (count == cells.Count)
        {
            Debug.Log("Mission completed");
            return;
        }
    
        var dictionary = new Dictionary<Cell, int>();
        var dictionaryState = new Dictionary<Cell, int>();
        foreach (var cell in cells)
        {
            cell.Toggle(true);
            cell.Toggle(true);
            dictionary.Add(cell, -100);
            dictionaryState.Add(cell, -100);
        
            var count1 = cells.Count(cell => cell.State == state);
            if (count < count1)
            {
                dictionary[cell] = count1;
                dictionaryState[cell] = cell.State;
            }
        
            await Task.Delay(duration);
            cell.Toggle(true);
            cell.Toggle(true);
            var count2 = cells.Count(cell => cell.State == state);
            if (count < count2)
            {
                if (count2>count1)
                {
                    dictionary[cell] = count2;
                    dictionaryState[cell] = cell.State;
                }
            }
            foreach (var p in dictionary)
            {
                Debug.Log($"{p.Key._x}/{p.Key._y} - {p.Value}");
            }
            if (dictionary[cell] == -100)
            {
                dictionary.Remove(cell);
                dictionaryState.Remove(cell);
            }
        
            await Task.Delay(duration);
        
            cell.Toggle(true);
            cell.Toggle(true);
        
        }
        if (dictionary.Count == 0)
        {
            Debug.Log($"Dead lock State{state}");
            //  FindingByMaxAlgorithm(getNextState(state));
            _solution.interactable = true;
            return;
        }
        await Task.Delay(duration);
    
        //var pair = dictionary.First();
        foreach (var p in dictionary)
        {
            /*await Task.Delay(duration);
            if (pair.Value < p.Value)
            {
                pair = p;
            }*/
            p.Key.Highlight(p.Value);
        }
        /*while (pair.Key.State != dictionaryState[pair.Key])
        {
            await Task.Delay(duration);
            pair.Key.Toggle(true);
        }*/
    
        Debug.Log($"Open count: {cells.Count(cell => cell.State == state)}/{cells.Count} State:{state}");
    }
    
    async void FindingByMaxAlgorithm(int state)
    {
        _solution.interactable = false;
        await MaxAlgo(state);
        _solution.interactable = true;
    }

    private void ResetCells()
    {
        foreach (var cell in cells)
        {
            cell.ResetCell();
        }
    }
    
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            StartUnClick();
        }
    }

    async void Shuffle()
    {
        var cellls = cells.ToList();
        cellls.Sort((x, y) => Random.Range(-10000,10000).CompareTo(Random.Range(-10000,10000)));
        foreach (var cell in cellls.Take(5))
        {
            await Task.Delay(50);
            cell.Toggle(true, true);
        }
    }
    
    async void StartUnClick()
    {
        var cellls = cells.ToList();
        cellls.Sort((x, y) => Random.Range(-10000,10000).CompareTo(Random.Range(-10000,10000)));
        foreach (var cell in cellls.Where((cell => cell.IsSelected)))
        {
            await Task.Delay(50);
            cell.Toggle(true);
        }
    }

    private void InstanceOnOnToggled(int x, int y)
    {
        foreach (var cell in cells)
        {
            if (cell.IsX(x) && cell.IsY(y))
                continue;
            if (cell.IsX(x) || cell.IsY(y))
            {
                cell.Toggle();
            }
        }
    }
}
