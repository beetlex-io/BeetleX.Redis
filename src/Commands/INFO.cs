using BeetleX.Buffers;
using BeetleX.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class INFO : Command
    {

        public INFO()
        {
            this.NetworkReceive = OnReceive;
        }

        public override bool Read => true;

        public override string Name => "INFO";

        public InfoSection? Section { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            if (Section != null)
                AddText(Section.Value.ToString());
        }

        private InfoResult mResult = new InfoResult();

        private Result OnReceive(RedisRequest request, PipeStream stream)
        {
            if (mResult.Read(stream))
            {
                Result result = new Result();
                result.ResultType = ResultType.Object;
                result.Data.Add(new ResultItem { Type = ResultType.Object, Data = mResult });
                return result;
            }
            else
                return null;
        }
    }

    public class InfoResult
    {

        public long mLength;

        public bool Read(PipeStream stream)
        {
            if (mLength == 0)
            {
                if (stream.TryReadLine(out string len))
                {
                    mLength = long.Parse(len.Substring(1));
                }
            }
            if (mLength > 0 && mLength <= stream.Length)
            {
                OnLoadInfo(stream);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnLoadInfo(PipeStream stream)
        {
            Dictionary<string, string> properties = Other;
            string line;

            while (stream.TryReadLine(out line))
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                if (line[0] == '#')
                {
                    if (line.IndexOf("server", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Server;
                    }
                    else if (line.IndexOf("clients", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Clients;
                    }
                    else if (line.IndexOf("memory", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Memory;
                    }
                    else if (line.IndexOf("persistence", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Persistence;
                    }
                    else if (line.IndexOf("stats", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Stats;
                    }
                    else if (line.IndexOf("replication", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Replication;
                    }
                    else if (line.IndexOf("cpu", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Cpu;
                    }
                    else if (line.IndexOf("commandstats", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Commandstats;
                    }
                    else if (line.IndexOf("cluster", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Cluster;
                    }
                    else if (line.IndexOf("keyspace", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        properties = Keyspace;
                    }
                    else
                    {
                        properties = Other;
                    }
                }
                else
                {
                    string[] property = line.Split(':');
                    if (property.Length > 1)
                        properties[property[0]] = property[1];
                    else
                        properties[property[0]] = null;
                }
            }

        }

        public Dictionary<string, string> Server { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Clients { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Memory { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Persistence { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Stats { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Replication { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Cpu { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Commandstats { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Cluster { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Keyspace { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Other { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    }


    public enum InfoSection
    {
        server,
        clients,
        memory,
        persistence,
        stats,
        replication,
        cpu,
        commandstats,
        cluster,
        keyspace,
        all,
    }

}
