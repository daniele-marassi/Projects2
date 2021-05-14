using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var _menuList = MenuList();
            var menuList =  new List<MenuItemDto>() { };
            var menuListFiltered = new List<MenuItemDto>() { };
            foreach (var item in _menuList.Where(_ => _.ParentId == null).OrderBy(_ => _.Order))
            {
                menuList.Add(item);
                var menu = new List<MenuItemDto>() { };
                var items = GetAllItems(_menuList, item.Id, menu);
                menuList.AddRange(items);
            }
            
            //var program = new Program();

            //program.PopulatePaths(menuList, "");

            var menuItemsExcluded = GetUserRolePaths();
            menuListFiltered.AddRange(menuList);
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

            foreach (var item in menuListFiltered)
            {
                Console.WriteLine(JsonConvert.SerializeObject(item));
            }
            
            //var path = GetItemPathFromFolder(menuList, 24, String.Empty);
            //Console.WriteLine(path);

            //var mainMenuString = program.PrepareMenuForHtml(menuList);

            //Console.WriteLine(mainMenuString);

            Console.ReadLine();
        }

        private async void PopulatePaths(List<MenuItemDto> menuItems, string token)
        {
            foreach (var menuItem in menuItems)
            {
                var path = GetItemPathFromParent(menuItems, menuItem.Id, String.Empty);
                Console.WriteLine(path);

            }
        }

        private static List<UserRolePathDto> GetUserRolePaths()
        {
            var data = @"[{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti"",""IsEnabled"":true,""Name"":""Gestione Utenti"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.0491627"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.0491558"",""Id"":24},{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti/Utenti"",""IsEnabled"":true,""Name"":""Utenti"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.0888878"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.088881"",""Id"":25},{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti/Ruoli"",""IsEnabled"":true,""Name"":""Ruoli"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.1389499"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.138943"",""Id"":26},{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti/Ruoli Utente"",""IsEnabled"":true,""Name"":""Ruoli Utente"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.1889757"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.1889642"",""Id"":27},{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti/Disable Account"",""IsEnabled"":true,""Name"":""Disable Account"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.2339167"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.2339113"",""Id"":28},{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti/Enable Account"",""IsEnabled"":true,""Name"":""Enable Account"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.2675743"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.2675703"",""Id"":29},{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti/Remove Account"",""IsEnabled"":true,""Name"":""Remove Account"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.3040316"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.3040274"",""Id"":30},{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti/Autenticazioni"",""IsEnabled"":true,""Name"":""Autenticazioni"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.3419982"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.3419924"",""Id"":31},{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti/Create Account"",""IsEnabled"":true,""Name"":""Create Account"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.3777689"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.3777657"",""Id"":32},{ ""UserRoleId"":2,""Path"":""/Amministrazione/Gestione Utenti/Reset Password"",""IsEnabled"":true,""Name"":""Reset Password"",""Description"":null,""Created"":""2020 - 04 - 19T20: 08:30.417392"",""LastUpdated"":""2020 - 04 - 19T20: 08:30.4173874"",""Id"":33}]"; 
            return JsonConvert.DeserializeObject<List<UserRolePathDto>>(data);
        }

        private static List<MenuItemDto> MenuList()
        {
            var menuList = new List<MenuItemDto>() { };

            menuList.Add(new MenuItemDto() { Id = 0, Order = 0, Enable = true, Visible = true, Name = "Home", ParentId = null, Link = "Home", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 1, Order = 1, Enable = true, Visible = true, Name = "DashBoard", ParentId = null, Link = "DashBoard", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 2, Order = 2, Enable = true, Visible = true, Name = "Downtime", ParentId = null, Link = "Downtime", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 3, Order = 3, Enable = true, Visible = true, Name = "Traking", ParentId = null, Link = "", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 4, Order = 0, Enable = true, Visible = true, Name = "Material Traking", ParentId = 3, Link = "MaterialTraking", Description = "Posizioni fisiche dei tubi con le caratteristiche principali del tubo" });
            menuList.Add(new MenuItemDto() { Id = 5, Order = 1, Enable = true, Visible = true, Name = "Event Logger", ParentId = 3, Link = "EventLogger", Description = "Storia dei tubi e delle lavorazioni fatti su di essi" });
            menuList.Add(new MenuItemDto() { Id = 6, Order = 4, Enable = true, Visible = true, Name = "Diagnostic", ParentId = null, Link = "Diagnostic", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 7, Order = 5, Enable = true, Visible = true, Name = "Manutenzione", ParentId = null, Link = "Manutenzione", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 8, Order = 6, Enable = true, Visible = true, Name = "Amministrazione", ParentId = null, Link = "", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 9, Order = 0, Enable = true, Visible = true, Name = "Login", ParentId = 8, Link = "login", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 10, Order = 1, Enable = true, Visible = true, Name = "Preferenze", ParentId = 8, Link = "Preferenze", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 11, Order = 2, Enable = true, Visible = true, Name = "Gestione Utenti", ParentId = 8, Link = "", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 12, Order = 0, Enable = true, Visible = true, Name = "Utenti", ParentId = 11, Link = "user", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 13, Order = 1, Enable = true, Visible = true, Name = "Ruoli", ParentId = 11, Link = "role", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 14, Order = 2, Enable = true, Visible = true, Name = "Ruoli Utente", ParentId = 11, Link = "userRole", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 15, Order = 3, Enable = true, Visible = true, Name = "Autenticazioni", ParentId = 11, Link = "authentication", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 16, Order = 4, Enable = true, Visible = true, Name = "Create Account", ParentId = 11, Link = "createAccount", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 17, Order = 5, Enable = true, Visible = true, Name = "Reset Password", ParentId = 11, Link = "resetPassword", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 18, Order = 6, Enable = true, Visible = true, Name = "Disable Account", ParentId = 11, Link = "disableAccount", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 19, Order = 7, Enable = true, Visible = true, Name = "Enable Account", ParentId = 11, Link = "enableAccount", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 20, Order = 8, Enable = true, Visible = true, Name = "Remove Account", ParentId = 11, Link = "removeAccount", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 21, Order = 9, Enable = true, Visible = true, Name = "Test menu", ParentId = 11, Link = "", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 22, Order = 0, Enable = true, Visible = true, Name = "Test item1", ParentId = 21, Link = "testItem1", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 23, Order = 1, Enable = true, Visible = true, Name = "Test item2", ParentId = 21, Link = "testItem2", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 24, Order = 2, Enable = true, Visible = true, Name = "Test menu x", ParentId = 21, Link = "", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 25, Order = 0, Enable = true, Visible = true, Name = "Test item x1", ParentId = 24, Link = "testItemx1", Description = "" });
            menuList.Add(new MenuItemDto() { Id = 26, Order = 1, Enable = true, Visible = true, Name = "Test item x2", ParentId = 24, Link = "testItemx2", Description = "" });

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
            _struct += "<div id='item_" + index + "' name='level_" + (level + 1) + "' class='btn btn-white' style='margin-top:0px; cursor:pointer; margin-left:" + left + "px;  width:100%; height:40px; z-index:99999; display:block;' " + events + " " + disableString + " >";
            index++;
            _struct += title;
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
            if (toolTip != null && toolTip != String.Empty) events = "onclick='LoadPage(this.id);' onmouseover='ShowToolTip(this.id);' onmouseout='HiddenToolTip(this.id);'";
            else events = "onclick='LoadPage(this.id);'";
            _struct += "<NavLink id='item_" + index + "' class='btn btn-white' style='margin-top:0px; cursor:pointer; margin-left:" + left + "px;  width:100%; height:40px; z-index:99999; display:block;' href='" + href + "' " + events + " " + disableString + ">";
            if (toolTip != null && toolTip != String.Empty) _struct += "<div id = 'toolTip_" + index + "' name = 'toolTip' class='tooltiptextNew' style='visibility:hidden; position:absolute;'> " + toolTip + "</div>";
            _struct += title;
            _struct += "</NavLink>";

            return _struct;
        }
    }
}
