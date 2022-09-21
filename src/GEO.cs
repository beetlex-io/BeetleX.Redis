using BeetleX.Redis.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Redis
{
    public class GEO
    {
        internal GEO(RedisDB db)
        {
            mDB = db;
        }

        private RedisDB mDB;

        public async ValueTask<long> GEOAdd(string key, params (double, double, string)[] items)
        {
            Commands.GEOADD cmd = new GEOADD(key, items);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<double> GEODist(string key, string member1, string member2, GEODISTUnit unit = GEODISTUnit.m)
        {
            Commands.GEODIST cmd = new GEODIST(key, member1, member2, unit);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            if (result.Value != null)
                return double.Parse(result.Value.ToString(), CultureInfo.InvariantCulture);
            return -1;
        }

        public async ValueTask<List<GeoLocation>> GEOPos(string key, params string[] members)
        {
            Commands.GEOPOS cmd = new GEOPOS(key, members);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<GeoLocation> items = new List<GeoLocation>();
            foreach (var item in result.Data)
            {
                if (item.Data == null)
                    items.Add(default(GeoLocation));
                else
                    items.Add((GeoLocation)item.Data);
            }
            return items;
        }


        public ValueTask<List<RadiusItem>> GEORadius(string key, double lon, double lat, int radius, GEODISTUnit unit = GEODISTUnit.m)
        {
            return GEORadius(key, lon, lat, radius, unit, GEORADIUS_ORDER_BY.ASC, 100);
        }

        public async ValueTask<List<RadiusItem>> GEORadius(string key, double lon, double lat, int radius, GEODISTUnit unit, GEORADIUS_ORDER_BY orderby, int count = 100)
        {
            GEORADIUS cmd = new GEORADIUS(key, lon, lat, radius);
            cmd.Unit = unit;
            cmd.ORDER_BY = orderby;
            cmd.Count = count;
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<RadiusItem> data = new List<RadiusItem>();
            foreach (var item in result.Data)
            {
                data.Add((RadiusItem)item.Data);
            }
            return data;
        }


        public ValueTask<List<RadiusItem>> GEORadiusByMember(string key, string member, int radius, GEODISTUnit unit = GEODISTUnit.m)
        {
            return GEORadiusByMember(key, member, radius, unit, GEORADIUS_ORDER_BY.ASC, 100);
        }

        public async ValueTask<List<RadiusItem>> GEORadiusByMember(string key, string member, int radius, GEODISTUnit unit, GEORADIUS_ORDER_BY orderby, int count = 100)
        {
            GEORADIUSBYMEMBER cmd = new GEORADIUSBYMEMBER(key, member, radius);
            cmd.Unit = unit;
            cmd.ORDER_BY = orderby;
            cmd.Count = count;
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<RadiusItem> data = new List<RadiusItem>();
            foreach (var item in result.Data)
            {
                data.Add((RadiusItem)item.Data);
            }
            return data;
        }
    }
}
