using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RhosynWatchies : MonoBehaviour
{
    public class Watch : MonoBehaviour
    {
        public enum ColourType
        {
            Icon,
            Text
        }

        public static Watch Instance { get; private set; }
        public UnityEngine.UI.Text baseText; // why must gorilla tag curse us with base unity text????
        public Image watchIcon; // that little icon on the watch
        public void Start()
        {
            Instance = this;
            baseText = GetComponentInChildren<UnityEngine.UI.Text>(); // simple :3
            watchIcon = FindBone(transform, "Material").GetComponent<Image>();
            // remove all other image other than watchIcon (because im cute :3 and i deserve a lil break)
            Image[] images = GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] != watchIcon)
                {
                    Destroy(images[i]);
                }
            }
            StartCoroutine(LoadBundle("https://quantumleapstudios.org/gt_bundles/watchfon", (bundle) =>
            {
                if (bundle != null)
                {
                    baseText.font = bundle.LoadAsset<Font>("Utopium");
                    baseText.fontSize = 8;
                }
            }));

        }

        private IEnumerator LoadBundle(string url, System.Action<AssetBundle> onComplete)
        {
            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                    onComplete?.Invoke(bundle);
                    Debug.Log("asset bundle loaded successfully :3");
                }
                else
                {
                    Debug.LogError($"asset bundle no loadeed :< : {www.error}");
                    onComplete?.Invoke(null);
                }
            }
        }

        public void SetText(string text)
        {
            baseText.text = text;
        }
        public void SetFontSize(int size)
        {
            baseText.fontSize = size;
        }
        public void SetIcon(Texture2D texture)
        {
            watchIcon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        public void SetColour(ColourType type, Color colour)
        {
            // this sucks :<
            switch (type)
            {
                case ColourType.Icon:
                    watchIcon.color = colour;
                    break;
                case ColourType.Text:
                    baseText.color = colour;
                    break;
            }
        }
    }
    public static Dictionary<Transform, List<Transform>> _cachedChildTransforms = new Dictionary<Transform, List<Transform>>();
    public static RhosynWatchies Instance { get; private set; }
    public static RhosynWatchies GetInstance() // cuz ik your too lazy to add it to a component :3
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("RhosynWatchies");
            Instance = obj.AddComponent<RhosynWatchies>();
        }
        return Instance;
    }
    public GameObject WatchInstance;
    public Watch BaseWatch;
    private GameObject WatchPrefab;
    public bool autoShowWatch = true; // if you want the watch to show by default :3

    public void Start()
    {
        Instance = this;
        WatchPrefab = GorillaTagger.Instance.offlineVRRig.huntComputer; // our prefab that we can simply copy
                                                                        // now, lets create our watch , with the same parent and stuff :3
        WatchInstance = Instantiate(WatchPrefab, GorillaTagger.Instance.offlineVRRig.huntComputer.transform.parent);
        // now, remove the components we dont need :3
        Destroy(WatchInstance.GetComponent<GorillaHuntComputer>());
        // and add the one we do...
        BaseWatch = WatchInstance.AddComponent<Watch>(); // good girll :3 (i self praise alot)
        if (autoShowWatch)
            ShowWatch(); // show the watch by default :3
        else
            HideWatch(); // hide the watch by default :3 (if its enabled, witch it probalby isnt)
    }

    // mental check
    private bool isAnythingNull()
    {
        return WatchInstance == null || BaseWatch == null || WatchPrefab == null;
    }

    /// <summary>
    /// Shows the watch. 
    /// </summary>
    public void ShowWatch()
    {
        if (isAnythingNull()) return;
        WatchInstance.SetActive(true);
    }

    /// <summary>
    /// Hides the watch.
    /// </summary>
    public void HideWatch()
    {
        if (isAnythingNull()) return;
        WatchInstance.SetActive(false);
    }

    /// <summary>
    /// This function sets the text of the watch. (I felt fancy and added a function for it :3)
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        if (isAnythingNull()) return;
        BaseWatch.SetText(text);
    }

    /// <summary>
    /// Lets you set the icon of the watch. (Pretty cool, i've never seen anyone use the image like this so ye :3
    /// </summary>
    /// <param name="texture"></param>
    public void SetIcon(Texture2D texture)
    {
        if (isAnythingNull()) return;
        BaseWatch.SetIcon(texture);
    }

    /// <summary>
    /// Lets you set the colour of the watch text or icon. (I felt fancy and made an enum for it :3 im such a good girl)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="colour"></param>
    public void SetColour(Watch.ColourType type, Color colour)
    {
        if (isAnythingNull()) return;
        BaseWatch.SetColour(type, colour);
    }

    /// <summary>
    /// Sets the font size of the watch text.
    /// </summary>
    /// <param name="size"></param>
    public void SetFontSize(int size)
    {
        if (isAnythingNull()) return;
        BaseWatch.SetFontSize(size);
    }

    // ima lazy butt :3
    public static Transform FindBone(Transform transform, string name, Transform targ = null)
    {
        if (!_cachedChildTransforms.TryGetValue(transform, out var boneSearchList))
        {
            boneSearchList = new List<Transform>();
            _cachedChildTransforms.Add(transform, boneSearchList);
        }

        boneSearchList.Clear();
        var root = targ == null ? transform : targ;
        boneSearchList.Add(root);

        while (boneSearchList.Count > 0)
        {
            var current = boneSearchList[boneSearchList.Count - 1];
            boneSearchList.RemoveAt(boneSearchList.Count - 1);

            if (current.name == name)
                return current;

            if (current.name.ToLower() == name.ToLower())
                return current;

            for (int i = 0; i < current.childCount; i++)
                boneSearchList.Add(current.GetChild(i));
        }

        foreach (var t in root.GetComponentsInChildren<Transform>(true))
        {
            if (t.name == name)
                return t;

            if (t.name.ToLower() == name.ToLower())
                return t;
        }

        return null;
    }
}
