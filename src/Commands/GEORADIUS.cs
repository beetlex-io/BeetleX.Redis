using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class GEORADIUS : Command
    {

        public GEORADIUS(string key, double lon, double lat, int radius)
        {
            Key = key;
            Longitude = lon;
            Latitude = lat;
            Radius = radius;
            NetworkReceive = OnReceive;
        }
        public override bool Read => true;

        public override string Name => "GEORADIUS";

        public string Key { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public int Radius { get; set; }

        public GEODISTUnit Unit { get; set; } = GEODISTUnit.m;

        public GEORADIUS_WITH With { get; set; } = GEORADIUS_WITH.NONE;

        public GEORADIUS_ORDER_BY ORDER_BY { get; set; } = GEORADIUS_ORDER_BY.ASC;

        public int Count { get; set; } = 100;

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Longitude);
            AddText(Latitude);
            AddText(Radius);
            AddText(Unit.ToString());
            AddText("WITHDIST");
            AddText("WITHCOORD");
            AddText(ORDER_BY.ToString());
            AddText("COUNT");
            AddText(Count);

        }

        private ArrayReader mReader;

        private Result OnReceive(RedisRequest request, PipeStream pipeStream)
        {
            if (mReader == null)
            {
                if (pipeStream.TryReadLine(out string line))
                {
                    if (line[0] == '-')
                        throw new RedisException(line.Substring(1));
                    var count = int.Parse(line.Substring(1));
                    if (count == 0)
                        return new Result { ResultType = ResultType.Arrays };
                    mReader = new ArrayReader(this, count, (client, cmd, stream, length, level) =>
                    {
                        return stream.ReadString(length);
                    });
                }
                else
                    return null;
            }
            if (mReader.Read(pipeStream, request.Client) == ArrayReader.Status.End)
            {
                Result result = new Result();
                result.ResultType = ResultType.Arrays;
                var data = mReader.Items;
                foreach (var item in data)
                {
                    RadiusItem ritem = new RadiusItem();
                    List<ArrayDataItem> baseinfo = (List<ArrayDataItem>)item.Value;
                    List<ArrayDataItem> locationInfo = (List<ArrayDataItem>)baseinfo[2].Value;
                    ritem.Name = (string)baseinfo[0].Value;
                    ritem.Value = double.Parse((string)baseinfo[1].Value);
                    ritem.Longitude = Double.Parse((string)locationInfo[0].Value);
                    ritem.Latitude = Double.Parse((string)locationInfo[1].Value);
                    result.Data.Add(new ResultItem { Type = ResultType.Object, Data = ritem });
                }
                return result;
            }
            return null;
        }
    }

    public class GEORADIUSBYMEMBER : Command
    {
        public override bool Read => true;

        public override string Name => "GEORADIUSBYMEMBER";

        public GEORADIUSBYMEMBER(string key, string member, int radius)
        {
            Key = key;
            Member = member;
            Radius = radius;
            NetworkReceive = OnReceive;
        }

        public string Key { get; set; }

        public string Member { get; set; }

        public int Radius { get; set; }

        public GEODISTUnit Unit { get; set; } = GEODISTUnit.m;

        public GEORADIUS_WITH With { get; set; } = GEORADIUS_WITH.NONE;

        public GEORADIUS_ORDER_BY ORDER_BY { get; set; } = GEORADIUS_ORDER_BY.ASC;

        public int Count { get; set; } = 100;

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Member);
            AddText(Radius);
            AddText(Unit.ToString());
            AddText("WITHDIST");
            AddText("WITHCOORD");
            AddText(ORDER_BY.ToString());
            AddText("COUNT");
            AddText(Count);

        }

        private ArrayReader mReader;

        private Result OnReceive(RedisRequest request, PipeStream pipeStream)
        {
            if (mReader == null)
            {
                if (pipeStream.TryReadLine(out string line))
                {
                    if (line[0] == '-')
                        throw new RedisException(line.Substring(1));
                    var count = int.Parse(line.Substring(1));
                    if (count == 0)
                        return new Result { ResultType = ResultType.Arrays };
                    mReader = new ArrayReader(this, count, (client, cmd, stream, length, level) =>
                    {
                        return stream.ReadString(length);
                    });
                }
                else
                    return null;
            }
            if (mReader.Read(pipeStream, request.Client) == ArrayReader.Status.End)
            {
                Result result = new Result();
                result.ResultType = ResultType.Arrays;
                var data = mReader.Items;
                foreach (var item in data)
                {
                    RadiusItem ritem = new RadiusItem();
                    List<ArrayDataItem> baseinfo = (List<ArrayDataItem>)item.Value;
                    List<ArrayDataItem> locationInfo = (List<ArrayDataItem>)baseinfo[2].Value;
                    ritem.Name = (string)baseinfo[0].Value;
                    ritem.Value = double.Parse((string)baseinfo[1].Value);
                    ritem.Longitude = Double.Parse((string)locationInfo[0].Value);
                    ritem.Latitude = Double.Parse((string)locationInfo[1].Value);
                    result.Data.Add(new ResultItem { Type = ResultType.Object, Data = ritem });
                }
                return result;
            }
            return null;
        }
    }



    public enum GEORADIUS_ORDER_BY
    {
        ASC,
        DESC
    }

    public struct RadiusItem
    {
        public string Name { get; set; }

        public double Value { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }



    public enum GEORADIUS_WITH
    {
        NONE = 1,
        WITHCOORD = 2,
        WITHDIST = 4,
        WITHHASH = 8
    }

}
