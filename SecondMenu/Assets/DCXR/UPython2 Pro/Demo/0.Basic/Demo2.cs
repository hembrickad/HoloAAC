using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DCXR.Demo
{
    public class Demo2 : MonoBehaviour
    {
        [SerializeField] UPython2ChannelSO pyChannel;


        void Callback(string data)
        {
            Debug.Log($"[Rec:] {data}");
        }

        public void MLDemo()
        {
            pyChannel.Call("MLDemo.py", Callback);
        }
    }
}