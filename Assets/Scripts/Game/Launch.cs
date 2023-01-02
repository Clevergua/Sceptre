using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Launch : MonoBehaviour
{
    private GameState gameState;
    private StringBuilder stringBuilder = new StringBuilder();
    [SerializeField]
    private Text text;
    [SerializeField]
    private Button loadAndLogButton, releaseButton, checkForCatalogUpdatesButton, updateCatalogsButton, logAllLocatorButton;
    [SerializeField]
    private InputField inputField;
    private Dictionary<string, List<AsyncOperationHandle<TextAsset>>> address2TextAssetHandle = new Dictionary<string, List<AsyncOperationHandle<TextAsset>>>();

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.logMessageReceived += LogOnContent;

        loadAndLogButton.onClick.AddListener(() =>
        {
            var address = inputField.text;
            Debug.Log($"Load {address}");
            var handle = Addressables.LoadAssetAsync<TextAsset>(inputField.text);
            handle.Completed += (handle) =>
            {
                if (handle.Result != null)
                {
                    Debug.Log(handle.Result.text);
                    if (!address2TextAssetHandle.ContainsKey(address))
                    {
                        address2TextAssetHandle.Add(address, new List<AsyncOperationHandle<TextAsset>>());
                    }
                    else
                    {
                        //do nothing..
                    }
                    address2TextAssetHandle[address].Add(handle);
                }
                else
                {
                    Debug.Log("Result is null!");
                }
            };
        });

        releaseButton.onClick.AddListener(() =>
        {
            var list = address2TextAssetHandle[inputField.text];
            foreach (var item in list)
            {
                Addressables.Release(item);
            }
            list.Clear();
        });

        checkForCatalogUpdatesButton.onClick.AddListener(() =>
        {
            var handle = Addressables.CheckForCatalogUpdates(true);
            handle.Completed += (handle) =>
            {
                if (handle.Result != null)
                {
                    Debug.Log(handle.Result.Count);
                    foreach (var item in handle.Result)
                    {
                        Debug.Log(item);
                    }
                }
            };
        });

        updateCatalogsButton.onClick.AddListener(() =>
        {
            var handle = Addressables.UpdateCatalogs();
        });

        logAllLocatorButton.onClick.AddListener(() =>
        {
            Debug.Log(Addressables.LibraryPath);
            Debug.Log(Addressables.BuildPath);
            Debug.Log(Addressables.RuntimePath);

            foreach (var item in Addressables.ResourceLocators)
            {
                Debug.Log(item.LocatorId);
            }
        });

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LogOnContent(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            stringBuilder.Append($"[{type.ToString()}]{condition}\n[StackTrace]{stackTrace}\n");
        }
        else
        {
            stringBuilder.Append($"[{type.ToString()}]{condition}\n");
        }
        text.text = stringBuilder.ToString();
    }
}
