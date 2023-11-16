using Android.App;
using Android.Content;
using Android.Net.Http;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Supp.App
{
    public class WebChromeClientOverride : WebChromeClient
    {
        public void onPermissionRequest(PermissionRequest request)
        {
            request.Grant(request.GetResources());
        }
    }
}