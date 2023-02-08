//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using Debug = UnityEngine.Debug;

//namespace Assets.Service
//{
//    public class AreaService : BaseService
//    {
//        Dictionary<Guid, Stopwatch> dicoTime = new Dictionary<Guid, Stopwatch>();

//        public event EventHandler<List<AreaDto>> AreasLoaded;
//        public event EventHandler<string> AreaError;
//        public override string HubName => ServerConstants.areaHub;
//        public AreaService()
//        {
//        }

//        public async Task GetAreas(PlayerDto hero)
//        {
//            Debug.Log("GetAreas");
//            var guid = Guid.NewGuid();
//            Stopwatch sw = new Stopwatch();
//            sw.Start();
//            dicoTime.Add(guid, sw);
//            await connection.SendAsync("GetArea", guid, hero);
//            return;
//        }

//        public async override Task Start()
//        {
//            await connection.ConnectAsync();
//            //connection.ServerTimeout = TimeSpan.FromSeconds(10);

//            connection.On<PlayerAreasDto>("AreaLoaded", (result) =>
//            {
//                Debug.Log("AreaLoaded");
//                var sw = dicoTime[result.CallId];
//                sw.Stop();
//                UnityEngine.Debug.Log($"Get AreaHub in {sw.ElapsedMilliseconds} ms");
//                dicoTime.Remove(result.CallId);
//                if (AreasLoaded != null)
//                {
//                    AreasLoaded(this, result.Areas);
//                }
//            });

//            connection.On<string>("AreaError", (result) =>
//            {
//                Debug.Log("AreaError");
//                if (AreaError != null)
//                {
//                    AreaError(this, result);
//                }
//            });
//        }
//    }
//}
