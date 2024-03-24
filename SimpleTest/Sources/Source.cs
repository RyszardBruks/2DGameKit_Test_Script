using gdio.common.objects;
using gdio.unity_api;
using gdio.unity_api.v2;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;

namespace SimpleTest
{
    public class Source
    {
        public static string path = null;
        public static string mode = "IDE";

        public string testMode = TestContext.Parameters.Get("Mode", "IDE");
        public string testHost = TestContext.Parameters.Get("Host", "localhost");
        public string executablePath = TestContext.Parameters.Get("executablePath", @"C:\Users\user\Builds\UnityBuild.exe");
        public string pathToExe = TestContext.Parameters.Get("pathToExe", path);

        public double tolerance = 0.0010; // wartość róznicy pomiędzy wektorami
        public Vector3 spawnPos = new Vector3(-2.244f, -4.236084f, 0f);
        public string playerName = "//*[@name='Ellen']";
        public string infoPost = "//*[@name='InfoPost'][1]";
        public string key = "(//*[@name='Key'])[3]";


        public static ApiClient api = new ApiClient();
        int id = 0;


        //METHODS 🤑//METHODS 🤑//METHODS 🤑//METHODS 🤑//METHODS 🤑//METHODS 🤑//METHODS 🤑//METHODS 🤑//METHODS 🤑//METHODS 🤑//METHODS 🤑
        public void TeleportToPlat(string platName)
        {
            double tolerancePlat = 0.5000;

            Vector3 platform = api.GetObjectPosition($"(//*[@name='MovingPlatform'])[{platName}]");
            api.SetObjectFieldValue(playerName + "/fn:component('UnityEngine.Transform')", "position", platform);
            api.Wait(500);
            //pobrać wektor z momentu X
            Vector3 playerPosT = api.GetObjectPosition(playerName);
            Console.WriteLine(playerPosT);
            api.Wait(1500);

            Vector3 playerPosT1 = api.GetObjectPosition(playerName);
            Console.WriteLine(playerPosT1);

            //porównać wartość y wektorów true - jest taka sama
            double compareY = playerPosT.y - playerPosT1.y;
            double compareX = playerPosT.x - playerPosT1.x;
            Assert.That(compareY < tolerancePlat && Math.Abs(compareX) > 0, $"Hero didn't teleport to expected loc:compareY = {compareY},compareX = {compareX}");
        }
    }
}
