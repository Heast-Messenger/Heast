using System.ComponentModel.DataAnnotations.Schema;
using ChatServer.network;
using ChatServer.permissionengine;

namespace ChatServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Database.Init();
            //PermissionsEngine.init();
        }
    }
}

