using gdio.common.objects;
using gdio.unity_api;
using gdio.unity_api.v2;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SimpleTest;
using System;

namespace ProgressionBlockerAutomatedTests
{

    [TestFixture]
    public class Setup : Source
    { 


        [OneTimeSetUp]

        public void Connect()
        {
            try
            {
                
                if (pathToExe != null)
                {
                    ApiClient.Launch(pathToExe);
                    api.Connect("localhost", 19734, false, 30);
                }
                else if (testMode == "IDE")
                {
                    api.Connect("localhost", 19734, true, 30);
                }
                else api.Connect("localhost", 19734, false, 30);
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection lost");
                Console.WriteLine(e.ToString());
                throw e;
            }

            api.EnableHooks(HookingObject.MOUSE);
            api.EnableHooks(HookingObject.KEYBOARD);

            //Start the Game
            api.WaitForObject("//*[@name='StartButton']");
            api.ClickObject(MouseButtons.LEFT, "//*[@name='StartButton']", 30);
            api.Wait(3000);
        }


        [Test, Order(0)]
        public void SpawnLocation()
        {
            Vector3 position = api.GetObjectPosition(playerName);
            double diff = Math.Sqrt(Math.Pow(position.x - spawnPos.x, 2) + Math.Pow(position.y - spawnPos.y, 2)); //vector translation magnitude (translacja - przesunięcie w x y) (transformacja - przesunięcie plus rotacja)

            ClassicAssert.Less((diff), tolerance, "postac zespawnowala sie dalej niz expected pos");
            Console.WriteLine("This test verifies if character has spawned on the level and if spawning pos is correct, by comparing it with expected pos");
        }


        [Test, Order(1)]
        public void WalkingSideToSide()
        {
            //Variables + Test Description
            Console.WriteLine("This test verifies if character correctly moves to the left and right correctly by comparing its pos before and after.");
            ulong numFrames = (ulong)(0.5 * api.GetLastFPS());// time for movement

            //Walking right
            Vector3 posBefore = api.GetObjectPosition(playerName);  //get player pos before movement
            api.KeyPress(new KeyCode[] { KeyCode.D }, numFrames);   //movement direction
            api.Wait(1000);
            Vector3 poseAfter = api.GetObjectPosition(playerName);  //get player pos after movement

            double diffX = poseAfter.x - posBefore.x;   //declairing and initilizing diff between before and after on X axis
            Assert.That(diffX, Is.Positive, "Hero moved left instead of right");    //Checking if diffX is positive, if not than
            Assert.That(Math.Abs(diffX), Is.GreaterThan(tolerance), "Hero didn't move more than idle animation");

            //Walking Left
            posBefore = api.GetObjectPosition(playerName);
            api.KeyPress(new KeyCode[] { KeyCode.A }, numFrames);
            api.Wait(1000);
            poseAfter = api.GetObjectPosition(playerName);

            diffX = poseAfter.x - posBefore.x;
            Assert.That(diffX, Is.Negative, "Hero moved right instead of left");
            Assert.That(Math.Abs(diffX), Is.GreaterThan(tolerance), "Hero didn't move more than idle animation");

        }


        [Test, Order(2)]
        public void EnteringZone2()
        {
            //Variables + Test Description
            Console.WriteLine("This test verifies both dropping down mechanic, and passing to zone two. It teleports the player with the use of API method \nto area with applicable drop down, and orders player to press s and space. Reporting of this test focuses only on Passing to Zone 2");
            ulong numFrames = (ulong)(0.5 * api.GetLastFPS());
            Vector3 infoPost1 = api.GetObjectPosition(infoPost);
            Vector3 position = api.GetObjectPosition(playerName);
            api.Wait(1000);

            //Teleport to info post
            api.SetObjectFieldValue(playerName + "/fn:component('UnityEngine.Transform')", "position", infoPost1);
            api.Wait(1000);

            //KeyPress to drop down
            api.KeyPress(new KeyCode[] { KeyCode.S }, numFrames);
            api.Wait(250);
            api.KeyPress(new KeyCode[] { KeyCode.Space }, numFrames);
            api.Wait(3000);

            //Assert if statement after drop down
            if (api.GetSceneName() == "Zone1")
            {
                api.CaptureScreenshot("Passing to zone 2 failed");
                Assert.Fail("Passing to zone 2 failed");

            }
        }            


        [Test, Order(3)]
        public void GettingKey()
        {
            //Test Description
            Console.WriteLine("This test verifies if the key is present within the level, and disappears when picked up");

            //pickup key loop
            if (api.ObjectExists(key))
            {
                Vector3 keyPos = api.GetObjectPosition("/*[@name='Key']/*[@name='Point light']"); // getting goal vector
                api.SetObjectFieldValue("//*[@name='Ellen']/fn:component('UnityEngine.Transform')", "position", keyPos); // teleporting hero to goal
                api.Wait(3000);
                if (api.ObjectExists(key)) // if object does not disappear = assert
                {
                    Assert.Fail("Key was not picked up");
                }
            }
            else Assert.Fail("no key available"); // verifying if key is within the level

        }


        [Test, Order(4)]
        public void EnteringZone3()
        {
            //Variables + Test Description
            ulong numFrames = (ulong)(0.5 * api.GetLastFPS());
            Console.WriteLine("This test verifies if Hero cap pass to Zone3");

            do
            {
                api.KeyPress(new KeyCode[] { KeyCode.D }, 1 * numFrames);
                api.Wait(500);
            }
            while (api.GetSceneName() == "Zone2");

            api.Wait(2000);
            if (api.GetSceneName() != "Zone3")
            {
                Assert.Fail("Failed to reach Zone3");
            }
        }


        [Test, Order(5)]
        public void StandingOnPlatform()
        {
            TeleportToPlat("0");
            api.Wait(1000);
            TeleportToPlat("1");
            api.Wait(1000);
            TeleportToPlat("2");
            api.Wait(1000);
        }


        [Test, Order(5)]
        public void x5()
        {

        }


        [Test, Order(6)]
        public void x6()
        {

        }


        [OneTimeTearDown]
        public void TearDown()
        {
            api.DisableHooks(HookingObject.ALL);
            api.Disconnect();
        }
    }

}

