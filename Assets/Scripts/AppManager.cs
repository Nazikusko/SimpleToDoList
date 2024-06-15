using Game.Core;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    [SerializeField] private UiObjectsHolder _uiObjectsHolder;

    private Timer _timer;
    public Timer Timer => _timer;

    protected override void DoAwake()
    {
        DontDestroyOnLoad(gameObject);
        UiManager.Instance.Initialize(_uiObjectsHolder);
        _timer = new Timer();

        var appModel = TaskManager.LoadTasks();
        if (appModel != null && appModel.TaskLists.Count > 0)
        {
            UiManager.Instance.ShowAdditional<ToDoWindowMediator>(UiElementType.Window, appModel.CurrentTaskListIndex);
        }
        else
        {
            UiManager.Instance.ShowAdditional<CreateListWindowMediator>(UiElementType.Window, 0);
        }
    }

    private void Update()
    {
        _timer.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void LateUpdate()
    {
        _timer.LateUpdate();
    }

    private void FixedUpdate()
    {
        _timer.FixedUpdate();
    }

    void OnApplicationQuit()
    {
        TaskManager.SaveModel();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) return;

        TaskManager.SaveModel();
    }
}
