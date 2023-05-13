using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllableUnit : MonoBehaviour
{
    [SerializeField] private GameObject border;

    private void Awake()
    {
        border.SetActive(false);
    }

    public void SelectUnit()
    {
        border.SetActive(true);
    }

    public void DeselectUnit()
    {
        border.SetActive(false);
    }
}
