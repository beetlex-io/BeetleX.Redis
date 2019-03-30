using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis
{
    public abstract class Command
    {
        static Command()
        {
            LineBytes = Encoding.ASCII.GetBytes("\r\n");
        }

        private static byte[] LineBytes;

        public Command()
        {

        }

        public Func<Result, PipeStream, RedisClient, bool> Reader { get; set; }

        public abstract bool Read { get; }

        public IDataFormater DataFormater { get; set; }

        public abstract string Name { get; }

        private List<CommandParameter> mParameters = new List<CommandParameter>();

        protected Command AddText(object text)
        {
            mParameters.Add(new CommandParameter { Value = text });
            return this;
        }

        protected Command AddData(object data)
        {
            mParameters.Add(new CommandParameter { Value = data, DataFormater = this.DataFormater, Serialize = true });
            return this;
        }

        public virtual void OnExecute()
        {
            mParameters.Add(new CommandParameter { Value = Name });
        }

        public void Execute(RedisClient client, PipeStream stream)
        {
            OnExecute();
            string headerStr = $"*{mParameters.Count}\r\n";
            stream.Write(headerStr);
            for (int i = 0; i < mParameters.Count; i++)
            {
                mParameters[i].Write(client, stream);
            }
        }

        public class CommandParameter
        {
            public object Value { get; set; }

            public IDataFormater DataFormater { get; set; }

            public bool Serialize
            {
                get; set;
            } = false;

            [ThreadStatic]
            private static byte[] mBuffer = null;

            public void Write(RedisClient client, PipeStream stream)
            {
                if (Serialize && DataFormater != null)
                {
                    DataFormater.SerializeObject(Value, client, stream);
                }
                else
                {
                    string value = Value as string;
                    if (value == null)
                        value = Value.ToString();
                    if (mBuffer == null)
                        mBuffer = new byte[1024 * 1024];
                    int len = Encoding.UTF8.GetBytes(value, 0, value.Length, mBuffer, 0);
                    stream.Write($"${len}\r\n");
                    stream.Write(mBuffer, 0, len);

                }
                stream.Write(LineBytes, 0, 2);
            }
        }

    }
}
