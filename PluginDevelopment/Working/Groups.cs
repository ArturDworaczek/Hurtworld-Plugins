using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("Group System", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 5)]
    [Description("Group System For Hurtworld.")]

    class Groups : CovalencePlugin
    {
        #region Initialization

        string[] AdminRights = new string[0] { };
        string[] SupportRights = new string[1] { "Announcements.Use" };
        string[] VipRights = new string[0] { };

        private void Init()
        {
            if (!permission.GroupExists("admin"))
            {
                permission.CreateGroup("admin", "ADMIN", 0);
            }
            if (AdminRights.Length > 0)
            {
                foreach (string Perm in AdminRights)
                {
                    if (!permission.GroupHasPermission("admin", Perm))
                    {
                        permission.GrantGroupPermission("admin", Perm, this);
                    }
                }
            }
            if (SupportRights.Length > 0)
            {
                foreach (string Perm in SupportRights)
                {
                    if (!permission.GroupHasPermission("admin", Perm))
                    {
                        permission.GrantGroupPermission("admin", Perm, this);
                    }
                }
            }
            if (VipRights.Length > 0)
            {
                foreach (string Perm in VipRights)
                {
                    if (!permission.GroupHasPermission("admin", Perm))
                    {
                        permission.GrantGroupPermission("admin", Perm, this);
                    }
                }
            }

            if (!permission.GroupExists("support"))
            {
                permission.CreateGroup("support", "SUPPORT", 0);
            }
            if (SupportRights.Length > 0)
            {
                foreach (string Perm in SupportRights)
                {
                    if (!permission.GroupHasPermission("support", Perm))
                    {
                        permission.GrantGroupPermission("support", Perm, this);
                    }
                }
            }

            if (!permission.GroupExists("vip"))
            {
                permission.CreateGroup("vip", "VIP", 0);
            }
            if (VipRights.Length > 0)
            {
                foreach (string Perm in VipRights)
                {
                    if (!permission.GroupHasPermission("vip", Perm))
                    {
                        permission.GrantGroupPermission("vip", Perm, this);
                    }
                }
            }
        }

        #endregion
    }
}
