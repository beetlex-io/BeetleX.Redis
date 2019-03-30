using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis
{
    public class Result
    {
        public List<ResultItem> Data { get; set; } = new List<ResultItem>();

        public string Messge { get; set; }

        public ResultStatus Status { get; set; } = ResultStatus.None;

        public ResultType ResultType { get; internal set; }

        internal int ArrayCount { get; set; }

        internal int ReadCount { get; set; }

        internal int ArrayReadCount { get; set; }

        internal int? BodyLength { get; set; }

        public bool IsError
        {
            get
            {
                return (this.ResultType == ResultType.DataError ||
                     this.ResultType == ResultType.Error
                     || this.ResultType == ResultType.NetError);
            }
        }

        public object Value
        {
            get
            {
                if (Data.Count > 0)
                    return Data[0].Data;
                return Messge;
            }
        }

    }

    public class ResultItem
    {
        public object Data { get; set; }

        public ResultType Type { get; set; }

        public override string ToString()
        {
            return $"{Type}:{Data}";
        }
    }


}
