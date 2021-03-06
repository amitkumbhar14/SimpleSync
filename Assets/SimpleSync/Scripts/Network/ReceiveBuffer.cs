using UnityEngine;
using System;

namespace SimpleSync.Network
{
    public class ReceiveBuffer
    {
        #region Member Data
        byte[] m_cBuffer;
        int m_uiBufferHead;
        int m_uiBufferTail;
        object m_kLocker;
        #endregion

        #region Member Data
        public void Init(int uiBuffSize)
        {
            m_cBuffer = new byte[uiBuffSize];
            m_kLocker = new object();
            m_uiBufferHead = 0;
            m_uiBufferTail = 0;
        }

        public void AppendData(ref byte[] data, int dataSize)
        {
            lock (m_kLocker)
            {
                if (dataSize > (m_cBuffer.Length - m_uiBufferTail))
                {
                    Debug.LogError("RecvBuffer full,fatal error");
                    return;
                }

                Buffer.BlockCopy(data, 0, m_cBuffer, m_uiBufferTail, dataSize);
                m_uiBufferTail += dataSize;
            }
        }

        public bool PopData(ref byte[] data, int dataSize)
        {
            lock (m_kLocker)
            {
                if (dataSize > (m_uiBufferTail - m_uiBufferHead))
                {
                    Debug.LogError("RecvBuff invalid,fatal error,dataSize=" + dataSize);
                    string kMsg = "Tail " + m_uiBufferTail.ToString() + " , Head " + m_uiBufferHead.ToString() + " , DataSize " + dataSize.ToString();
                    Debug.LogError("Protocol Buffer PopData Error " + kMsg);
                    return false;
                }

                Buffer.BlockCopy(m_cBuffer, m_uiBufferHead, data, 0, dataSize);

                m_uiBufferHead += dataSize;
            }
            return true;
        }

        public bool PopData(int dataSize)
        {
            lock (m_kLocker)
            {
                if (dataSize > (m_uiBufferTail - m_uiBufferHead))
                {
                    Debug.LogError("RecvBuff invalid,fatal error,dataSize=" + dataSize);
                    string kMsg = "Tail " + m_uiBufferTail.ToString() + " , Head " + m_uiBufferHead.ToString() + " , DataSize " + dataSize.ToString();
                    Debug.LogError( "Protocol Buffer PopData Error " + kMsg );
                    return false;
                }

                m_uiBufferHead += dataSize;
            }
            return true;
        }

        public bool GetData(ref byte[] data, int dataSize)
        {
            lock (m_kLocker)
            {
                if (dataSize > (m_uiBufferTail - m_uiBufferHead))
                {
                    Debug.LogError("RecvBuff invalid,fatal error,dataSize=" + dataSize);
                    string kMsg = "Tail " + m_uiBufferTail.ToString() + " , Head " + m_uiBufferHead.ToString() + " , DataSize " + dataSize.ToString();
                    Debug.LogError("Protocol Buffer PopData Error " + kMsg);
                    return false;
                }

                Buffer.BlockCopy(m_cBuffer, m_uiBufferHead, data, 0, dataSize);
            }
            return true;
        }

        public int GetBuffValidSize()
        {
            return m_uiBufferTail - m_uiBufferHead;
        }

        public void Regroup()
        {
            lock (m_kLocker)
            {
                int uiLeftDataSize = m_uiBufferTail - m_uiBufferHead;
                if (uiLeftDataSize <= 0)
                {
                    m_uiBufferTail = 0;
                    m_uiBufferHead = 0;
                }

                byte[] tempdata = new byte[uiLeftDataSize];
                Buffer.BlockCopy(m_cBuffer, m_uiBufferHead, tempdata, 0, uiLeftDataSize);

                Buffer.BlockCopy(tempdata, 0, m_cBuffer, 0, uiLeftDataSize);

                m_uiBufferHead = 0;
                m_uiBufferTail = uiLeftDataSize;
            }
        }
        #endregion
    }

}
