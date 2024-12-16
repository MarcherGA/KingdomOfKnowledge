using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;

public class BirthdaySelector : MonoBehaviour
{
    public bool IsBirthdaySelected => _yearDropdown.value > -1 && _monthDropdown.value > -1 && _dayDropdown.value > -1;

    [Header("Dropdown References")]
    [SerializeField] private TMP_Dropdown _dayDropdown;
    [SerializeField] private TMP_Dropdown _monthDropdown;
    [SerializeField] private TMP_Dropdown _yearDropdown;

    [Header("Birthday Limits")]
    [SerializeField] private int _minYear = 1900;
    [SerializeField] private int _maxYear = DateTime.Now.Year;

    private int _selectedMonth = 1;
    private int _selectedYear = DateTime.Now.Year;

    void Start()
    {
        // Populate dropdowns once
        PopulateYearDropdown();
        PopulateMonthDropdown();
        PopulateDayDropdown();

        // Add listeners only once
        _monthDropdown.onValueChanged.AddListener(OnMonthChanged);
        _yearDropdown.onValueChanged.AddListener(OnYearChanged);
    }

    void PopulateYearDropdown()
    {
        // Use caching to reduce memory allocations
        List<string> yearOptions = new List<string>(_maxYear - _minYear + 1);
        for (int year = _maxYear; year >= _minYear; year--)
        {
            yearOptions.Add(year.ToString());
        }
        _yearDropdown.ClearOptions();
        _yearDropdown.AddOptions(yearOptions);

        // Default to current year
        _yearDropdown.value = -1;
        _selectedYear = _maxYear;
    }

    void PopulateMonthDropdown()
    {
        List<string> monthOptions = new List<string>(12);
        for (int month = 1; month <= 12; month++)
        {
            monthOptions.Add(month.ToString());
        }
        _monthDropdown.ClearOptions();
        _monthDropdown.AddOptions(monthOptions);

        // Default to January
        _monthDropdown.value = -1;
        _selectedMonth = 1;
    }

    void PopulateDayDropdown()
    {
        // Update days only for the selected month and year
        int daysInMonth = DateTime.DaysInMonth(_selectedYear, _selectedMonth);

        List<string> dayOptions = new List<string>(daysInMonth);
        for (int day = 1; day <= daysInMonth; day++)
        {
            dayOptions.Add(day.ToString());
        }
        _dayDropdown.ClearOptions();
        _dayDropdown.AddOptions(dayOptions);

        // Default to the first day
        _dayDropdown.value = -1;
    }

    void OnMonthChanged(int monthIndex)
    {
        // Update the selected month and refresh the days
        _selectedMonth = monthIndex + 1; // Dropdown index starts at 0
        PopulateDayDropdown();
    }

    void OnYearChanged(int yearIndex)
    {
        // Update the selected year and refresh the days
        _selectedYear = _maxYear - yearIndex;
        PopulateDayDropdown();
    }

    public string GetSelectedBirthday()
    {
        if (!IsBirthdaySelected) return null;

        // Database-ready date format: YYYY-MM-DD
        int day = _dayDropdown.value + 1; // Dropdown index starts at 0
        return $"{_selectedYear:D4}-{_selectedMonth:D2}-{day:D2}";
    }


}
