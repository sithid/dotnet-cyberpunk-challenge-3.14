using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnet_cyberpunk_challenge_3.malware;
using dotnet_cyberpunk_challenge_3.malware.lib._lib;

namespace dotnet_cyberpunk_challenge_3
{
    public static class Handler
    {
        public static async Task StartTheChallenges(){
            /*
                OBJECTIVE: We've learned that Militech has just stolen a number of files from
                Arasaka and while that's great we also want our hands on those files. We can't
                break into Militech YET. 
                
                First we need you to adapt the Kuang malware we used on Arasaka to also be able
                to attack Militech without losing any capability to attack Arasaka either. The
                trick is to make the Kuang malware generic enough that can we spin up an attack
                process for both of them at a moment's notice.

                Couple concepts you'll need to remember from your recent adventures:
                - Async/await and how the `Task` datatype works
                - Generics
                - Abstraction (hiding all the super complicated stuff, like we're doing in the path
                 `lib/_lib/`)
                - Remember we're talking with an actual API and getting data back. Feel free to
                explore "under the hood" in the functions in _lib/

                Good luck!
            */
            await challenge1(); // 10mins
            await challenge2(); // 25mins + 5m talking
            await challenge3(); // 10ish mins

            Console.WriteLine("You're done!");
        }

        public static async Task challenge1() {
            // FIXME: Go back and review this

            /*
                TODO: Objective: We already have instantiation of the Arasaka and Militech variants of the
                malware and the Initialization. Both of the variants have their own Server Connections. This
                needs to be genericized, meaning that we need to go create some generic code to handle this.
                Right now for this challenge we only care about genericizing the ServerConnection because
                we're gonna do more genericizing later.
                    1. Use `F12` on the arasakaIceBreaker's `Initialize()` method. Find more info there! 
                    2. Assuming you completed step 1, then use `F12` to go into 
                    militechIceBreaker's `Initialize()` method
            */
            ArasakaKuangPrimusMalware arasakaIceBreaker = new ArasakaKuangPrimusMalware();
            await arasakaIceBreaker.Initialize();

            MilitechKuangPrimusMalware militechIceBreaker = new MilitechKuangPrimusMalware();
            await militechIceBreaker.Initialize();
        }

        public static async Task challenge2() {
            /*
            TODO: Objective: Okay since we're able to generically use the KuangMalwareFamily base class
            we're going to make a new base class to use instead. We're going to ABANDON using the `KuangMalwareFamily.cs`
                in favor of the MULTI Kaung Malware Family base class to make sure our changes are going to work.

            1. We need to swap the `ArasakaKuangPrimusMalware` and `MilitechKuangPrimusMalware` to use the base class
                `MultiKuangDaemonFamilyBase`. So `F12` into the `ArasakaKuangPrimusMalware` class and do the tasks
                listed as `TODO: challenge2`

                2. You'll notice that you're probably getting red squigglies on `GetArasakaProcessList` below. Change this
                to be the `GetProcessList()`.

                3. Repeat step 1 but do this for `MilitechKuangPrimusMalware`.

                4. You'll notice that you're probably getting red squigglies on `GetMilitechProcessList` below. Change this
                to be the `GetProcessList()`.
                
                5. Move on to the next challenge!

            */

            ArasakaKuangPrimusMalware arasakaIceBreaker = new ArasakaKuangPrimusMalware();
            await arasakaIceBreaker.Initialize();

            List<ArasakaMessageProcessList> arasakaProcessList = await arasakaIceBreaker.GetProcessList();
            IEnumerable<string> arasakaMemoryMapping = await arasakaIceBreaker.GetProcessMemoryMapping();

            MilitechKuangPrimusMalware militechIceBreaker = new MilitechKuangPrimusMalware();
            await militechIceBreaker.Initialize();
            List<MilitechICEProcessList> militechProcessLists = await militechIceBreaker.GetProcessList();
            IEnumerable<string> militechMemoryMapping = await militechIceBreaker.GetProcessMemoryMapping();
        }

        public static async Task challenge3() {
            // FIXME: Mentor note - Need to make the generic Arasaka/Militech Primus Malware

            /*
                TODO: Objective: We've already setup for you how the `MultiKuangPrimusMalware` is going to be
                used to generically create an Arasaka ice breaker and a Militech ice breaker from the same class.
                You'll need to either `F12` into the `MultiKuangPrimusMalware` or simply open it up and make
                sure to do the TODO tasks listed there at the class definition.


                Did you complete it? Yeah?
                Let's run it! Set a breakpoint somewhere and see if you can step through it and explore!
                Generics can be quite satisfying to implement but the mental toll of keeping track during implementation
                can be intense. Get prepared for the homework ;)
            */
            MultiKuangPrimusMalware<ArasakaMessageRoot, ArasakaMessageProcessList> arasakaIceBreaker = new MultiKuangPrimusMalware<ArasakaMessageRoot, ArasakaMessageProcessList>();
            await arasakaIceBreaker.Initialize();
            List<ArasakaMessageProcessList> arasakaMessageProcessList = await arasakaIceBreaker.GetProcessList();
            IEnumerable<string> arasakaMemoryMapping = await arasakaIceBreaker.GetProcessMemoryMapping();

            MultiKuangPrimusMalware<MilitechMessageRoot, MilitechICEProcessList> militechIceBreaker = new MultiKuangPrimusMalware<MilitechMessageRoot, MilitechICEProcessList>();
            await militechIceBreaker.Initialize();
            List<MilitechICEProcessList> militechICEProcessLists = await militechIceBreaker.GetProcessList();
            IEnumerable<string> militechMemoryMapping = await militechIceBreaker.GetProcessMemoryMapping();
        }
    }
}