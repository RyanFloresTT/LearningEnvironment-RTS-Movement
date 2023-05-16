using System;
using TMPro;
using UnityEngine;

public class FormationDropdownMenu : MonoBehaviour
{
    private Formations formation;
    private TMP_Dropdown menu;

    public event EventHandler<Formations> OnFormationDropdownMenuChanged;

    public static FormationDropdownMenu Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        formation = Formations.HorizontalLine;
        menu = GetComponent<TMP_Dropdown>();
        menu.value = 0;
    }

    public void DropdownChanged(int value)
    {
        menu.value = value;
        switch(value)
        {
            case 0:
                formation = Formations.HorizontalLine;
                break;
            case 1:
                formation = Formations.VerticalLine;
                break;
        }
        OnFormationDropdownMenuChanged?.Invoke(this, formation);
    }
}
