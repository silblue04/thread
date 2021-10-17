using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.U2D;

using DG.Tweening;


public class Util
{
    public static int GetEnumMax<T>() where T : struct
    {
        return System.Enum.GetValues(typeof(T)).Length;
    }

    // 테이블 등에서 사용하는 값은 1000이 1초라고 함. 기본 밀리세컨으로 사용함. //
    public static float ToSec(float time)
    {
        return time * 0.001f;
    }

 #region Hash
        /// <summary>
        /// Hashes the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The hash.</returns>
        public static int Hash(params object[] items)
        {
            if (items.Length == 0) return 0;
            int hash = 23;
            for (int i = 0; i < items.Length; i++) hash = hash * 31 + items[i].GetHashCode();
            return hash;
        }

        /// <summary>
        /// Hashes the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The hash.</returns>
        public static int Hash(IEnumerable<object> items)
        {
            int hash = 23;
            bool any = false;

            foreach (var item in items)
            {
                any = true;
                hash = hash * 31 + item.GetHashCode();
            }

            return any ? hash : 0;
        }

        /// <summary>
        /// Hashes the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The hash.</returns>
        public static int Hash(System.Collections.IEnumerable items)
        {
            int hash = 23;
            bool any = false;

            foreach (var item in items)
            {
                any = true;
                hash = hash * 31 + item.GetHashCode();
            }

            return any ? hash : 0;
        }

        /// <summary>
        /// Hashes the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The hash.</returns>
        public static int Hash(object[,] items)
        {
            if (items.GetLength(0) == 0 || items.GetLength(1) == 0) return 0;

            int hash = 23;
            for (int i = 0; i < items.GetLength(0); i++)
            {
                for (int j = 0; j < items.GetLength(1); j++)
                {
                    hash = hash * 31 + items[i, j].GetHashCode();
                }
            }

            return hash;
        }

#endregion Hash

    /*
     * myung_ari
     * ColorUtility.TryParseHtmlString 를 권장
    */
    // public static Color HexToRGB(string hex)
    // {
    //     hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
    //     hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
    //     byte a = 255;//assume fully visible unless specified in hex
    //     byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
    //     byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
    //     byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
    //     //Only use alpha if the string has enough characters
    //     if (hex.Length == 8)
    //     {
    //         a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
    //     }

    //     return new Color32(r, g, b, a);
    //}



    public static T[] AddToArray<T>(T[] Org, int index, T New_Value)
    {
        List<T> list = new List<T>();
        list.AddRange(Org);

        if (index >= 0 && list.Count >= index)
            list.Insert(index, New_Value);

        return list.ToArray();
    }

    public static void InitLocalTransform(Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static void InitLocalTransformExceptScale(Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public static void InitLocalRectTransform(RectTransform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static void DestroyChild(Transform transform)
    {
        int count = transform.childCount;
        for (int i = count - 1; i >= 0; --i)
        {
            UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static void DestroyImmediateChild(Transform transform)
    {
        int count = transform.childCount;
        for (int i = count - 1; i >= 0; --i)
        {
            UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public static Vector3 RotationXY(Vector3 direction)
    {
        float baseAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float lengthXZ = Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z);
        float headAngle = (Mathf.Atan2(lengthXZ, direction.y) * Mathf.Rad2Deg) - 90f;

        return new Vector3(headAngle, baseAngle, 0f);
    }

    public static Vector3 GetInspectorRotation(Transform tr, Vector3 origin)
    {
        float x = origin.x;
        float y = origin.y;
        float z = origin.z;
        if (Vector3.Dot(tr.up, Vector3.up) >= 0.0f)
        {
            if (origin.x >= 0.0f && origin.x <= 90.0f)
                x = origin.x;
            if (origin.x >= 270.0f && origin.x <= 360.0f)
                x = origin.x - 360.0f;
        }
        if (Vector3.Dot(tr.up, Vector3.up) < 0.0f)
        {
            if (origin.x >= 0.0f && origin.x <= 90f)
                x = 180 - origin.x;
            if (origin.x >= 270.0f && origin.x <= 360.0f)
                x = 180 - origin.x;
        }

        if (origin.y > 180.0f)
            y = origin.y - 360.0f;

        if (origin.z > 180.0f)
            z = origin.z - 360.0f;

        return new Vector3(x, y, z);
    }

    public static Transform FindChildRecursive(Transform rootTr, string findName)
    {
        for (int i = 0; i < rootTr.childCount; i++)
        {
            Transform child = rootTr.GetChild(i);
            if (child.name == findName)
                return child;

            Transform foundChild = FindChildRecursive(child, findName);
            if (foundChild != null)
                return foundChild;
        }

        return null;
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    public static string GetDeviceModel()
    {
        return SystemInfo.deviceModel;
    }

    public static T LoadResources<T>(Transform parent, string prefabPath) where T : MonoBehaviour
    {
        T prefab = Resources.Load<T>(prefabPath);
        if (null != prefab)
        {
            T instance = MonoBehaviour.Instantiate<T>(prefab, parent);
            InitLocalTransform(instance.transform);
            return instance;
        }

        return null;
    }

    public static T LoadResources<T>(string prefabPath) where T : MonoBehaviour
    {
        T prefab = Resources.Load<T>(prefabPath);
        if (null != prefab)
        {
            T instance = MonoBehaviour.Instantiate<T>(prefab);
            InitLocalTransform(instance.transform);
            return instance;
        }

        return null;
    }

    public static T LoadResources<T>(string path, string prefabName) where T : MonoBehaviour
    {
        T prefab = Resources.Load<T>(string.Format("{0}/{1}", path, prefabName));
        if (null != prefab)
        {
            T instance = MonoBehaviour.Instantiate<T>(prefab);
            InitLocalTransform(instance.transform);
            return instance;
        }

        return null;
    }

    public static void UnloadResources<T>(ref T prefab) where T : MonoBehaviour
    {
        Resources.UnloadAsset(prefab);
        prefab = null;
    }

    public static GameObject Instantiate(GameObject gameObject)
    {
        GameObject instance = GameObject.Instantiate(gameObject);
        InitLocalTransform(instance.transform);
        return instance;
    }

    public static T Instantiate<T>(T gameObject) where T : UnityEngine.Component
    {
        T instance = GameObject.Instantiate<T>(gameObject);
        InitLocalTransform(instance.transform);
        return instance;
    }

    public static GameObject Instantiate(Transform parent, GameObject gameObject)
    {
        GameObject instance = GameObject.Instantiate(gameObject, parent);
        InitLocalTransform(instance.transform);
        return instance;
    }

    public static T Instantiate<T>(Transform parent, T gameObject) where T : UnityEngine.Component
    {
        T instance = GameObject.Instantiate<T>(gameObject, parent);
        InitLocalTransform(instance.transform);

        return instance;
    }

    static public T AddChild<T>(GameObject parent, T prefab) where T : MonoBehaviour
	{
		T instance = MonoBehaviour.Instantiate<T>(prefab, parent.transform);

		if (instance != null && parent != null)
		{
			Transform t = instance.transform;
            //t.SetParent(parent.transform, false);
			//t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			instance.gameObject.layer = parent.layer;
			instance.gameObject.name = prefab.name;
		}        
        return instance;
	}

	static public T AddChildWithTrans<T>(GameObject parent, T prefab)  where T : MonoBehaviour
	{
		T instance = MonoBehaviour.Instantiate<T>(prefab, parent.transform);
		
		if (instance != null && parent != null)
		{
			Transform pt = prefab.transform;
			Transform t = instance.transform;
            //t.SetParent(parent.transform, false);
			//t.parent = parent.transform;
			t.localPosition = pt.localPosition;
			t.localRotation = pt.localRotation;
			t.localScale = pt.localScale;
			instance.gameObject.layer = parent.layer;
			instance.gameObject.name = prefab.name;
		}
		return instance;
	}

    public static bool Approximately(float a, float b)
    {
        return (Mathf.Approximately(a, b));
    }

    public static bool Approximately(Vector2 a, Vector2 b)
    {
        return (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y));
    }

    public static bool Approximately(Vector3 a, Vector3 b)
    {
        return (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) &&  Mathf.Approximately(a.z, b.z));
    }

    public static Vector2 ConvertScreenPosToWorldPos(Vector3 screenPos)
    {
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    public static Vector2 ConvertWorldPosToUIPos(Vector3 worldPos)
    {
        Vector2 canvasSizeDelta = UIManager.Instance.CanvasSizeDelta;
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
        return new Vector2(
            (viewportPos.x * canvasSizeDelta.x) - (canvasSizeDelta.x * 0.5f)
            , (viewportPos.y * canvasSizeDelta.y) - (canvasSizeDelta.y * 0.5f));
    }

    // public static Vector2 ConvertScreenPosToUIPos(Vector3 screenPos)
    // {
    //     Vector2 canvasSizeDelta = UIManager.Instance.CanvasSizeDelta;
    //     Vector2 viewportPos = Camera.main.ScreenToWorldPoint(screenPos);
    //     viewportPos = Camera.main.WorldToViewportPoint(viewportPos);
    //     return new Vector2(
    //         (viewportPos.x * canvasSizeDelta.x) - (canvasSizeDelta.x * 0.5f)
    //         , (viewportPos.y * canvasSizeDelta.y) - (canvasSizeDelta.y * 0.5f));
    // }

    static public string ConvertNumberToShort(System.Numerics.BigInteger value)
    {
        bool isMinus = (value < System.Numerics.BigInteger.Zero);
        value = System.Numerics.BigInteger.Abs(value);


        string valueText = string.Empty;

        int convertCnt = 0;
        System.Numerics.BigInteger convertValue = value / 1000;

        if (convertValue < 1)
        {
            valueText =  string.Format("{0:n0}", value);
        }
        else
        {
            while (convertValue >= 1000)
            {
                convertValue /= 1000;
                convertCnt += 1;
            }

            string strValue = value.ToString();
            string strForeValue = strValue.Substring(0, 3);
            int length = strValue.Substring(3).Length;
            if (length > 3)
            {
                while (length > 3)
                {
                    length -= 3;
                }
            }
            if (length < 3)
            {
                strForeValue = strForeValue.Insert(length, ".");
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(strForeValue);
            char code = Convert.ToChar(65 + convertCnt % 26);
            sb.Append(code);
            convertCnt /= 26;
            while (convertCnt != 0)
            {
                int cnt = 65 + (convertCnt % 26) - 1;
                if (cnt > 64)
                {
                    code = Convert.ToChar(cnt);
                }
                else
                {
                    code = '0';
                }
                sb.Insert(4, code);
                convertCnt /= 26;
            }

            valueText = sb.ToString();
        }



        if(isMinus)
        {
            return string.Format(DefsString.TextFormat.MINUS_VALUE_TEXT_FORMAT, valueText);
        }
        else
        {
            return valueText;
        }
    }

    static public string ConvertNumberToShort(int number)
    {
        System.Numerics.BigInteger convertNum = number;
        return ConvertNumberToShort(convertNum);
    }

    static public string Format(string fmt, params object[] args)
    {
        return string.Format(fmt, args);
    }

    public static float GetDecibel(float volume)
    {
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
    }

    static public void PlayTweenerList(DG.Tweening.DOTweenAnimation[] tweenerList, bool reverse = false)
    {
        foreach(var tweener in tweenerList)
        {
            PlayTweener(tweener, reverse);
        }
    }
    static public void PlayTweener(DG.Tweening.DOTweenAnimation tweener, bool reverse = false)
    {
        tweener.enabled = true;
        
        if(reverse)
        {
            FlipEase(tweener);
            tweener.DOComplete();
            tweener.DOPlayBackwards();
        }
        else
        {
            tweener.DORestart();
        }
    }

    static public void StopTweenerList(DG.Tweening.DOTweenAnimation[] tweenerList)
    {
        foreach(var tweener in tweenerList)
        {
            StopTweener(tweener);
        }
    }
    static public void StopTweener(DG.Tweening.DOTweenAnimation tweener)
    {
        tweener.enabled = false;
        tweener.DOPause();
    }

    static public void SetTweenerLastFrame(DG.Tweening.DOTweenAnimation[] tweenerList)
    {
        foreach(var tweener in tweenerList)
        {
            SetTweenerLastFrame(tweener);
        }
    }
    static public void SetTweenerLastFrame(DG.Tweening.DOTweenAnimation tweener)
    {
        tweener.DOComplete();
    }

    static public void PlaySkeletonGraphicAnimation(Spine.Unity.SkeletonGraphic skeletonGraphic)
    {
        if(skeletonGraphic.AnimationState == null)
        {
            skeletonGraphic.Initialize(false);
        }

        if (skeletonGraphic.AnimationState != null)
        {
            skeletonGraphic.AnimationState.ClearTrack(0);
            skeletonGraphic.timeScale = 1.0f;
            skeletonGraphic.AnimationState.SetAnimation(0, skeletonGraphic.startingAnimation, skeletonGraphic.startingLoop);
        }
    }

    static public void SetSkeletonGraphicAnimationFixedFrame(Spine.Unity.SkeletonGraphic skeletonGraphic, float frameRatio)
    {
        if(skeletonGraphic == null || skeletonGraphic.AnimationState == null)
        {
            return;
        }

        Spine.TrackEntry te = skeletonGraphic.AnimationState.SetAnimation(0, skeletonGraphic.startingAnimation, false);
        te.TrackTime = te.AnimationEnd * frameRatio;
        te.TimeScale = 0.0f;
    }

    // static public void ConvertFromTo(DG.Tweening.DOTweenAnimation[] tweenerList)
    // {
    //     foreach(var tweener in tweenerList)
    //     {
    //         ConvertFromTo(tweener);
    //     }
    // }

    // static public void ConvertFromTo(DG.Tweening.DOTweenAnimation tweener)
    // {
    //     tweener.isFrom = !tweener.isFrom;

    //     if (tweener.isFrom)
    //     {
    //         ((DG.Tweening.Tweener)tweener.tween).From(tweener.tween.isRelative);
    //         //DG.Tweening.TweenSettingsExtensions.From((DG.Tweening.Tweener)tweener.tween, tweener.tween.isRelative);
    //     }
    //     else
    //     {
    //         tweener.tween.SetRelative(tweener.tween.isRelative);
    //         //DG.Tweening.TweenSettingsExtensions.SetRelative(tweener.tween, tweener.tween.isRelative);
    //     }
    // }

    static public void FlipEase(DG.Tweening.DOTweenAnimation[] tweenerList)
    {
        foreach(var tweener in tweenerList)
        {
            FlipEase(tweener);
        }
    }

    static public void FlipEase(DG.Tweening.DOTweenAnimation tweener)
    {
        Ease flipEase = tweener.easeType;
        switch(flipEase)
        {
            case Ease.InSine:
            case Ease.InQuad:
            case Ease.InCubic:
            case Ease.InQuart:
            case Ease.InQuint:
            case Ease.InExpo:
            case Ease.InElastic:
            case Ease.InBack:
            case Ease.InBounce:
            case Ease.InFlash:
            flipEase = (flipEase) + 1;
            break;

            case Ease.OutSine:
            case Ease.OutQuad:
            case Ease.OutCubic:
            case Ease.OutQuart:
            case Ease.OutQuint:
            case Ease.OutExpo:
            case Ease.OutElastic:
            case Ease.OutBack:
            case Ease.OutBounce:
            case Ease.OutFlash:
            flipEase = (flipEase) - 1;
            break;
        }

        tweener.tween.SetEase(flipEase);
    }

    static public System.Collections.IEnumerator CoPlaySliderAnimation
    (
        UnityEngine.UI.Slider slider
        , List<SliderAnimationData> dataList
        , float duration = 2.0f
        , System.Action<int, float> callbackOnChangeValue = null
        , System.Action<int> callbackOnEndPerAnimation = null
    )
    {
        if(dataList == null)
        {
            yield break;
        }
        
        
        float DURATION_PER_ANIMATION = duration / (float)dataList.Count;

        for(int i = 0; i < dataList.Count; ++i)
        {
            slider.minValue = dataList[i].min;
            slider.maxValue = dataList[i].max;

            slider.value = dataList[i].startValue;


            float valuegab = dataList[i].endValue - dataList[i].startValue; 
            float DURATION_PER_VALUE = DURATION_PER_ANIMATION / valuegab;

            for(float j = 0.0f; j < valuegab; j += 1.0f)
            {
                yield return YieldInstructionCache.WaitForSeconds(DURATION_PER_VALUE);
                slider.value += 1.0f;

                callbackOnChangeValue?.Invoke(i, slider.value);
            }

            slider.value = dataList[i].endValue;

            callbackOnEndPerAnimation?.Invoke(i);
        }
    }

// #region Ease Function
//     public static float spring(float start, float end, float value)
//     {
//         value = Mathf.Clamp01(value);
//         value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
//         return start + (end - start) * value;
//     }

//     public static float easeInQuad(float start, float end, float value)
//     {
//         end -= start;
//         return end * value * value + start;
//     }

//     public static float easeOutQuad(float start, float end, float value)
//     {
//         end -= start;
//         return -end * value * (value - 2) + start;
//     }

//     public static float easeInOutQuad(float start, float end, float value)
//     {
//         value /= .5f;
//         end -= start;
//         if (value < 1) return end / 2 * value * value + start;
//         value--;
//         return -end / 2 * (value * (value - 2) - 1) + start;
//     }

//     public static float easeInCubic(float start, float end, float value)
//     {
//         end -= start;
//         return end * value * value * value + start;
//     }

//     public static float easeOutCubic(float start, float end, float value)
//     {
//         value--;
//         end -= start;
//         return end * (value * value * value + 1) + start;
//     }

//     public static float easeInOutCubic(float start, float end, float value)
//     {
//         value /= .5f;
//         end -= start;
//         if (value < 1) return end / 2 * value * value * value + start;
//         value -= 2;
//         return end / 2 * (value * value * value + 2) + start;
//     }

    // public static float easeInQuart(float start, float end, float value)
    // {
    //     end -= start;
    //     return end * value * value * value * value + start;
    // }

    // public static float easeOutQuart(float start, float end, float value)
    // {
    //     value--;
    //     end -= start;
    //     return -end * (value * value * value * value - 1) + start;
    // }

//     public static float easeInOutQuart(float start, float end, float value)
//     {
//         value /= .5f;
//         end -= start;
//         if (value < 1) return end / 2 * value * value * value * value + start;
//         value -= 2;
//         return -end / 2 * (value * value * value * value - 2) + start;
//     }

//     public static float easeInQuint(float start, float end, float value)
//     {
//         end -= start;
//         return end * value * value * value * value * value + start;
//     }

//     public static float easeOutQuint(float start, float end, float value)
//     {
//         value--;
//         end -= start;
//         return end * (value * value * value * value * value + 1) + start;
//     }

//     public static float easeInOutQuint(float start, float end, float value)
//     {
//         value /= .5f;
//         end -= start;
//         if (value < 1) return end / 2 * value * value * value * value * value + start;
//         value -= 2;
//         return end / 2 * (value * value * value * value * value + 2) + start;
//     }

//     public static float easeInSine(float start, float end, float value)
//     {
//         end -= start;
//         return -end * Mathf.Cos(value / 1 * (Mathf.PI / 2)) + end + start;
//     }

//     public static float easeOutSine(float start, float end, float value)
//     {
//         end -= start;
//         return end * Mathf.Sin(value / 1 * (Mathf.PI / 2)) + start;
//     }

//     public static float easeInOutSine(float start, float end, float value)
//     {
//         end -= start;
//         return -end / 2 * (Mathf.Cos(Mathf.PI * value / 1) - 1) + start;
//     }

    // public static float easeInExpo(float start, float end, float value)
    // {
    //     end -= start;
    //     return end * Mathf.Pow(2, 10 * (value / 1 - 1)) + start;
    // }

//     public static float easeOutExpo(float start, float end, float value)
//     {
//         end -= start;
//         return end * (-Mathf.Pow(2, -10 * value / 1) + 1) + start;
//     }

//     public static float easeInOutExpo(float start, float end, float value)
//     {
//         value /= .5f;
//         end -= start;
//         if (value < 1) return end / 2 * Mathf.Pow(2, 10 * (value - 1)) + start;
//         value--;
//         return end / 2 * (-Mathf.Pow(2, -10 * value) + 2) + start;
//     }

//     public static float easeInCirc(float start, float end, float value)
//     {
//         end -= start;
//         return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
//     }

//     public static float easeOutCirc(float start, float end, float value)
//     {
//         value--;
//         end -= start;
//         return end * Mathf.Sqrt(1 - value * value) + start;
//     }

//     public static float easeInOutCirc(float start, float end, float value)
//     {
//         value /= .5f;
//         end -= start;
//         if (value < 1) return -end / 2 * (Mathf.Sqrt(1 - value * value) - 1) + start;
//         value -= 2;
//         return end / 2 * (Mathf.Sqrt(1 - value * value) + 1) + start;
//     }

//     public static float bounce(float start, float end, float value)
//     {
//         value /= 1f;
//         end -= start;
//         if (value < (1 / 2.75f))
//         {
//             return end * (7.5625f * value * value) + start;
//         }
//         else if (value < (2 / 2.75f))
//         {
//             value -= (1.5f / 2.75f);
//             return end * (7.5625f * (value) * value + .75f) + start;
//         }
//         else if (value < (2.5 / 2.75))
//         {
//             value -= (2.25f / 2.75f);
//             return end * (7.5625f * (value) * value + .9375f) + start;
//         }
//         else
//         {
//             value -= (2.625f / 2.75f);
//             return end * (7.5625f * (value) * value + .984375f) + start;
//         }
//     }

//     public static float easeInBack(float start, float end, float value)
//     {
//         end -= start;
//         value /= 1;
//         float s = 1.70158f;
//         return end * (value) * value * ((s + 1) * value - s) + start;
//     }

//     public static float easeOutBack(float start, float end, float value)
//     {
//         float s = 1.70158f;
//         end -= start;
//         value = (value / 1) - 1;
//         return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
//     }

//     public static float easeInOutBack(float start, float end, float value)
//     {
//         float s = 1.70158f;
//         end -= start;
//         value /= .5f;
//         if ((value) < 1)
//         {
//             s *= (1.525f);
//             return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
//         }
//         value -= 2;
//         s *= (1.525f);
//         return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;
//     }

//     public static float punch(float amplitude, float value)
//     {
//         float s = 9;
//         if (value == 0)
//         {
//             return 0;
//         }
//         if (value == 1)
//         {
//             return 0;
//         }
//         float period = 1 * 0.3f;
//         s = period / (2 * Mathf.PI) * Mathf.Asin(0);
//         return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
//     }

//     public static float elastic(float start, float end, float value)
//     {
//         //Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
//         end -= start;

//         float d = 1f;
//         float p = d * .3f;
//         float s = 0;
//         float a = 0;

//         if (value == 0) return start;

//         if ((value /= d) == 1) return start + end;

//         if (a == 0f || a < Mathf.Abs(end))
//         {
//             a = end;
//             s = p / 4;
//         }
//         else
//         {
//             s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
//         }

//         return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
//     }

//     public static Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
//     {
//         t = Mathf.Clamp01(t);
//         float oneMinusT = 1f - t;
//         return
//             oneMinusT * oneMinusT * p0 +
//             2f * oneMinusT * t * p1 +
//             t * t * p2;
//     }
//     #endregion

// #if UNITY_ANDROID
//     public static int getSDKVersion() 
//     {
// #if UNITY_EDITOR
//             return 27;//DefsValue.StandardApiVersion;
// #endif
//         using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
//         {
//             return version.GetStatic<int>("SDK_INT");
//         }
//     }
// #endif

// #if UNITY_EDITOR
//     #region JsonParser Copy From MF2_ToolUtil    
//     private const string _jsonMainDataAttributes = "ATTRIBUTES";
//     private const string _jsonMainDataDatas = "DATA";

//     [Serializable]
//     public class JsonMainData<T>
//     {
//         public List<string> attributes;
//         public List<T> datas;
//     }

//     private static string _LoadTextFile(string filePath)
//     {
//         if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
//         {
//             return null;
//         }

//         using (StreamReader streamReader = new StreamReader(filePath))
//         {
//             return streamReader.ReadToEnd();
//         }
//     }

    //private static JsonMainData<T> _JsonParseToJsonMainData<T>(string jsonText)
    //{
    //    JsonMainData<T> jsonMainData = new JsonMainData<T>();

    //    var jsonObject = JObject.Parse(jsonText);
    //    var jsonAttributes = jsonObject[_jsonMainDataAttributes];

    //    jsonMainData.attributes = new List<string>();
    //    foreach (var attribute in jsonAttributes)
    //    {
    //        jsonMainData.attributes.Add(attribute.ToString());
    //    }

    //    var jsonDatas = jsonObject[_jsonMainDataDatas];
    //    jsonMainData.datas = new List<T>();
    //    foreach (var data in jsonDatas)
    //    {
    //        jsonMainData.datas.Add(data.ToObject<T>());
    //    }

    //    return jsonMainData;
    //}

    //public static JsonMainData<T> LoadJsonDesignData<T>(string fileName)
    //{
    //    string jsonText = _LoadTextFile(Application.dataPath + "/../DesignData/jsons/" + fileName);
    //    return _JsonParseToJsonMainData<T>(jsonText);
    //}
    //#endregion

    // static public bool GetRandomResult(int percentage)
    // {
    //     return (UnityEngine.Random.Range(0, 100) < percentage);
    // }
    static public bool GetRandomResult(float percentage)
    {
        if(percentage <= DefsDefault.MIN_PERCENTAGE)
        {
            return false;
        }
        return (UnityEngine.Random.Range(DefsDefault.MIN_PERCENTAGE, DefsDefault.MAX_PERCENTAGE) <= percentage);
    }

    public class SimpleRatioSelector<T>
    {
        public class RatioItem
        {
            public T item;
            public int ratio;

            public RatioItem(T item, int ratio)
            {
                this.item = item;
                this.ratio = ratio;
            }
        }

        List<RatioItem> items = new List<RatioItem>();
        int total_ratio = 0;

        List<RatioItem> save_items = new List<RatioItem>();
        int save_total_ratio = 0;

        public void Clear()
        {
            items.Clear();
            save_items.Clear();
            total_ratio = 0;
            save_total_ratio = 0;
        }

        public void AddItem(T item, int ratio)
        {
            RatioItem rt = new RatioItem(item, ratio);
            AddItem(rt);
        }

        void AddItem(RatioItem rt)
        {
            items.Add(rt);
            total_ratio += rt.ratio;
        }

        void RemoveItem(RatioItem rt)
        {
            total_ratio -= rt.ratio;
            items.Remove(rt);
        }

        public void SaveItems()
        {
            save_total_ratio = total_ratio;
            save_items.Clear();
            save_items.AddRange(items);
        }

        public void RestoreItems(bool isClearSavedItems)
        {
            items.Clear();
            items.AddRange(save_items);
            if (isClearSavedItems)
            {
                save_items.Clear();
            }

            total_ratio = save_total_ratio;
        }

        public int RemainingCount()
        {
            return items.Count;
        }

        public delegate int CustomRandom(int start, int end);

        public List<T> SelectAll()
        {
            List<T> itemList = new List<T>();
            foreach(var item in items)
            {
                itemList.Add(item.item);
            }
            
            return itemList;
        }

        public T SelectRandom(CustomRandom custom_random, bool isRemove = false)
        {
            if(items.Count <= 0)
            {
                return default(T);
            }
            if (items.Count == 1)
            {
                return items[0].item;
            }


            RatioItem picked = null;
            if (total_ratio > 0)
            {
                int dice = (custom_random != null ? custom_random(0, total_ratio) : UnityEngine.Random.Range(0, total_ratio));
                int check_ratio = 0;
                for (int i = 0; i < items.Count; ++i)
                {
                    var rt = items[i];
                    if (rt.ratio == DefsDefault.VALUE_NONE)
                    {
                        continue;
                    }

                    check_ratio += rt.ratio;

                    if (dice < check_ratio)
                    {
                        picked = rt;
                        break;
                    }
                }

                if (picked == null)
                {
                    picked = items[items.Count - 1];
                }
            }
            else
            {
                picked = items[0];
            }

            if (isRemove)
            {
                RemoveItem(picked);
            }

            return picked.item;
        }
    }
//#endif
}