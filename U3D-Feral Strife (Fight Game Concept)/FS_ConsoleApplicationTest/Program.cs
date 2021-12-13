//=========== Written by Arthur W. Sheldon AKA Lizband_UCC ====================
//
// SID: AKC
// Purpose: 
// Applied to: 
// Editor script: 
// Notes: 
//
//=============================================================================

using System;

namespace FS_ConsoleApplicationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "NW_FS007_Server";
            Server.Start(3, 25568); // Start server with a max of 3 players on port 25568
            Console.ReadKey();
        }
    }
}
