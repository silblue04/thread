using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEditor;


// InstanceUIData, IntrinsicUIData
using ShowUIData = Pair<InstanceUIData, IntrinsicUIData>;
// Sorting Layer, UIBase
using InstanceMapValue = System.Collections.Generic.Dictionary<int, UIBase>;


public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private Canvas _uiCanvas;
    [SerializeField] private RectTransform _uiCanvasRectTrans;

    public Vector2 CanvasSizeDelta { get { return _uiCanvasRectTrans.sizeDelta; }}

    [SerializeField] private CanvasScaler _canvasScaler;
    [SerializeField] private GraphicRaycaster _graphicRaycaster;

    [Header("TopMenu")]
    [SerializeField] private TopMenu _topMenu;
    [SerializeField] private RectTransform _uiRootBackOfTopMenu;
    [SerializeField] private RectTransform _uiRootBackOfTopMenuWithSafeArea;
    [SerializeField] private RectTransform _uiRootForwardOfTopMenu;
    [SerializeField] private RectTransform _uiRootForwardOfTopMenuWithSafeArea;

    [Header("Loading")]
    [SerializeField] private PopupSubLoading _popupSubLoading;
    [SerializeField] private PopupLoading _popupLoading;

    private bool _isDisableOnBackButton = false;


    private Queue<ShowUIData> _showUIQueue                      = new Queue<ShowUIData>();
    private Queue<Pair<IntrinsicUIData, bool>> _updateUIQueue   = new Queue<Pair<IntrinsicUIData, bool>>();
    // Name, InstanceMapValue
    private Dictionary<string, InstanceMapValue> _instanceDic   = new Dictionary<string, InstanceMapValue>();
    // sorting Layer index, Value : Max Order
    private Dictionary<int, int> _orderDic                      = new Dictionary<int, int>();

    private List<UIBase> _popupQuene = new List<UIBase>();
    private Queue<PopupSystemMessageOpenData> _systemMessageUIOpenQueue = new Queue<PopupSystemMessageOpenData>();



    public bool IsVisible { get { return _uiCamera.enabled; } }
    public void SetVisible(bool isVisible)
    {
        _uiCanvas.enabled = isVisible;
    }

    private void Start()
    {
        _InitCanvasScale();
    }
    private void _InitCanvasScale()
    {
        float fRateWidth = (float)Screen.width / DefsDefault.DEFAULT_SCREEN_WIDTH;
        float fRateHeight = (float)Screen.height / DefsDefault.DEFAULT_SCREEN_HEIGHT;

        if(fRateWidth < fRateHeight)
        {
            _canvasScaler.matchWidthOrHeight = 0f;
        }
        else
        {
            _canvasScaler.matchWidthOrHeight = 1f;
        }
    }


    public void Init()
    {
        _topMenu.Init();
        _popupSubLoading.Init();
        _popupLoading.Init();
    }
    
    public void Release()
    {

    }


    public void ShowPopupSubLoading(bool on)
    {
        _popupSubLoading.Show(on);
    }
    public void ShowPopupLoading(bool on)
    {
        _popupLoading.Show(on);
    }
    public void SetPopupLoadingType(PopupLoading.LoadingType type)
    {
        _popupLoading.SetLoadingType(type);
    }
    public void PlayLoadPakAnimation(LoadPakAnimationType type, Action callback = null)
    {
        _popupLoading.PlayLoadPakAnimation(type, callback);
    }
    
    
    public void Show<T>
    (
        string prefabPath
        , Param param = null
        , bool isForwardTopMenu = true
        , bool isNeededSafeArea = false
        , Action<UIBase> onOpenStart = null, Action<UIBase> onOpenEnd = null
        , Action<UIBase> onCloseStart = null, Action<UIBase> onCloseEnd = null
    )
    {
        InstanceUIData instanceUIData = new InstanceUIData(prefabPath, isForwardTopMenu, isNeededSafeArea);
        IntrinsicUIData intrinsicUIData = new IntrinsicUIData(typeof(T).Name, param)
        {
            callBackOpenStart = onOpenStart
            , callBackOpenEnd = onOpenEnd
            , callBackCloseStart = onCloseStart
            , callBackCloseEnd = onCloseEnd
        };

        _showUIQueue.Enqueue(new ShowUIData(instanceUIData, intrinsicUIData));
    }
    
    public UIBase ShowImmediately<T>
    (
        string prefabPath
        , Param param = null
        , bool isForwardTopMenu = true
        , bool isNeededSafeArea = false
        , Action<UIBase> onOpenStart = null, Action<UIBase> onOpenEnd = null
        , Action<UIBase> onCloseStart = null, Action<UIBase> onCloseEnd = null
    )
    {
        InstanceUIData instanceUIData = new InstanceUIData(prefabPath, isForwardTopMenu, isNeededSafeArea);
        IntrinsicUIData intrinsicUIData = new IntrinsicUIData(typeof(T).Name, param)
        {
            callBackOpenStart = onOpenStart
            , callBackOpenEnd = onOpenEnd
            , callBackCloseStart = onCloseStart
            , callBackCloseEnd = onCloseEnd
        };

        return _OpenUI(new ShowUIData(instanceUIData, intrinsicUIData));
    }

    private void Update()
    {
        if(_showUIQueue.Count > DefsDefault.VALUE_ZERO)
        {
            _OpenUI(_showUIQueue.Dequeue());
        }
        if (_updateUIQueue.Count > DefsDefault.VALUE_ZERO)
        {
            _UpdateUI(_updateUIQueue.Dequeue());
        }

        _UpdateBackButton();
    }

    private UIBase _OpenUI(ShowUIData showUIData)
    {
        IntrinsicUIData intrinsicUIData = showUIData.second;
        UIBase uiBase = _GetActivatedUI(intrinsicUIData);

        //같은 종류의 팝업이 생성 됐을때 정보만 변경
        if (uiBase != null && uiBase.IsOnlyOne)
        {
            _UpdateUI(uiBase, intrinsicUIData);
            uiBase.transform.SetAsLastSibling();
            return uiBase;
        }

        InstanceUIData instanceUIData = showUIData.first;
        string prefabPath = instanceUIData.prefabPath;

        uiBase = _CreateInstance(instanceUIData, intrinsicUIData);
        uiBase.OnOpenStart(intrinsicUIData.uiParam);
        uiBase.transform.SetAsLastSibling();
        //_UpdateForcedTopUI(uiBase);

        return uiBase;
    }

    public void UpdateUI<T>(Param param, bool isNeededToForceTop = false)
    {
        IntrinsicUIData intrinsicUIData = new IntrinsicUIData(typeof(T).Name, param);
        UIBase uiBase = _GetActivatedUI(intrinsicUIData);
        if (uiBase == null)
        {
            _updateUIQueue.Enqueue(new Pair<IntrinsicUIData, bool>(intrinsicUIData, isNeededToForceTop));
            return;
        }

        _UpdateUI(uiBase, intrinsicUIData, isNeededToForceTop);
    }

    public bool IsOpenPopup<T>() where T : UIBase
    {
        foreach(var popup in _popupQuene)
        {
            if(popup.GetType() == typeof(T))
                return true;
        }        
        return false;
    }

    private void _UpdateUI(Pair<IntrinsicUIData, bool> intrinsicUIDataAndIsNeededToForceTop)
    {
        IntrinsicUIData intrinsicUIData = intrinsicUIDataAndIsNeededToForceTop.first;
        bool isNeededToForceTop = intrinsicUIDataAndIsNeededToForceTop.second;

        UIBase uiBase = _GetActivatedUI(intrinsicUIData);
        if (uiBase == null)
        {
            _updateUIQueue.Enqueue(intrinsicUIDataAndIsNeededToForceTop);
            return;
        }

        _UpdateUI(uiBase, intrinsicUIData, isNeededToForceTop);
    }

    private void _UpdateUI(UIBase uiBase, IntrinsicUIData intrinsicUIData, bool isNeededToForceTop = false)
    {
        uiBase.gameObject.SetActive(true);
        uiBase.OnUpdateStart(intrinsicUIData);
        
        // if(isNeededToForceTop)
        // {
        //     _UpdateForcedTopUI(uiBase);
        // }
    }

    // private void _UpdateForcedTopUI(UIBase uiBase)
    // {
    //     if (uiBase.IsForcedTop == false)
    //     {
    //         //onlyOne일때는 새로 생성이 아니라 기존에 있던 걸 위치만 옮기는 것 이므로 -1
    //         if (uiBase.IsOnlyOne)
    //         {
    //             uiBase.transform.SetSiblingIndex(GetCurTopUIIndex(uiBase));
    //         }
    //         else
    //         {
    //             uiBase.transform.SetSiblingIndex(GetCurTopUIIndex(uiBase));
    //         }
    //     }
    //     else
    //     {
    //         uiBase.transform.SetAsLastSibling();
    //     }
    // }

    // private int GetCurTopUIIndex(UIBase uiBase)
    // {
    //     int index = DefsDefault.VALUE_NONE;
    //     foreach (var instanceMapValue in _instanceMap)
    //     {
    //         foreach (var instance in instanceMapValue.Value)
    //         {
    //             if (instance.Value.IsForcedTop)
    //             {
    //                 index = instance.Value.transform.GetSiblingIndex();
    //                 break;
    //             }
    //         }

    //         if (index != DefsDefault.VALUE_NONE)
    //         {
    //             break;  
    //         }
    //     }

    //     if (index != 0)
    //     {
    //         if (uiBase.IsOnlyOne)
    //         {
    //             return index - 1;
    //         }
    //         else
    //         {
    //             return index;
    //         }
    //     }
    //     else
    //     {
    //         return 0;
    //     }
    // }


    public void Close<T>()
    {
        IntrinsicUIData data = new IntrinsicUIData(typeof(T).Name);
        Close(data);
    }
    public void Close<T>(Action<UIBase> callbackCloseEnd)
    {
        IntrinsicUIData data = new IntrinsicUIData(typeof(T).Name, null, null, null, callbackCloseEnd);
        Close(data);
    }

    public void Close(IntrinsicUIData intrinsicUIData)
    {
        if (_instanceDic.ContainsKey(intrinsicUIData.name))
        {
            InstanceMapValue instanceMapValue = _instanceDic[intrinsicUIData.name];

            if (intrinsicUIData.id == DefsDefault.VALUE_NONE)
            {
                foreach (var instance in instanceMapValue)
                {
                    instance.Value.OnCloseStart();
                    _popupQuene.Remove(instance.Value);
                }
                instanceMapValue.Clear();

                if (instanceMapValue.Count == 0)
                {
                    _instanceDic.Remove(intrinsicUIData.name);
                }
            }
            else if (instanceMapValue.ContainsKey(intrinsicUIData.id))
            {
                UIBase uiBase = instanceMapValue[intrinsicUIData.id];

                _popupQuene.Remove(uiBase);
                uiBase.OnCloseStart();
                instanceMapValue.Remove(intrinsicUIData.id);

                if (instanceMapValue.Count == 0)
                {
                    _instanceDic.Remove(intrinsicUIData.name);
                }
            }
        }
    }

    public void CloseAll()
    {
        foreach (var _instanceMap in _instanceDic)
        {
            foreach (var instance in _instanceMap.Value)
            {
                instance.Value.OnCloseStart();
            }

            _instanceMap.Value.Clear();
        }

        _instanceDic.Clear();
        _popupQuene.Clear();
        ClearSystemMessageQueue();
    }

    public void ClearAll(List<string> ignoreUIList = null)
    {
        List<string> removeKeyList = new List<string>();
        foreach (var _instanceMap in _instanceDic)
        {
            // 자동으로 지우지 않을 UI예외처리
            if (ignoreUIList != null && ignoreUIList.Contains(_instanceMap.Key))
            {
                continue;
            }

            removeKeyList.Add(_instanceMap.Key);

            foreach (var instance in _instanceMap.Value)
            {
                Destroy(instance.Value.gameObject);
            }
            _instanceMap.Value.Clear();
        }

        for (int i = 0; i < removeKeyList.Count; i++)
        {
            RemovePopupQueue(removeKeyList[i]);
            _instanceDic.Remove(removeKeyList[i]);
        }

        ClearSystemMessageQueue();
    }

    private void RemovePopupQueue(string uiName)
    {
        foreach (var instance in _instanceDic[uiName])
        {
            instance.Value.OnCloseStart();
            _popupQuene.Remove(instance.Value);
        }
    }

    public void CloseDestory(UIBase uiBase)
    {    
        var imgChilds = uiBase.GetComponentsInChildren<Image>(true);
        for (int i = 0; i < imgChilds.Length; ++i)
        {
            imgChilds[i].sprite = null;
        }

        Destroy(uiBase.gameObject);
    }

    public UIBase GetActivatedUI<T>()
    {
        string name = typeof(T).Name;
        if (_instanceDic.ContainsKey(name) == false)
        {
            return null;
        }

        if( _instanceDic[name] == null
            || _instanceDic[name].Count == 0)
        {
            return null;
        }
            

        foreach (var uiBase in _instanceDic[name])
        {
            if (uiBase.Value.IsActivated)
            {
                return uiBase.Value;
            }
        }

        return null;
    }

    private UIBase _GetActivatedUI(IntrinsicUIData intrinsicUIData)
    {
        if (_instanceDic.ContainsKey(intrinsicUIData.name) == false)
        {
            return null;
        }


        InstanceMapValue instanceMapValue = _instanceDic[intrinsicUIData.name];

        if( instanceMapValue == null
            || instanceMapValue.Count == 0)
        {
            return null;
        }
            

        foreach (var uiBase in instanceMapValue)
        {
            if (uiBase.Value.IsActivated)
            {
                return uiBase.Value;
            }
        }

        return null;
    }

    private UIBase _CreateInstance(InstanceUIData instanceUIData, IntrinsicUIData intrinsicUIData)
    {
        UIBase loadedUiAsset = _GetUIPrefab(instanceUIData.prefabPath);
        if (loadedUiAsset == null)
        {
            throw new NullReferenceException(string.Format("[ERROR] Load UI : {0}", intrinsicUIData.name));
        }
        intrinsicUIData.id = _GetUniqueID(intrinsicUIData);


        RectTransform parent = _uiRootForwardOfTopMenuWithSafeArea;
        if(instanceUIData.isForwardTopMenu)
        {
            parent = (instanceUIData.isNeededSafeArea)
                ? _uiRootForwardOfTopMenuWithSafeArea 
                : _uiRootForwardOfTopMenu;
        }
        else
        {
            parent = (instanceUIData.isNeededSafeArea)
                ? _uiRootBackOfTopMenuWithSafeArea 
                : _uiRootBackOfTopMenu;
        }

        UIBase instancedUI = Util.Instantiate(parent, loadedUiAsset);
        instancedUI.Initialize(intrinsicUIData);

        if (_instanceDic.ContainsKey(intrinsicUIData.name) == false)
        {
            _instanceDic.Add(intrinsicUIData.name, new InstanceMapValue());
        }


        InstanceMapValue instanceMapValue = _instanceDic[intrinsicUIData.name];

        if (instanceMapValue.ContainsKey(intrinsicUIData.id) == false)
        {
            instanceMapValue.Add(intrinsicUIData.id, instancedUI);

            if (instancedUI.UIType == UIType.Popup)
            {
                _popupQuene.Add(instancedUI);
            }
        }
        else
        {
            Debug.LogErrorFormat("[ERROR] UIManager - Already Existing UI {0}(id : {1})", intrinsicUIData.name, intrinsicUIData.id);
        }

        return instancedUI;
    }

    private UIBase _GetUIPrefab(string prefabPath)
    {
        return Resources.Load<UIBase>(prefabPath);
    }

    private int _GetUniqueID(IntrinsicUIData intrinsicUIData)
    {
        if (_instanceDic.ContainsKey(intrinsicUIData.name) == false)
        {
            return DefsDefault.VALUE_NONE;
        }

        if(_instanceDic[intrinsicUIData.name] == null)
        {
            return DefsDefault.VALUE_NONE;
        }


        InstanceMapValue instanceMapValue = _instanceDic[intrinsicUIData.name];
        int nextUniqueId = 0;

        foreach (var uiBase in instanceMapValue)
        {
            if (uiBase.Value.UIData.id > nextUniqueId)
            {
                nextUniqueId = uiBase.Value.UIData.id;
            }
        }

        return nextUniqueId + 1;
    }

    private void _UpdateBackButton()
    {
        if (_isDisableOnBackButton == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _ClosePopupQueue();
        }
    }

    private void _ClosePopupQueue()
    {
        if (_IsPopupOpenForBackButton() == false)
        {
            Show<PopupExit>(DefsUI.Prefab.POPUP_EXIT);
        }
        else
        {
            UIBase lastOpenPopup = _GetLastOpenPopupForBackButton();
            if(lastOpenPopup != null)
            {
                lastOpenPopup.OnBackButton();
            }
        }
    }

    private bool _IsPopupOpen()
    {
        int cnt = DefsDefault.VALUE_ZERO;
        for (int i = 0; i < _popupQuene.Count; ++i)
        {
            if (_popupQuene[i] == null)
            {
                _popupQuene.RemoveAt(i);
                i--;
            }
            else
            {
                if(_popupQuene[i].gameObject.activeSelf)
                {
                    ++cnt;
                }
            }
        }
        return cnt > 0;
    }

    private bool _IsPopupOpenForBackButton()
    {
        int cnt = DefsDefault.VALUE_ZERO;
        for (int i = 0; i < _popupQuene.Count; i++)
        {
            if (_popupQuene[i] == null)
            {
                _popupQuene.RemoveAt(i);
                --i;
            }
            else
            {
                if(_popupQuene[i].IsSkippedBackButton == false
                    && _popupQuene[i].gameObject.activeSelf)
                {
                    ++cnt;
                }
            }
        }
        return cnt > 0;
    }

    private UIBase _GetLastOpenPopupForBackButton()
    {
        for (int i = _popupQuene.Count - 1; i >= 0; --i)
        {
            if (_popupQuene[i] == null)
            {
                continue;
            }
            if(_popupQuene[i].IsSkippedBackButton)
            {
                continue;
            }

            if (_popupQuene[i].gameObject.activeSelf)
            {
                return _popupQuene[i];
            }
        }
        
        return null;
    }

    public void AddSystemMessageUI(PopupSystemMessageOpenData data)
    {
        _systemMessageUIOpenQueue.Enqueue(data);

        if(false == IsInvoking("CheckNextSystemMessageUI"))
        {
            InvokeRepeating("CheckNextSystemMessageUI", 0f, 0.8f);
        }
    }
    public void CheckNextSystemMessageUI()
    {
        if (DefsDefault.VALUE_ZERO < _systemMessageUIOpenQueue.Count)
        {
            PopupSystemMessageOpenData openData = _systemMessageUIOpenQueue.Dequeue();
            Show<PopupSystemMessage>(DefsUI.Prefab.POPUP_SYSTEM_MESSAGE, openData);
        }
        else
        {
            EndCheckNextSystemMessageUI();
        }
    }
    public void EndCheckNextSystemMessageUI()
    {
        if (DefsDefault.VALUE_ZERO >= _systemMessageUIOpenQueue.Count)
        {
            CancelInvoke("CheckNextSystemMessageUI");
        }
    }
    public void ClearSystemMessageQueue()
    {
        CancelInvoke("CheckNextSystemMessageUI");
        _systemMessageUIOpenQueue.Clear();
    }

//     public void Broadcast(string funcName)
//     {
// #if UNITY_EDITOR
// 		if (Application.isPlaying == false)
//         {
//             return;
//         }
// #endif
// 		_StartCoBroadcastMessage(funcName);
//     }

    // private IEnumerator _coBroadcastMessage    = null;
    // private void _StartCoBroadcastMessage(string funcName)
    // {
    //     _StopCoBroadcastMessage();

    //     _coBroadcastMessage = _CoBroadcastMessage(funcName);
    //     StartCoroutine(_coBroadcastMessage);
    // }
    // private void _StopCoBroadcastMessage()
    // {
    //     if(_coBroadcastMessage == null)
    //     {
    //         return;
    //     }

    //     StopCoroutine(_coBroadcastMessage);
    //     _coBroadcastMessage = null;
    // }
    // private IEnumerator _CoBroadcastMessage(string funcName)
    // {
    //     // var components = GetComponentsInChildren<MonoBehaviour>();
    //     // yield return YieldInstructionCache.WaitForEndOfFrame;

    //     // foreach (var component in components)
    //     // {
    //     //     component.Invoke(funcName, 0);
    //     //     yield return YieldInstructionCache.WaitForEndOfFrame;
    //     // }

    //     // var components = GetComponentsInChildren<UILocalize>();
    //     // yield return YieldInstructionCache.WaitForEndOfFrame;

    //     // foreach (var component in components)
    //     // {
    //     //     if (component == null)
    //     //     {
    //     //         continue;
    //     //     }
    //     //     component.OnLocalize();
    //     //     yield return YieldInstructionCache.WaitForEndOfFrame;
    //     // }

    //     // PopupBottomTabUpdateData updateData = new PopupBottomTabUpdateData(PopupBottomTab.UIUpdateType.RenewBottomTab);
    //     // UIManager.Instance.UpdateUI<PopupBottomTab>(updateData);
    // }

    public override void OnDestroy()
    {
        _topMenu.Release();

        base.OnDestroy();
    }
}
