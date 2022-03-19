using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models
{
    public class AuthRequest
    {
        public static int id;
        public static int roleId;
        public static string name;
        public static string roleName;
        public static byte[] avt;

        public static List<string> permissions = new List<string>();

        public static void GetListPermission()
        {
            /* BitteroMVCEntities h = new BitteroMVCEntities();
             foreach (var item in h.role_permissions)
             {
                 if (item.role_id == roleId)
                     permissions.Add(GetCodePermission((int)item.permission_id));
             }*/

        }
        public static string GetCodePermission(int id)
        {
            /*   BitteroMVCEntities h = new BitteroMVCEntities();
               permission p = h.permissions.FirstOrDefault(x => x.id == id);
               if (p != null)
               {
                   return p.code;
               }*/
            return null;
        }
        public static bool hasPermission(string permissionCode)
        {
            foreach (var item in permissions)
            {
                if (item == permissionCode)
                {
                    return true;
                }

            }
            if (permissions.Count == 0) return false;
            return false;
        }
        public static void ClearSession()
        {
            permissions.Clear();
            id = 0;
            roleId = 0;
            name = null;
            roleName = null;
        }
        public static void SetCurrent(int Id, int roleID, string _name,byte[] avtbyte)
        {
            id = Id;
            roleId = roleID;
            name = _name;
            avt = avtbyte;

        }
      
    }
}
