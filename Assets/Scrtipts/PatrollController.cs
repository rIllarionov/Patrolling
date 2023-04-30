using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollController : MonoBehaviour
{
    [SerializeField] private float _speedOfPatrolling;
    [SerializeField] private float _freezingTime;
    [SerializeField] private List <Transform> _patrollLocations;

    private float _currentTime; //сюда накапливаем прошедшее время со старта
    private int _patrollSections; //сюда запишем сколько точек в маршруте
    private int _currentStart;//текущий старт
    private int _currentFinish;//текущий финиш
    private float _timeOfJurney;
    

    private void Start()
    {
        _patrollSections = _patrollLocations.Count;//снимаем кол-во точек
        _currentFinish = _currentStart + 1;//сразу сдвигаем финиш на следующий после старта пункт
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        var progress = _currentTime / _timeOfJurney;
        
        var newPossition = 
            Vector3.Lerp(_patrollLocations[_currentStart].position, 
                _patrollLocations[_currentFinish].position, progress);
        transform.position = newPossition;

        if (progress > 1)
        {
            _currentTime = 0-_freezingTime;
            SetCurrentSector();
            GetTimeCurrentDestination();
        }

    }

    private void SetCurrentSector()//устанавливаем текущий старт и финиш
    {

        if (_currentStart == _patrollSections - 1 || _currentFinish == 0) //если стартовая точка на последнем пункте,
                                                                          //а финиш на первом,
                                                                          //значит мы сделали круг и надо начать сначала
        {
            _currentStart = 0;
            _currentFinish = 1;
        }
        
        else if (_currentFinish == _patrollSections - 1) //если дошли до последней точки то перемещаем финиш на первый пункт
        {
            _currentStart++;
            _currentFinish = 0;
        }
        
        else if  (_currentFinish < _patrollSections - 1)//если еще не дошли до последней точки, то сдвигаем обе границы
        {
            _currentStart++;
            _currentFinish++;
        }
        
    }

    private void GetTimeCurrentDestination() //считаем за какое время мы должны пройти текущий отрезок
    {
        var distance = Vector3.Distance(_patrollLocations[_currentStart].position, 
            _patrollLocations[_currentFinish].position);
        _timeOfJurney = distance / _speedOfPatrolling;
    }
}
