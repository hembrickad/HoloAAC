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


/// <summary>
///
/// MIT license
/// Created by Haikun Huang
/// Date: 2021
/// </summary>
///

namespace DCXR
{
    [AddComponentMenu("DCXR/UPython2")]
    public class UPython2 : MonoBehaviour
    {
        
        class SocketStatus
        {
            public Socket socket = null;
            public bool ready = true;
        }

        [Header("Listen To")]
        public UPython2ChannelSO listenToChannel = default;

        List<Socket> activatedSockets = new List<Socket>();

        Dictionary<string, SocketStatus> onlineSockets = new Dictionary<string, SocketStatus>(); // always online sockets

        CancellationTokenSource taskCancelToken = new CancellationTokenSource();



        private void OnEnable()
        {
            // subscribe the channels
            listenToChannel.uPython2 = this;

            listenToChannel.python_cmd += Call;
            listenToChannel.python_channel_cmd += Call;
            listenToChannel.close_channel += CloseChannel;
            listenToChannel.channel_status += ChannelStatus;
        }


        private void OnDisable()
        {
            taskCancelToken.Cancel();

            // close all activated sockets
            while (activatedSockets.Count > 0)
            {
                activatedSockets[0].Disconnect(false);
                activatedSockets[0].Close();
                activatedSockets.RemoveAt(0);
            }
            activatedSockets.Clear();
            
            foreach(var k in onlineSockets.Keys)
            {
                onlineSockets[k].socket.Disconnect(false);
                onlineSockets[k].socket.Close();
            }
            onlineSockets.Clear();

            // un-subscribe the channels
            listenToChannel.uPython2 = null;

            listenToChannel.python_cmd -= Call;
            listenToChannel.python_channel_cmd -= Call;
            listenToChannel.close_channel -= CloseChannel;
            listenToChannel.channel_status -= ChannelStatus;
        }

        void Call(string cmd, UnityAction<string> result)
        {
            var t = _Call(cmd, result);
        }

        // call python cmd
        async Task _Call(string cmd, UnityAction<string> result)
        {
            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Connecting to {listenToChannel.host}:{listenToChannel.port} ...");

            c.Connect(listenToChannel.host, listenToChannel.port);

            activatedSockets.Add(c);

            while (!c.Connected)
                await Task.Yield();

            if (listenToChannel.showLog)
                Debug.Log("[UPython2] Connected!");

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Send: {cmd}");


            byte[] bs = Encoding.UTF8.GetBytes(cmd);
            c.Send(bs);

            string recvStr = "";
            byte[] recvBytes = new byte[listenToChannel.dataBuffer];
            int bytes = 0;

            var task = new Task(() =>
            {
                try
                {
                    bytes = c.Receive(recvBytes, recvBytes.Length, 0);
                }
                catch (Exception e)
                {
                    if (listenToChannel.showLog)
                        Debug.Log(e);
                }

            }, taskCancelToken.Token);


            task.Start();

            await Task.WhenAll(task);


            c.Close();
            activatedSockets.Remove(c);

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Disconnected with {listenToChannel.host}:{ listenToChannel.port}");


            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] return: {recvStr}");

            if (recvStr.Length > 0 )
                result.Invoke(recvStr);

        }

        // call and wait to return value
        public async Task<string> ACall(string cmd)
        {
            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Connecting to {listenToChannel.host}:{listenToChannel.port} ...");

            c.Connect(listenToChannel.host, listenToChannel.port);

            activatedSockets.Add(c);

            while (!c.Connected)
                await Task.Yield();

            if (listenToChannel.showLog)
                Debug.Log("[UPython2] Connected!");

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Send: {cmd}");


            byte[] bs = Encoding.UTF8.GetBytes(cmd);
            c.Send(bs);

            string recvStr = "";
            byte[] recvBytes = new byte[listenToChannel.dataBuffer];
            int bytes = 0;

            var task = new Task(() =>
            {
                try
                {
                    bytes = c.Receive(recvBytes, recvBytes.Length, 0);
                }
                catch (Exception e)
                {
                    if (listenToChannel.showLog)
                        Debug.Log(e);
                }

            }, taskCancelToken.Token);


            task.Start();

            await Task.WhenAll(task);

            c.Close();
            activatedSockets.Remove(c);

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Disconnected with {listenToChannel.host}:{ listenToChannel.port}");


            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] return: {recvStr}");

            return recvStr;
        }

        void Call(string channel, string cmd, UnityAction<string> result)
        {
            var t = _Call(channel, cmd, result);
        }

        // call python cmd by channel
        async Task _Call(string channel, string cmd, UnityAction<string> result)
        {
            // GetChannel
            var gc = GetChannel(channel);
            await Task.WhenAll(gc);
            var c = gc.Result;

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Send: {cmd}");


            byte[] bs = Encoding.UTF8.GetBytes(cmd);
            c.socket.Send(bs);

            string recvStr = "";
            byte[] recvBytes = new byte[listenToChannel.dataBuffer];
            int bytes = 0;

            var task = new Task(() =>
            {
                try
                {
                    bytes = c.socket.Receive(recvBytes, recvBytes.Length, 0);
                }
                catch (Exception e)
                {
                    if (listenToChannel.showLog)
                        Debug.Log(e);
                }

            }, taskCancelToken.Token);


            task.Start();

            await Task.WhenAll(task);



            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] return: {recvStr}");

            if (recvStr.Length > 0)
                result.Invoke(recvStr);

            c.ready = true; // ready for next use

        }

        // call and wait to return value
        public async Task<string> ACall(string channel, string cmd)
        {
           
            // GetChannel
            var gc = GetChannel(channel);
            await Task.WhenAll(gc);
            var c = gc.Result;

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Send: {cmd}");


            byte[] bs = Encoding.UTF8.GetBytes(cmd);
            c.socket.Send(bs);

            string recvStr = "";
            byte[] recvBytes = new byte[listenToChannel.dataBuffer];
            int bytes = 0;

            var task = new Task(() =>
            {
                try
                {
                    bytes = c.socket.Receive(recvBytes, recvBytes.Length, 0);
                }
                catch (Exception e)
                {
                    if (listenToChannel.showLog)
                        Debug.Log(e);
                }

            }, taskCancelToken.Token);
            task.Start();
            await Task.WhenAll(task);

            recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] return: {recvStr}");

            c.ready = true; // ready for next use

            return recvStr;
        }

        async Task<SocketStatus> GetChannel(string channel)
        {
            SocketStatus c = null;

            // check if has the channel
            if (onlineSockets.ContainsKey(channel))
            {
                // wait, until the socket get ready!
                while (onlineSockets[channel].ready == false)
                {
                    if (listenToChannel.showLog)
                        Debug.LogWarning($"[UPython2] {channel} is not ready!");
                    await Task.Yield();
                }
                c = onlineSockets[channel];

                if (listenToChannel.showLog)
                    Debug.Log($"[UPython2] Get {channel}:{listenToChannel.host}:{listenToChannel.port} ...");
            }
            else
            {
                c = new SocketStatus();
                onlineSockets.Add(channel, c);

                c.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                
            }


            if(!c.socket.Connected)
            {
                c.socket.Connect(listenToChannel.host, listenToChannel.port);

                if (listenToChannel.showLog)
                    Debug.Log($"[UPython2] Create {channel}:{listenToChannel.host}:{listenToChannel.port} ...");

                while (!c.socket.Connected)
                    await Task.Yield();

                if (listenToChannel.showLog)
                    Debug.Log("[UPython2] Connected!");

            }


            // set used
            c.ready = false;


            return c;
        }


        // close channel immediately
        // in most case, you won't need to call this function
        void CloseChannel(string channel)
        {
            // check if has the channel
            if (onlineSockets.ContainsKey(channel))
            {
                onlineSockets[channel].socket.Disconnect(false);
                onlineSockets[channel].socket.Close();
                onlineSockets.Remove(channel);
            }

            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Close Channel: {channel}");
        }


        // check the given channel's status
        void ChannelStatus(string channel, UnityAction<string, bool> result)
        {

            // check if has the channel
            if (onlineSockets.ContainsKey(channel))
            {
                if (listenToChannel.showLog)
                    Debug.Log($"[UPython2] Channel Status: {channel} = {onlineSockets[channel].ready}");

                result.Invoke(channel, onlineSockets[channel].ready);
            }
            else
            {
                if (listenToChannel.showLog)
                    Debug.Log($"[UPython2] Channel Status: {channel} = {true}");

                result.Invoke(channel, true);
            }
        }

        public bool AChannelStatus(string channel)
        {
            // check if has the channel
            if (onlineSockets.ContainsKey(channel))
            {
                if (listenToChannel.showLog)
                    Debug.Log($"[UPython2] Channel Status: {channel} = {onlineSockets[channel].ready}");

                return onlineSockets[channel].ready;
            }
            else
            {
                if (listenToChannel.showLog)
                    Debug.Log($"[UPython2] Channel Status: {channel} = {true}");

                return true;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Big File
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        // send a big file
        public async Task ASendBigFile(string filePath, byte[] sendingBytes)
        {

            while (onlineSockets.ContainsKey(filePath))
                await Task.Yield();


            // GetChannel by the filepath
            var gc = GetChannel(filePath);
            await Task.WhenAll(gc);
            var c = gc.Result;



            // file
            if (listenToChannel.showLog)
                Debug.Log($"[UPython2] Creating {filePath}");
            var task = new Task(() =>
            {
                try
                {
                    c.socket.Send(Encoding.UTF8.GetBytes(filePath));
                }
                catch (Exception e)
                {
                    if (listenToChannel.showLog)
                        Debug.Log(e);
                }

            }, taskCancelToken.Token);
            task.Start();
            await Task.WhenAll(task);

            // send file
            if (listenToChannel.showLog)
                Debug.Log("[UPython2] DATA TRANSFERRING...");

            task = new Task(() =>
            {
                try
                {
                    c.socket.Send(sendingBytes);
                }
                catch (Exception e)
                {
                    if (listenToChannel.showLog)
                        Debug.Log(e);
                }

            }, taskCancelToken.Token);
            task.Start();
            await Task.WhenAll(task);


            // close channel
            CloseChannel(filePath);

            if (listenToChannel.showLog)
            {
                Debug.Log("[UPython2] DATA TRANSFERRING COMPLETED!");
                Debug.Log($"[UPython2] Disconnected with {listenToChannel.host}:{ listenToChannel.port}");
            }

        }

    }
}
