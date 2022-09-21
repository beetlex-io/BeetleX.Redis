using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class GEOPOS : Command
    {

        public GEOPOS(string key, params string[] members)
        {
            Key = key;
            Members = members;
            this.NetworkReceive = OnReceive;

        }

        public string Key { get; private set; }

        public string[] Members { get; set; }

        public override bool Read => true;

        public override string Name => "GEOPOS";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            foreach (var item in Members)
                AddText(item);

        }

        private ArrayReader mReader;

        private Result OnReceive(RedisRequest request, PipeStream pipeStream)
        {
            if (mReader == null)
            {
                if (pipeStream.TryReadLine(out string line))
                {
                    var count = int.Parse(line.Substring(1));
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
                var items = mReader.Items;
                Result data = new Result();
                foreach (var item in items)
                {
                    List<ArrayDataItem> childs = (List<ArrayDataItem>)item.Value;
                    GeoLocation location = new GeoLocation();
                    if (childs.Count > 0)
                    {
                        location.Longitude = double.Parse((string)childs[0].Value, CultureInfo.InvariantCulture);
                        location.Latitude = double.Parse((string)childs[1].Value, CultureInfo.InvariantCulture);

                    }
                    data.Data.Add(new ResultItem { Type = ResultType.Object, Data = location });
                }
                return data;
            }
            return null;
        }

    }

    public struct GeoLocation
    {
        public double Longitude;

        public double Latitude;
    }

}
