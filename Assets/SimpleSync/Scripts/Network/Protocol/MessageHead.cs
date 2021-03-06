using System;
using System.IO;

namespace SimpleSync.Protocol
{
    public class MSG_HEAD
    {
        public UInt16 m_usSize;
        public UInt16 m_usMsgType;

        public void Encode(BinaryWriter outputStream)
        {
            outputStream.Write(m_usSize);
            outputStream.Write(m_usMsgType);
        }

        public void Decode(BinaryReader inputStream)
        {
            m_usSize = inputStream.ReadUInt16();
            m_usMsgType = inputStream.ReadUInt16();
        }
    }

    public enum MsgHeadSize
    {
        CS_MG_HEAD_SIZE = 4,
    }
}
