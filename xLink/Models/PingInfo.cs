﻿using System;

namespace xLink.Models
{
    /// <summary>心跳信息</summary>
    public class PingInfo
    {
        #region 属性
        /// <summary>可用内存大小</summary>
        public UInt64 AvailableMemory { get; set; }

        /// <summary>磁盘可用空间。应用所在盘</summary>
        public UInt64 AvailableSpace { get; set; }

        /// <summary>主频</summary>
        public Single CpuRate { get; set; }

        /// <summary>温度</summary>
        public Double Temperature { get; set; }

        /// <summary>MAC地址</summary>
        public String Macs { get; set; }

        /// <summary>串口</summary>
        public String COMs { get; set; }

        /// <summary>进程列表</summary>
        public String Processes { get; set; }

        /// <summary>开机时间，ms</summary>
        public Int32 Uptime { get; set; }

        /// <summary>本地UTC时间。ms毫秒</summary>
        public Int64 Time { get; set; }

        /// <summary>延迟</summary>
        public Int32 Delay { get; set; }
        #endregion
    }

    /// <summary>心跳响应</summary>
    public class PingResponse
    {
        /// <summary>本地时间。ms毫秒</summary>
        public Int64 Time { get; set; }

        /// <summary>服务器时间</summary>
        public DateTime ServerTime { get; set; }

        /// <summary>下发命令</summary>
        public CommandModel[] Commands { get; set; }
    }

    /// <summary>命令模型</summary>
    public class CommandModel
    {
        /// <summary>序号</summary>
        public Int32 Id { get; set; }

        /// <summary>命令</summary>
        public String Command { get; set; }

        /// <summary>参数</summary>
        public String Argument { get; set; }

        //public String Result { get; set; }
    }
}