using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private static ViewManager _instance;
    public static bool isOpen;
    [SerializeField] private View _startingView;
    [SerializeField] private View[] _views;
    private View _currentView;
    private readonly Stack<View> _history = new Stack<View>();

    public static T GetView<T>() where T : View
    {
        for (int i = 0; i < _instance._views.Length; i++)
        {
            if (_instance._views[i] is T tView) { return tView; }
        }

        return null;
    }

    public static void SetIsOpen()
    {
        // Debug.Log($"Setting isOpen from {isOpen} to...");
        isOpen = _instance._currentView;
        // Debug.Log($"... {isOpen}");
    }

    public static void Show<T>(bool remember = true) where T : View
    {
        for (int i = 0; i < _instance._views.Length; i++)
        {
            if (_instance._views[i] is T)
            {
                if (_instance._views[i] != null)
                {
                    if (remember)
                    {
                        _instance._history.Push(_instance._currentView);
                    }
                    _instance._currentView.Hide();
                }
                _instance._currentView = _instance._views[i];
                _instance._views[i].Show();
            }
        }
    }

    public static void Show(View view, bool remember = true)
    {
        if (_instance._currentView != null)
        {
            if (remember)
            {
                _instance._history.Push(_instance._currentView);
            }
            _instance._currentView.Hide();
        }
        _instance._currentView = view;
        view.Show();
    }

    public static void ShowLast()
    {
        if (_instance._history.Count != 0)
        {
            Show(_instance._history.Pop(), false);
        }
    }

    public static void Hide(View view)
    {
        if (_instance._currentView != null)
        {
            _instance._currentView.Hide();
            _instance._currentView = null;
            SetIsOpen();
        }
    }
    
    private void Awake() => _instance = this;

    private void Start()
    {
        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].Initialize();
            _views[i].Hide();
        }
        
        if (_startingView != null)
        {
            Show(_startingView, true);
        }
    }
}
