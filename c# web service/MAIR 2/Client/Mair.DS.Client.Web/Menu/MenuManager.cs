using Mair.DS.Client.Web.Common;
using Mair.DS.Client.Web.Models.Dto;
using Mair.DS.Client.Web.Models.Dto.Auth;
using Mair.DS.Client.Web.Models.Results;
using Mair.DS.Client.Web.Models.Results.Auth;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Mair.DS.Client.Web.Repositories.Auth;
using Mair.DS.Client.Web.Repositories;

namespace Mair.DS.Client.Web.Menu
{
    public class MenuManager
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private Utility utility;
        private readonly AuthenticationsRepository authenticationRepo;
        private readonly RolesRepository roleRepo;
        private readonly RolePathsRepository rolePathRepo;
        private readonly UserRolesRepository userRoleRepo;
        private readonly UsersRepository userRepo;
        private readonly MenuItemsRepository menuItemsRepo;

        public MenuManager()
        {
            authenticationRepo = new AuthenticationsRepository();
            roleRepo = new RolesRepository();
            rolePathRepo = new RolePathsRepository();
            userRoleRepo = new UserRolesRepository();
            userRepo = new UsersRepository();
            menuItemsRepo = new MenuItemsRepository();
            utility = new Utility();
        }

        /// <summary>
        /// Get Menu Items
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<MenuItemResult> GetMenuItems(string token, long userId)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MenuItemResult() { Data = new List<MenuItemDto>(), ResultState = new ResultType() };
                try
                {
                    var menuList = new List<MenuItemDto>() { };
                    var menuListFiltered = new List<MenuItemDto>() { };

                    //call api
                    var userRolesResult = await userRoleRepo.GetAllUserRoles(token);
                    var rolePathsResult = await rolePathRepo.GetAllRolePaths(token);

                    // populate menu
                    //menuList = GetMenuList(token).Result.Data;
                    menuList = MenuList();
                    menuListFiltered.AddRange(menuList);

                    //////////////////////////////////////
                    //to populate table with faults
                    //PopulatePaths(menuList, token);
                    //////////////////////////////////////

                    if (userRolesResult.Successful && rolePathsResult.Successful)
                    {
                        var userRoles = userRolesResult.Data;
                        var rolePaths = rolePathsResult.Data;

                        if (userRoles.Count > 0 && rolePaths.Count > 0)
                        {
                            var roleIds = userRoles.Where(_ => _.UserId == userId).Select(_ => _.RoleId).ToList();
                            var menuItemsExcluded = rolePaths.Where(_ => roleIds.Contains(_.RoleId)).ToList();

                            bool allMenu = menuItemsExcluded.Where(_ => _.Name.Trim().ToLower() == "all").Count() > 0 || menuItemsExcluded.Count() == 0;

                            //filter menu  
                            if (!allMenu)
                            {
                                foreach (var menuItemExcluded in menuItemsExcluded)
                                {
                                    var menuItems = menuList.Where(_ => _.Name.Trim().ToLower() == menuItemExcluded.Name.Trim().ToLower()).ToList();
                                    foreach (var menuItem in menuItems)
                                    {
                                        var path = GetItemPathFromItem(menuList, menuItem.Id, String.Empty);

                                        if (menuItemExcluded.Path.Trim().ToLower() == path.Trim().ToLower())
                                        {
                                            menuListFiltered.Remove(menuItem);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!userRolesResult.Successful || !rolePathsResult.Successful)
                    {
                        //no login
                        //var _menuList = menuListFiltered.Where(_ => _.Name.Trim().ToLower() == "Amministrazione".Trim().ToLower() || _.Name.Trim().ToLower() == "Login".Trim().ToLower()).ToList();

                        //foreach (var item in _menuList.Where(_ => _.ParentId == null).OrderBy(_ => _.Order))
                        //{
                        //    response.Data.Add(item);
                        //    var menu = new List<MenuItemDto>() { };
                        //    var items = GetAllItems(_menuList, item.Id, menu);
                        //    response.Data.AddRange(items);
                        //}
                    }
                    else
                    {
                        var _menuList = menuListFiltered;

                        foreach (var item in _menuList.Where(_ => _.ParentId == null).OrderBy(_ => _.Order))
                        {
                            response.Data.Add(item);
                            var menu = new List<MenuItemDto>() { };
                            var items = GetAllItems(_menuList, item.Id, menu);
                            response.Data.AddRange(items);
                        }
                    }

                    response.Successful = true;
                    response.ResultState = ResultType.Found;
                    response.Message = String.Empty;
                    response.OriginalException = null;
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                }

                return response;
            }
        }

        private async void PopulatePaths(List<MenuItemDto> menuItems, string token)
        {
            foreach (var menuItem in menuItems)
            {
                var path = GetItemPathFromParent(menuItems, menuItem.Id, String.Empty);

                var dto = new RolePathDto() { Name = menuItem.Name, Description = menuItem.Description, IsEnabled = true, RoleId = 4, Path = path };
                await AddRolePath(dto, token);
            }
        }

        private async Task<MenuItemResult> GetMenuList(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new MenuItemResult() { Data = new List<MenuItemDto>(), ResultState = new ResultType() };
                try
                {
                    response = await menuItemsRepo.GetAllMenuItems(token);
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                }

                return response;
            }
        }

        private async Task<RolePathResult> AddRolePath(RolePathDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new RolePathResult() { Data = new List<RolePathDto>(), ResultState = new ResultType() };
                try
                {
                    response = await rolePathRepo.AddRolePath(dto, token);
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                }

                return response;
            }
        }

        private List<MenuItemDto> MenuList()
        {
            var menuList = new List<MenuItemDto>() { };

            menuList.Add(new MenuItemDto() { Id = 0, Order = 0, Enable = true, Visible = true, Name = "Home", ParentId = null, Link = "/", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 1, Order = 1, Enable = true, Visible = true, Name = "DashBoard", ParentId = null, Link = "tagdispatcher", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 2, Order = 2, Enable = true, Visible = true, Name = "Downtime", ParentId = null, Link = "Downtime", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 3, Order = 3, Enable = true, Visible = true, Name = "Tracking", ParentId = null, Link = String.Empty, Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 4, Order = 0, Enable = true, Visible = true, Name = "Material Tracking", ParentId = 3, Link = "MaterialTracking", Description = "Posizioni fisiche dei tubi con le caratteristiche principali del tubo" });
            menuList.Add(new MenuItemDto() { Id = 5, Order = 1, Enable = true, Visible = true, Name = "Event Logger", ParentId = 3, Link = "EventLogger", Description = "Storia dei tubi e delle lavorazioni fatti su di essi" });
            menuList.Add(new MenuItemDto() { Id = 6, Order = 4, Enable = true, Visible = true, Name = "Diagnostic", ParentId = null, Link = "Diagnostic", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 7, Order = 5, Enable = true, Visible = true, Name = "Manutenzione", ParentId = null, Link = "Manutenzione", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 8, Order = 6, Enable = true, Visible = true, Name = "Amministrazione", ParentId = null, Link = String.Empty, Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 9, Order = 0, Enable = true, Visible = true, Name = "Login", ParentId = 8, Link = "login", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 10, Order = 1, Enable = true, Visible = true, Name = "Change Password", ParentId = 8, Link = "changePassword", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 11, Order = 2, Enable = true, Visible = true, Name = "Preferenze", ParentId = 8, Link = "Preferenze", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 12, Order = 3, Enable = true, Visible = true, Name = "Gestione Utenti", ParentId = 8, Link = String.Empty, Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 13, Order = 0, Enable = true, Visible = true, Name = "Users", ParentId = 12, Link = "users", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 14, Order = 1, Enable = true, Visible = true, Name = "Roles", ParentId = 12, Link = "roles", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 15, Order = 2, Enable = true, Visible = true, Name = "User Roles", ParentId = 12, Link = "userRoles", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 16, Order = 3, Enable = true, Visible = true, Name = "Authentications", ParentId = 12, Link = "authentications", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 17, Order = 4, Enable = true, Visible = true, Name = "Create Account", ParentId = 12, Link = "createAccount", Description = "Creazione account" });
            menuList.Add(new MenuItemDto() { Id = 18, Order = 5, Enable = true, Visible = true, Name = "Reset Password", ParentId = 12, Link = "resetPassword", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 19, Order = 6, Enable = true, Visible = true, Name = "Disable Account", ParentId = 12, Link = "disableAccount", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 20, Order = 7, Enable = true, Visible = true, Name = "Enable Account", ParentId = 12, Link = "enableAccount", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 21, Order = 8, Enable = true, Visible = true, Name = "Remove Account", ParentId = 12, Link = "removeAccount", Description = String.Empty });
            menuList.Add(new MenuItemDto() { Id = 22, Order = 9, Enable = true, Visible = true, Name = "Role Paths", ParentId = 12, Link = "rolePaths", Description = String.Empty });

            return menuList;
        }

        private static string GetItemPathFromItem(List<MenuItemDto> items, long? parentId, string path)
        {
            var item = items.Where(_ => _.Id == parentId).FirstOrDefault();
            var separator = "";
            if (path != null && path != String.Empty && item?.Name != null && item?.Name != String.Empty) separator = "/";

            if (item?.Name != null && item?.Name != String.Empty) path = item?.Name + separator + path;

            parentId = item?.ParentId;

            if (parentId == null || parentId == 0) return "/" + path;

            return GetItemPathFromItem(items, parentId, path);
        }

        private static string GetItemPathFromParent(List<MenuItemDto> items, long? parentId, string path)
        {
            var item = items.Where(_ => _.Id == parentId).FirstOrDefault();

            if (item?.Name != null && item?.Name != String.Empty) path = item?.Name + path;

            if (path != null && path != String.Empty && item?.Name != null && item?.Name != String.Empty) path = "/" + path;

            parentId = item?.ParentId;

            if (parentId == null || parentId == 0) return path;

            return GetItemPathFromParent(items, parentId, path);
        }

        private static List<MenuItemDto> GetAllItems(List<MenuItemDto> menuList, long? itemId, List<MenuItemDto> menu)
        {
            if (itemId != null)
            {
                var items = menuList.Where(_ => _.ParentId == itemId).OrderBy(_ => _.Order).ToList();

                foreach (var item in items)
                {
                    menu.Add(item);

                    foreach (var parent in menuList.Where(_ => _.ParentId == item.Id).OrderBy(_ => _.Order).ToList())
                    {
                        menu.Add(parent);
                        long? parentId = parent?.Id;
                        if (parentId != null)
                        {
                            GetAllItems(menuList, parentId, menu);
                        }
                    }
                }
            }

            return menu;
        }

        /// <summary>
        /// Prepare Menu For Html
        /// </summary>
        /// <param name="menuList"></param>
        /// <returns></returns>
        public string PrepareMenuForHtml(List<MenuItemDto> menuList)
        {
            var mainMenuString = String.Empty;
            var menuStringFolder = String.Empty;
            var menuStringFolderContent = String.Empty;
            var menuStringLink = String.Empty;
            var index = -1;
            var level = 0;
            var left = 0;
            var leftSubFolder = 20;
            var firstMarginLeft = 10;
            long? oldParentId = -1;
            long? oldItemId = -1;

            for (int m = 0; m < menuList.Count; m++)
            {
                var item = menuList[m];

                var nextItem = new MenuItemDto() { };

                if (m + 1 < menuList.Count)
                {
                    nextItem = menuList[m + 1];
                }

                long? parentId = item.ParentId;
                long? itemId = item.Id;
                long? nextParentId = null;

                if (nextItem != null)
                {
                    nextParentId = nextItem.ParentId;
                }

                if (item.Visible == true)
                {
                    if ((item.Link == null || item.Link == String.Empty))
                    {
                        if (menuStringFolder != String.Empty)
                        {
                            level = 0;
                            var _left = firstMarginLeft + leftSubFolder;

                            menuStringFolderContent += MenuFolder(level, index, item.Name, "#CONTENT#", left + _left, item.Enable, item.Description);
                            left += leftSubFolder;
                        }
                        else
                        {
                            var _left = firstMarginLeft + leftSubFolder;
                            if (level == 0) _left = firstMarginLeft;
                            menuStringFolder += MenuFolder(level, index, item.Name, "#CONTENT#", left + _left, item.Enable, item.Description);
                        }
                        index += 3;
                        level += 3;
                    }

                    if ((item.Link != null && item.Link != String.Empty))
                    {
                        var _left = firstMarginLeft + leftSubFolder;
                        if (level == 0) _left = firstMarginLeft;
                        menuStringLink += MenuLink(level, index, item.Name, item.Link, left + _left, item.Enable, item.Description);
                        index += 1;
                        level += 1;
                    }
                }

                if (nextParentId == null || nextParentId == itemId || (m + 1) == menuList.Count)
                {
                    if (menuStringFolder != String.Empty && (nextParentId == null || (m + 1) == menuList.Count))
                    {
                        menuStringFolder = menuStringFolder.Replace("#CONTENT#", menuStringLink);
                        menuStringLink = String.Empty;
                    }

                    if (menuStringFolderContent != String.Empty && (nextParentId == itemId || (m + 1) == menuList.Count))
                    {
                        menuStringLink += menuStringFolderContent;
                        menuStringFolderContent = String.Empty;
                        menuStringFolder = menuStringFolder.Replace("#CONTENT#", menuStringLink);
                        menuStringLink = String.Empty;
                    }

                    if (menuStringLink != String.Empty && nextParentId == null)
                    {
                        if (menuStringFolder != String.Empty)
                        {
                            menuStringFolder = menuStringFolder.Replace("#CONTENT#", menuStringLink);
                            menuStringLink = String.Empty;
                        }
                        else
                        {
                            mainMenuString += menuStringLink;
                            menuStringLink = String.Empty;
                        }
                    }

                    if (nextParentId == null)
                    {
                        level = 0;
                        left = 0;
                        mainMenuString += menuStringFolder;
                        menuStringFolder = String.Empty;
                        menuStringLink = String.Empty;
                        menuStringFolderContent = String.Empty;
                    }
                }
                oldItemId = itemId;
                oldParentId = parentId;
            }

            return mainMenuString;
        }

        private static string MenuFolder(int? level, int? index, string title, string content, int left, bool enable, string toolTip)
        {
            var _struct = String.Empty;
            index++;
            _struct += "<div id='item_" + index + "' name='level_" + level + "' style='display:block;' onmouseover='OpenSubMenu(this.id);' onmouseout='LostFocusMenu();'>";
            index++;
            var disableString = "disabled='disabled'";
            if (enable == true) disableString = String.Empty;
            var events = String.Empty;
            if (toolTip != null && toolTip != String.Empty) events = "onmouseover='ShowToolTip(this.id); OpenSubMenu(this.id);' onmouseout='HiddenToolTip(this.id); LostFocusMenu();'";
            else events = "onmouseover='OpenSubMenu(this.id);' onmouseout='LostFocusMenu();'";
            var borderString = "border-left:solid 1px #444;";
            _struct += "<div id='item_" + index + "' name='level_" + (level + 1) + "' class='btn btn-white' style='margin-top:0px; cursor:pointer; text-align:left; margin-left:" + left + "px;  width:100%; height:40px; z-index:99999; display:block; " + borderString + "' " + events + " " + disableString + " >";
            index++;
            _struct += "<div style='float:left;'><b style='color:#222;'>&#9776; </b></div><div style='float:left; margin-left:10px;'>" + title + "</div>";
            _struct += "</div>";
            _struct += "<div id='item_" + index + "' name='level_" + (level + 2) + "' style='display:block;' onmouseover='OpenSubMenu(this.id);' onmouseout='LostFocusMenu();'>";
            _struct += content;
            if (toolTip != null && toolTip != String.Empty) _struct += "<div id = 'toolTip_" + index + "' name = 'toolTip' class='tooltiptextNew' style='visibility:hidden; position:absolute;'> " + toolTip + "</div>";
            _struct += "</div>";
            _struct += "</div>";

            return _struct;
        }

        private static string MenuLink(int? level, int? index, string title, string href, int left, bool enable, string toolTip)
        {
            var _struct = String.Empty;
            index++;
            var disableString = "disabled='disabled'";
            if (enable == true) disableString = String.Empty;
            var events = String.Empty;
            var borderString = "border-left:solid 1px #444;";
            if (toolTip != null && toolTip != String.Empty) events = "onclick='LoadPage(this.id);' onmouseover='ShowToolTip(this.id);' onmouseout='HiddenToolTip(this.id);'";
            else events = "onclick='LoadPage(this.id);'";
            _struct += "<NavLink id='item_" + index + "' class='btn btn-white' style='margin-top:0px; cursor:pointer; text-align:left; margin-left:" + left + "px;  width:100%; height:40px; z-index:99999; display:block; " + borderString + "' href='" + href + "' " + events + " " + disableString + ">";
            if (toolTip != null && toolTip != String.Empty) _struct += "<div id = 'toolTip_" + index + "' name = 'toolTip' class='tooltiptextNew' style='visibility:hidden; position:absolute;'> " + toolTip + "</div>";
            _struct += "<div style='float:left;'><b style='color:#888;'>&#10162; </b></div><div style='float:left; margin-left:10px;'>" + title + "</div>";
            _struct += "</NavLink>";

            return _struct;
        }
    }
}
