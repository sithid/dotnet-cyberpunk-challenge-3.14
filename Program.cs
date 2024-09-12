using System;
using System.Runtime.InteropServices;

namespace dotnet_cyberpunk_challenge_3_14
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Handler.StartTheChallenges();
        }
    }
}