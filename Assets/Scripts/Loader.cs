// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AddressableAssets;

// public class Loader : MonoBehaviour
// {
//     // [SerializeField]
//     // AssetReference ar;
//     // Start is called before the first frame update
//     void Start()
//     {
//         var handle = Addressables.LoadAssetAsync<Material>("Assets/UIAssets/material.mat");
//         handle.Completed += (handle) =>
//         {
//             gameObject.GetComponent<MeshRenderer>().sharedMaterial = handle.Result;
//         };
//         var handle2 = Addressables.LoadAssetAsync<GameObject>("Assets/Characters/Cube.prefab");
//         handle2.Completed += (handle2) =>
//         {
//             var r = handle2.Result;
//             Instantiate(r);
//         };
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }
