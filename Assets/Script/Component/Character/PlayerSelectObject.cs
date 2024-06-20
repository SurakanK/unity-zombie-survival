// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerSelectObject : MonoBehaviour
// {
//     [SerializeField] ItemsDetail itemsDetailContainer;

//     private Camera _camera;
//     private CharacterShoot _shoot;
//     private CharacterAutoSelect _select;

//     // Start is called before the first frame update

//     void Start()
//     {
//         _camera = GameObject.FindGameObjectWithTag("world-camera").GetComponent<Camera>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         touch();
//     }

//     private void touch()
//     {

//         if (Input.touchCount != 1 || !_camera) return;
//         Touch touch = Input.touches[0];
//         Vector3 touchPos = touch.position;

//         if (touch.phase == TouchPhase.Began)
//         {
//             Ray ray = _camera.ScreenPointToRay(touchPos);
//             RaycastHit hit;

//             if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("bodyArmy")))
//             {
//                 GameObject Object = hit.collider.gameObject.transform.parent.gameObject;
//                 if (Object.tag == "army")
//                 {
//                     if (!itemsDetailContainer) return;
//                     var data = Object.GetComponentInChildren<CharacterData>();
                    
//                     itemsDetailContainer.setDetailArmy(data, () => {
//                         Object.GetComponent<MemberMove>().isFollow = true;
//                         Object.tag = "member";
//                     });
//                 }
//             }
//         }
//     }

// }
