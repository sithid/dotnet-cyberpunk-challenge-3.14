using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnet_cyberpunk_challenge_3_14.malware;
using dotnet_cyberpunk_challenge_3_14.malware.lib._lib;

namespace dotnet_cyberpunk_challenge_3_14
{
    public static class Handler
    {
        public static async Task StartTheChallenges(){
            /*
                OBJECTIVE: Now that we have a working implementation of both individual Arasaka and Militech
                Kuang Primus malware variants as well as a single generic malware capable of attacking both
                companies, we have a request from the Voodoo Boys gang to develop a variant capable of targeting
                Biotechnica. What they're asking for is:
                    1. An individual File called `BiotechnicaKuangPrimusMalware.cs` that has all the same functionality
                    as the Arasaka and Militech variants yet should only target the Biotechnica resources. The reason
                    for thisis that certain Voodoo Boys netrunners should only have ability to target the Biotechnica
                    stuff, not the Arasaka and Militech resources.

                    2. Add the functionality for us to create a variant of the MultiKuangPrimusMalware that's able to
                    target Biotechnica resources by passing in the required generic types. The reason is so that our
                    multi-target malware is not only able to target Arasaka and Militech but also Biotechnica.

                *hint*: You can find more details on *exactly* what to do and sort of a walkthrough in `docs/`. It'll
                include some documentation on the code and how it's built out as well
            */
            await challenge1();
            await challenge2();

            Console.WriteLine("You're done!");
        }

        public static async Task challenge1() {
            ArasakaKuangPrimusMalware arasakaIceBreaker = new ArasakaKuangPrimusMalware();
            await arasakaIceBreaker.Initialize();
            List<ArasakaMessageProcessList> arasakaProcessList = await arasakaIceBreaker.GetProcessList();
            IEnumerable<string> arasakaMemoryMapping = await arasakaIceBreaker.GetProcessMemoryMapping();

            MilitechKuangPrimusMalware militechIceBreaker = new MilitechKuangPrimusMalware();
            await militechIceBreaker.Initialize();
            List<MilitechICEProcessList> militechProcessLists = await militechIceBreaker.GetProcessList();
            IEnumerable<string> militechMemoryMapping = await militechIceBreaker.GetProcessMemoryMapping();

            // TODO: Add BiotechnicaKuangPrimusMalware object and use it to get the process list and process memory mapping
            // essentially just like we're doing above
            // INSERT YOUR CODE
            BiotechnicaKuangPrimusMalware iceBreaker = new BiotechnicaKuangPrimusMalware();
            await iceBreaker.Initialize();
            List<BiotechnicaProcessList> processLists = await iceBreaker.GetProcessList();
            IEnumerable<string> memoryMapping = await iceBreaker.GetProcessMemoryMapping();

        }

        public static async Task challenge2() {
            // FIXME: Mentor note - Need to make the generic Arasaka/Militech Primus Malware
            MultiKuangPrimusMalware<ArasakaMessageRoot, ArasakaMessageProcessList> arasakaIceBreaker = new MultiKuangPrimusMalware<ArasakaMessageRoot, ArasakaMessageProcessList>();
            await arasakaIceBreaker.Initialize();
            List<ArasakaMessageProcessList> arasakaMessageProcessList = await arasakaIceBreaker.GetProcessList();
            IEnumerable<string> arasakaMemoryMapping = await arasakaIceBreaker.GetProcessMemoryMapping();

            MultiKuangPrimusMalware<MilitechMessageRoot, MilitechICEProcessList> militechIceBreaker = new MultiKuangPrimusMalware<MilitechMessageRoot, MilitechICEProcessList>();
            await militechIceBreaker.Initialize();
            List<MilitechICEProcessList> militechICEProcessLists = await militechIceBreaker.GetProcessList();
            IEnumerable<string> militechMemoryMapping = await militechIceBreaker.GetProcessMemoryMapping();

            // TODO: Add the Biotechnica test with `MultiKuangPrimusMalware` object and use it to get 
            // the process list and process memory mapping essentially just like we're doing above
            // INSERT YOUR CODE
            MultiKuangPrimusMalware<BiotechnicaMessageRoot, BiotechnicaProcessList> iceBreaker = new MultiKuangPrimusMalware<BiotechnicaMessageRoot, BiotechnicaProcessList>();
            await iceBreaker.Initialize();
            List<BiotechnicaProcessList> processLists = await iceBreaker.GetProcessList();
            IEnumerable<string> memoryMapping = await iceBreaker.GetProcessMemoryMapping();
        }
    }
}