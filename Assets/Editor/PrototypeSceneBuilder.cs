#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;

public static class PrototypeSceneBuilder
{
    const string Root = "Assets/BlueArmyGenerated";
    [MenuItem("Tools/Blue Army/Build Prototype Scene")]
    public static void Build() => BuildScene(true);

    // Entry point used by headless CI builds; never opens editor dialogs.
    public static void BuildForCI() => BuildScene(false);

    static void BuildScene(bool showDialog)
    {
        if (!AssetDatabase.IsValidFolder(Root)) AssetDatabase.CreateFolder("Assets", "BlueArmyGenerated");
        if (!AssetDatabase.IsValidFolder(Root + "/Prefabs")) AssetDatabase.CreateFolder(Root, "Prefabs");
        if (!AssetDatabase.IsValidFolder("Assets/Scenes")) AssetDatabase.CreateFolder("Assets", "Scenes");

        Material blue = Mat("SoldierBlue", new Color(.08f, .35f, .95f));
        Material darkBlue = Mat("SoldierDarkBlue", new Color(.03f, .12f, .4f));
        Material ocean = Mat("Ocean", new Color(.02f, .55f, .72f));
        Material trackMat = Mat("Track", new Color(.18f, .2f, .24f));
        Material gold = Mat("GoldGate", new Color(1f, .7f, .05f));
        Material green = Mat("GreenGate", new Color(.1f, .85f, .35f));
        Material red = Mat("BossRed", new Color(.8f, .08f, .12f));
        Material projectileMat = Mat("Projectile", new Color(1f, .9f, .2f));

        GameObject projectileObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectileObj.name = "ProjectilePrefab";
        projectileObj.transform.localScale = Vector3.one * .18f;
        projectileObj.GetComponent<Renderer>().sharedMaterial = projectileMat;
        SphereCollider pc = projectileObj.GetComponent<SphereCollider>(); pc.isTrigger = true;
        Rigidbody prb = projectileObj.AddComponent<Rigidbody>(); prb.isKinematic = true; prb.useGravity = false;
        Projectile proj = projectileObj.AddComponent<Projectile>();
        string projPath = Root + "/Prefabs/Projectile.prefab";
        PrefabUtility.SaveAsPrefabAsset(projectileObj, projPath);
        Object.DestroyImmediate(projectileObj);
        Projectile projectilePrefab = AssetDatabase.LoadAssetAtPath<Projectile>(projPath);

        GameObject soldier = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        soldier.name = "BlueSoldierPrefab";
        soldier.transform.localScale = new Vector3(.42f, .48f, .42f);
        soldier.GetComponent<Renderer>().sharedMaterial = blue;
        soldier.GetComponent<CapsuleCollider>().height = 1.7f;
        SoldierAutoShooter shooter = soldier.AddComponent<SoldierAutoShooter>();
        Transform muzzle = new GameObject("Muzzle").transform;
        muzzle.SetParent(soldier.transform, false); muzzle.localPosition = new Vector3(0, .2f, .45f);
        // Configure prefab fields through SerializedObject.
        SerializedObject so = new SerializedObject(shooter);
        so.FindProperty("projectilePrefab").objectReferenceValue = projectilePrefab;
        so.FindProperty("muzzle").objectReferenceValue = muzzle;
        so.ApplyModifiedPropertiesWithoutUndo();
        string soldierPath = Root + "/Prefabs/BlueSoldier.prefab";
        PrefabUtility.SaveAsPrefabAsset(soldier, soldierPath);
        Object.DestroyImmediate(soldier);
        GameObject soldierPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(soldierPath);

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        GameObject oceanObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        oceanObj.name = "Ocean"; oceanObj.transform.position = new Vector3(0, -1.5f, 45);
        oceanObj.transform.localScale = new Vector3(80, 1, 150); oceanObj.GetComponent<Renderer>().sharedMaterial = ocean;
        GameObject bridge = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bridge.name = "OceanBridge"; bridge.transform.position = new Vector3(0, -.2f, 45);
        bridge.transform.localScale = new Vector3(8, .4f, 110); bridge.GetComponent<Renderer>().sharedMaterial = trackMat;

        GameObject crowdRoot = new GameObject("CrowdRoot");
        CrowdController crowd = crowdRoot.AddComponent<CrowdController>();
        SerializedObject crowdSO = new SerializedObject(crowd);
        crowdSO.FindProperty("soldierPrefab").objectReferenceValue = soldierPrefab;
        crowdSO.FindProperty("startingSoldiers").intValue = 12;
        crowdSO.FindProperty("horizontalLimit").floatValue = 2.8f;
        crowdSO.ApplyModifiedPropertiesWithoutUndo();

        MakeGate("Gate_Add10", new Vector3(-1.8f, .45f, 13), green, MultiplierGate.Operation.Add, 10);
        MakeGate("Gate_x2", new Vector3(1.8f, .45f, 13), gold, MultiplierGate.Operation.Multiply, 2);
        MakeGate("Gate_Add15", new Vector3(-1.8f, .45f, 28), green, MultiplierGate.Operation.Add, 15);
        MakeGate("Gate_x3", new Vector3(1.8f, .45f, 28), gold, MultiplierGate.Operation.Multiply, 3);

        for (int i = 0; i < 5; i++)
        {
            GameObject tire = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tire.name = "Obstacle_Tire_" + (i + 1);
            tire.transform.position = new Vector3((i % 2 == 0 ? -1.5f : 1.5f), .35f, 42 + i * 5);
            tire.transform.localScale = new Vector3(.8f, .28f, .8f);
            tire.GetComponent<Renderer>().sharedMaterial = darkBlue;
            Damageable d = tire.AddComponent<Damageable>();
            SerializedObject dso = new SerializedObject(d); dso.FindProperty("maxHealth").intValue = 10; dso.ApplyModifiedPropertiesWithoutUndo();
        }

        // Place optional coin lines between gates and combat encounters.
        float[] coinRows = { 20f, 35f, 51f, 66f, 76f };
        float[] coinLanes = { -2.1f, 0f, 2.1f };
        int coinIndex = 0;
        foreach (float z in coinRows)
        {
            foreach (float x in coinLanes)
            {
                MakeCoin("CoinPickup_" + (++coinIndex), new Vector3(x, .55f, z), gold);
            }
        }

        GameObject boss = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boss.name = "GiantBoss"; boss.transform.position = new Vector3(0, 1.2f, 83);
        boss.transform.localScale = new Vector3(4, 3, 2); boss.GetComponent<Renderer>().sharedMaterial = red;
        Damageable bossDamage = boss.AddComponent<Damageable>();
        SerializedObject bso = new SerializedObject(bossDamage);
        bso.FindProperty("maxHealth").intValue = 200;
        bso.FindProperty("boss").boolValue = true;
        bso.FindProperty("coinReward").intValue = 100;
        bso.ApplyModifiedPropertiesWithoutUndo();

        GameObject manager = new GameObject("GameManager");
        GameManager gm = manager.AddComponent<GameManager>();
        if (!Object.FindObjectOfType<EventSystem>())
        {
            GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        Canvas canvas = new GameObject("HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)).GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        Text crowdText = MakeText(canvas.transform, "CrowdCount", "SOLDIERS  12", new Vector2(.22f,.94f), 32);
        Text coinsText = MakeText(canvas.transform, "Coins", "COINS  0", new Vector2(.8f,.94f), 30);
        Slider bossSlider = MakeSlider(canvas.transform);
        GameObject resultPanel = new GameObject("ResultPanel", typeof(RectTransform), typeof(Image));
        resultPanel.transform.SetParent(canvas.transform, false);
        RectTransform rp = resultPanel.GetComponent<RectTransform>(); rp.anchorMin = new Vector2(.15f,.35f); rp.anchorMax = new Vector2(.85f,.65f); rp.offsetMin = rp.offsetMax = Vector2.zero;
        resultPanel.GetComponent<Image>().color = new Color(0,0,0,.8f);
        Text resultText = MakeText(resultPanel.transform, "ResultText", "VICTORY", new Vector2(.5f,.65f), 40);
        Button restart = new GameObject("RestartButton", typeof(RectTransform), typeof(Image), typeof(Button)).GetComponent<Button>();
        restart.transform.SetParent(resultPanel.transform, false);
        RectTransform rb = restart.GetComponent<RectTransform>(); rb.anchorMin = new Vector2(.25f,.1f); rb.anchorMax = new Vector2(.75f,.35f); rb.offsetMin = rb.offsetMax = Vector2.zero;
        Text restartText = MakeText(restart.transform, "RestartLabel", "RESTART", new Vector2(.5f,.5f), 28);
        restart.onClick.AddListener(gm.Restart);

        Button pauseButton = new GameObject("PauseButton", typeof(RectTransform), typeof(Image), typeof(Button)).GetComponent<Button>();
        pauseButton.transform.SetParent(canvas.transform, false);
        RectTransform pb = pauseButton.GetComponent<RectTransform>();
        pb.anchorMin = new Vector2(.78f, .9f); pb.anchorMax = new Vector2(.98f, .96f);
        pb.offsetMin = pb.offsetMax = Vector2.zero;
        pauseButton.GetComponent<Image>().color = new Color(.04f, .12f, .22f, .9f);
        Text pauseLabel = MakeText(pauseButton.transform, "PauseLabel", "PAUSE", new Vector2(.5f, .5f), 24);
        pauseLabel.rectTransform.sizeDelta = new Vector2(200, 70);
        pauseButton.onClick.AddListener(gm.TogglePause);

        GameObject pausePanel = new GameObject("PausePanel", typeof(RectTransform), typeof(Image));
        pausePanel.transform.SetParent(canvas.transform, false);
        RectTransform pp = pausePanel.GetComponent<RectTransform>();
        pp.anchorMin = new Vector2(.15f, .32f); pp.anchorMax = new Vector2(.85f, .68f);
        pp.offsetMin = pp.offsetMax = Vector2.zero;
        pausePanel.GetComponent<Image>().color = new Color(0, 0, 0, .88f);
        Text pausedTitle = MakeText(pausePanel.transform, "PausedTitle", "PAUSED", new Vector2(.5f, .8f), 42);
        pausedTitle.rectTransform.sizeDelta = new Vector2(500, 80);

        Button resumeButton = new GameObject("ResumeButton", typeof(RectTransform), typeof(Image), typeof(Button)).GetComponent<Button>();
        resumeButton.transform.SetParent(pausePanel.transform, false);
        RectTransform resumeRect = resumeButton.GetComponent<RectTransform>();
        resumeRect.anchorMin = new Vector2(.2f, .42f); resumeRect.anchorMax = new Vector2(.8f, .64f);
        resumeRect.offsetMin = resumeRect.offsetMax = Vector2.zero;
        resumeButton.GetComponent<Image>().color = new Color(.05f, .55f, .25f, 1f);
        Text resumeLabel = MakeText(resumeButton.transform, "ResumeLabel", "RESUME", new Vector2(.5f, .5f), 28);
        resumeLabel.rectTransform.sizeDelta = new Vector2(480, 80);
        resumeButton.onClick.AddListener(gm.Resume);

        Button pauseRestartButton = new GameObject("PauseRestartButton", typeof(RectTransform), typeof(Image), typeof(Button)).GetComponent<Button>();
        pauseRestartButton.transform.SetParent(pausePanel.transform, false);
        RectTransform prbRect = pauseRestartButton.GetComponent<RectTransform>();
        prbRect.anchorMin = new Vector2(.2f, .12f); prbRect.anchorMax = new Vector2(.8f, .34f);
        prbRect.offsetMin = prbRect.offsetMax = Vector2.zero;
        pauseRestartButton.GetComponent<Image>().color = new Color(.65f, .12f, .12f, 1f);
        Text pauseRestartLabel = MakeText(pauseRestartButton.transform, "RestartLabel", "RESTART RUN", new Vector2(.5f, .5f), 26);
        pauseRestartLabel.rectTransform.sizeDelta = new Vector2(480, 80);
        pauseRestartButton.onClick.AddListener(gm.Restart);

        SerializedObject gmso = new SerializedObject(gm);
        gmso.FindProperty("crowdText").objectReferenceValue = crowdText;
        gmso.FindProperty("coinsText").objectReferenceValue = coinsText;
        gmso.FindProperty("bossHealthSlider").objectReferenceValue = bossSlider;
        gmso.FindProperty("resultPanel").objectReferenceValue = resultPanel;
        gmso.FindProperty("pausePanel").objectReferenceValue = pausePanel;
        gmso.FindProperty("resultText").objectReferenceValue = resultText;
        gmso.ApplyModifiedPropertiesWithoutUndo();
        resultPanel.SetActive(false);
        pausePanel.SetActive(false);

        Camera cam = Camera.main;
        if (cam)
        {
            cam.transform.position = new Vector3(0, 8, -11);
            cam.transform.rotation = Quaternion.Euler(24, 0, 0);
            cam.gameObject.AddComponent<PrototypeCameraFollow>().target = crowdRoot.transform;
        }
        Light light = Object.FindObjectOfType<Light>();
        if (light) { light.transform.rotation = Quaternion.Euler(45, -30, 0); light.intensity = 1.3f; }

        string scenePath = "Assets/Scenes/BlueArmyPrototype.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(scenePath, true) };
        AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
        if (showDialog) EditorUtility.DisplayDialog("Blue Army", "Prototype scene created at " + scenePath + ". Open it and press Play.", "OK");
    }

    static void MakeCoin(string name, Vector3 pos, Material mat)
    {
        GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        coin.name = name;
        coin.transform.position = pos;
        coin.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        coin.transform.localScale = new Vector3(.38f, .07f, .38f);
        coin.GetComponent<Renderer>().sharedMaterial = mat;
        Collider collider = coin.GetComponent<Collider>();
        collider.isTrigger = true;
        Rigidbody body = coin.AddComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;
        coin.AddComponent<CoinPickup>();
    }

    static void MakeGate(string name, Vector3 pos, Material mat, MultiplierGate.Operation op, int value)
    {
        GameObject gate = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gate.name = name; gate.transform.position = pos; gate.transform.localScale = new Vector3(2.2f, 1.8f, .25f);
        gate.GetComponent<Renderer>().sharedMaterial = mat;
        BoxCollider c = gate.GetComponent<BoxCollider>(); c.isTrigger = true;
        Rigidbody rb = gate.AddComponent<Rigidbody>(); rb.isKinematic = true; rb.useGravity = false;
        MultiplierGate mg = gate.AddComponent<MultiplierGate>();
        SerializedObject so = new SerializedObject(mg);
        so.FindProperty("operation").enumValueIndex = (int)op;
        so.FindProperty("value").intValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
        TextMesh label = new GameObject("GateLabel").AddComponent<TextMesh>();
        label.transform.SetParent(gate.transform, false); label.transform.localPosition = new Vector3(0,0,.6f);
        label.transform.localRotation = Quaternion.Euler(0,180,0);
        label.text = op == MultiplierGate.Operation.Add ? "+" + value : "x" + value;
        label.fontSize = 80; label.characterSize = .12f; label.anchor = TextAnchor.MiddleCenter; label.alignment = TextAlignment.Center;
        label.color = Color.white;
    }

    static Material Mat(string name, Color color)
    {
        string path = Root + "/" + name + ".mat";
        Material m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (!m) { m = new Material(Shader.Find("Standard")); AssetDatabase.CreateAsset(m,path); }
        m.color = color; EditorUtility.SetDirty(m); return m;
    }
    static Text MakeText(Transform parent, string name, string value, Vector2 anchor, int size)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>(); rt.anchorMin = rt.anchorMax = anchor; rt.sizeDelta = new Vector2(520,80); rt.anchoredPosition = Vector2.zero;
        Text t = go.GetComponent<Text>(); t.text = value; t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); t.fontSize = size; t.alignment = TextAnchor.MiddleCenter; t.color = Color.white;
        return t;
    }
    static Slider MakeSlider(Transform parent)
    {
        GameObject go = new GameObject("BossHealth", typeof(RectTransform), typeof(Slider));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>(); rt.anchorMin = new Vector2(.2f,.87f); rt.anchorMax = new Vector2(.8f,.9f); rt.offsetMin = rt.offsetMax = Vector2.zero;
        Slider s = go.GetComponent<Slider>(); s.minValue=0; s.maxValue=200; s.value=200;
        GameObject bg = new GameObject("Background", typeof(RectTransform), typeof(Image)); bg.transform.SetParent(go.transform,false);
        RectTransform br = bg.GetComponent<RectTransform>(); br.anchorMin=Vector2.zero; br.anchorMax=Vector2.one; br.offsetMin=br.offsetMax=Vector2.zero;
        bg.GetComponent<Image>().color = Color.gray;
        GameObject fill = new GameObject("Fill", typeof(RectTransform), typeof(Image)); fill.transform.SetParent(go.transform,false);
        RectTransform fr=fill.GetComponent<RectTransform>(); fr.anchorMin=Vector2.zero; fr.anchorMax=Vector2.one; fr.offsetMin=fr.offsetMax=Vector2.zero;
        fill.GetComponent<Image>().color=Color.red;
        s.fillRect=fr; s.targetGraphic=fill.GetComponent<Image>(); s.direction=Slider.Direction.LeftToRight;
        return s;
    }
}
#endif
