using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// MIT license
/// Created by Haikun Huang
/// Date: 2021
/// </summary>
///


namespace DCXR
{
    [CreateAssetMenu(menuName ="DCXR/UPython2 Channel SO", fileName = "UPython2 Channel SO")]
    public class UPython2ChannelSO : ScriptableObject
    {

        public string host = "127.0.0.1";
        public int port = 8888;
        public int dataBuffer = 4096;
        public bool showLog = true;

        // Action
        public UnityAction<string, UnityAction<string>> python_cmd;
        public UnityAction<string, string, UnityAction<string>> python_channel_cmd;

        public UnityAction<string> close_channel;
        public UnityAction<string, UnityAction<string, bool>> channel_status;

        // Big File
        public UnityAction<string, byte[], bool> send_bigfile;

        [HideInInspector]
        public UPython2 uPython2;

        public void Call(string cmd, UnityAction<string> result)
        {
            python_cmd.Invoke(cmd, result);
        }

        public void Call(string channel, string cmd, UnityAction<string> result)
        {
            python_channel_cmd.Invoke(channel, cmd, result);
        }

        public void Close(string channel)
        {
            close_channel.Invoke(channel);
        }

        public void ChannelStatus(string channel, UnityAction<string, bool> result)
        {
            channel_status.Invoke(channel, result);
        }

        public bool AChannelStatus(string channel)
        {
            return uPython2.AChannelStatus(channel);
        }

        public async Task<string> ACall(string cmd)
        {
            var t = uPython2.ACall(cmd);
            await Task.WhenAll(t);
            return t.Result;
        }

        public async Task<string> AChannelCall(string channel, string cmd)
        {
            var t = uPython2.ACall(channel, cmd);
            await Task.WhenAll(t);
            return t.Result;
        }

        public async Task ASendBigFile(string filePath, byte[] sendingBytes)
        {
            var t = uPython2.ASendBigFile(filePath, sendingBytes);
            await Task.WhenAll(t);
        }

    }
}