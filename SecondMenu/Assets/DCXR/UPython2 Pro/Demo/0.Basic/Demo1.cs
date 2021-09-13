using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Events;
using System.Threading;

namespace DCXR.Demo
{
    [HelpURL("")]
    public class Demo1 : MonoBehaviour
    {
        [SerializeField] UPython2ChannelSO pyChannel;


        void Callback(string data)
        {
            Debug.Log($"[Rec:] {data}");
        }

        void Callback(string channel, bool b)
        {
            Debug.Log($"[Channel] {channel}:{b}");
        }


        public void HelloWorld()
        {
            pyChannel.Call("hello.py", Callback);
        }

        public void Sum()
        {
            pyChannel.Call("sum.py 1 2 3 4", Callback);
        }

        public void SumArray()
        {
            pyChannel.Call("sumarray.py 3,4,5,6", Callback);
        }

        public void Plot()
        {
            pyChannel.Call("plot.py 1,2,3,4 3,1,2,4", null);
        }


        public void ChannelCall()
        {
            pyChannel.Call("Demo", "hello.py", Callback);
        }

        public void ChannelStatus()
        {
            pyChannel.ChannelStatus("Demo", Callback);
        }

        public void AChannelStatus()
        {
            Debug.Log($"[Async Channel Status]: {pyChannel.AChannelStatus("Demo")}");
        }

        public async void AsyncCall()
        {
            var t = pyChannel.ACall("hello.py");
            await Task.WhenAll(t);

            var result = t.Result;
            Debug.Log($"[Async Call]: {result}");
        }

        public async void AsyncChannelCall()
        {
            var t = pyChannel.AChannelCall("Demo","hello.py");
            await Task.WhenAll(t);

            var result = t.Result;
            Debug.Log($"[Async Call]: {result}");
        }

        public void CloseChannel()
        {
            pyChannel.Close("Demo");
        }

    }
}