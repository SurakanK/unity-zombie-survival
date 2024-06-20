// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class MemberMove : MonoBehaviour
// {
//     private GameObject _player;
//     private GameObject _memberNode;
//     private NavMeshAgent _navMesh;
//     private Animator _animator;
//     private CharacterShoot _shoot;

//     public bool isFollow = false;

//     // Start is called before the first frame update
//     void Start()
//     {
//         initScript();

//         _navMesh.enabled = true;
//     }

//     private void initScript()
//     {
//         _animator = GetComponentInChildren<Animator>();
//         _navMesh = GetComponent<NavMeshAgent>();
//         _shoot = GetComponent<CharacterShoot>();
//         _player = GameObject.FindGameObjectWithTag("user");
//         _memberNode = _animator.gameObject;

//     }

//     // Update is called once per frame
//     private void FixedUpdate()
//     {
//         move();
//     }

//     private void move()
//     {
//         if (!isFollow) return;
//         _navMesh.SetDestination(_player.transform.position);

//         if (Mathf.Abs(_navMesh.velocity.x) > 0.05f || Mathf.Abs(_navMesh.velocity.z) > 0.05f)
//         {
//             _animator.SetBool("IsRun", true);

//             if (!_shoot.target)
//             {
//                 _memberNode.transform.rotation = Quaternion.LookRotation(_navMesh.velocity);
//             }
//         }
//         else
//         {
//             _animator.SetBool("IsRun", false);
//         }
//     }
// }
