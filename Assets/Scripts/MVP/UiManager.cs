using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public sealed class UiManager
{
    public const string kViewName = "View";
    public const string kPath = "UiElements";

    private readonly List<Mediator> _additionalHuds;
    private UiObjectsHolder _uiObjectsContainer;

    public Canvas Canvas
    {
        get
        {
            return _uiObjectsContainer?.Canvas;
        }
    }

    private static UiManager _instance;

    public static UiManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UiManager();
            }
            return _instance;
        }
    }

    private UiManager()
    {
        _additionalHuds = new List<Mediator>();
    }

    public void Initialize(UiObjectsHolder holder)
    {
        _uiObjectsContainer = holder;
    }

    private Mediator ShowAdditional(UiElementType uiType, Type type, params object[] args)
    {
        var mediator = (Mediator)Activator.CreateInstance(type, args);
        _additionalHuds.Add(mediator);

        var hudType = mediator.ViewType;
        var hudView = CreateHud(uiType, hudType);

        if (hudView.transform.parent == _uiObjectsContainer.GetRootObjectByUiType(uiType))
        {
            hudView.transform.SetAsLastSibling();
        }

        mediator.Mediate(hudView);
        mediator.InternalShow();

        return mediator;
    }

    public T ShowAdditional<T>(UiElementType uiType, params object[] args) where T : Mediator
    {
        return (T)ShowAdditional(uiType, typeof(T), args);
    }

    public void HideAllAdditionals()
    {
        for (int i = _additionalHuds.Count - 1; i >= 0; i--)
        {
            var hud = _additionalHuds[i];
            hud.InternalHide();
            hud.Unmediate();
            _additionalHuds.RemoveAt(i);
        }
    }

    public void HideAdditional<T>() where T : Mediator
    {
        for (int i = _additionalHuds.Count - 1; i >= 0; i--)
        {
            var hud = _additionalHuds[i];

            if (!(hud is T))
                continue;

            hud.InternalHide();
            hud.Unmediate();
            _additionalHuds.RemoveAt(i);
        }
    }

    public bool IsOpened<T>()
    {
        return _additionalHuds.Exists(type => type is T);
    }

    public void ForceShow<T>(UiElementType uiType) where T : IHud
    {
        var hud = CreateHud(uiType, typeof(T));

        if (null == hud)
            return;

        hud.IsActive = true;
    }

    private IHud CreateHud(UiElementType uiType, Type viwType)
    {
        var hudView = _uiObjectsContainer.TryGetUiElement(viwType);

        if (null == hudView)
        {
            string fileName = viwType.Name.Replace(kViewName, string.Empty);
            var prefab = Resources.Load(Path.Combine(kPath, fileName));
            if (null == prefab)
            {
                Debug.LogError("Can't find hud " + Path.Combine(kPath, fileName));
                return null;
            }
            hudView = GameObject.Instantiate(prefab, _uiObjectsContainer.GetRootObjectByUiType(uiType)).GetComponent<IHud>();
            _uiObjectsContainer.AddUiElement(hudView);
        }

        hudView.transform.SetAsFirstSibling();

        return hudView;
    }
}
