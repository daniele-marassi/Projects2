

using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;


/// <summary>
/// Methods from https://developer.mozilla.org/en-US/docs/Web/API/Console
/// </summary>
/// 
namespace JSconsoleExtensionsLib
{
    public static class JSconsoleExtensions
    {

        public static bool enabled { get; set; } = true;

        private static Dictionary<string, List<string>> dictMessages = new Dictionary<string, List<string>>();

        private static MethodBase currentMethod => new StackTrace().GetFrame(1).GetMethod();
        private static ValueTask<bool> InvokeBoolValueMethod(IJSRuntime js, string method, string message, bool repeatOnce)
        {

            List<string> lstMessages = GetMessagesList(method);

            var task = new ValueTask<bool>();
            if (enabled == false) return task;

            var firstRepeat = !lstMessages.Contains(message);
            var repeatThis = repeatOnce == false || (repeatOnce == true && firstRepeat == true);
            if (repeatThis == true)
                task = js.InvokeAsync<bool>(method, new object[] { message });
            if (repeatOnce == true && firstRepeat == true)
                lstMessages.Add(message);
            return task;
        }
        private static ValueTask<string> InvokeStringValueMethod(IJSRuntime js, string method, string message, bool repeatOnce)
        {
            List<string> lstMessages = GetMessagesList(method);

            var task = new ValueTask<string>();
            if (enabled == false) return task;

            var firstRepeat = !lstMessages.Contains(message);
            var repeatThis = repeatOnce == false || (repeatOnce == true && firstRepeat == true);
            if (repeatThis == true)
                task = js.InvokeAsync<string>(method, new object[] { message });
            if (repeatOnce == true && firstRepeat == true)
                lstMessages.Add(message);
            return task;
        }
        private static ValueTask<string> InvokeStringValueMethod(IJSRuntime js, string method, object[] messageObj, bool repeatOnce)
        {
            List<string> lstMessages = GetMessagesList(method);

            var task = new ValueTask<string>();
            if (enabled == false) return task;

            var firstRepeat = !lstMessages.Contains(messageObj.ToString());
            var repeatThis = repeatOnce == false || (repeatOnce == true && firstRepeat == true);
            if (repeatThis == true)
                task = js.InvokeAsync<string>(method, messageObj);
            if (repeatOnce == true && firstRepeat == true)
                lstMessages.Add(messageObj.ToString());
            return task;
        }
        private static List<string> GetMessagesList(string method)
        {
            if (!dictMessages.ContainsKey(method))
            {
                dictMessages.Add(method, new List<string>());
            }
            List<string> lstMessages = dictMessages[method];
            return lstMessages;
        }

        private static void InvokeMethod(IJSRuntime js, string method, string message, bool repeatOnce)
        {
            if (enabled == false) return;
            List<string> lstMessages = GetMessagesList(method);


            var firstRepeat = !lstMessages.Contains(message);
            var repeatThis = repeatOnce == false || (repeatOnce == true && firstRepeat == true);
            if (repeatThis == true)
                js.InvokeAsync<bool>(method, new object[] { message });
            if (repeatOnce == true && firstRepeat == true)
                lstMessages.Add(message);
        }

        private static void InvokeMethod(IJSRuntime js, string method, object[] messageObj, bool repeatOnce)
        {
            if (enabled == false) return;
            List<string> lstMessages = GetMessagesList(method);

            var firstRepeat = !lstMessages.Contains(messageObj[0].ToString());
            var repeatThis = repeatOnce == false || (repeatOnce == true && firstRepeat == true);
            if (repeatThis == true)
                js.InvokeAsync<bool>(method, messageObj);
            if (repeatOnce == true && firstRepeat == true)
                lstMessages.Add(messageObj[0].ToString());
        }
        private static void InvokeMethod(IJSRuntime js, string method, object messageObj, bool repeatOnce)
        {
            if (enabled == false) return;
            List<string> lstMessages = GetMessagesList(method);

            var firstRepeat = !lstMessages.Contains(messageObj.ToString());
            var repeatThis = repeatOnce == false || (repeatOnce == true && firstRepeat == true);
            if (repeatThis == true)
                js.InvokeAsync<bool>(method, messageObj);
            if (repeatOnce == true && firstRepeat == true)
                lstMessages.Add(messageObj.ToString());
        }        /// <summary>
                 /// Not Javascript console methods, but useful nonetheless
                 /// </summary>
                 /// <param name="js"></param>
                 /// <param name="condition"></param>
                 /// <param name="message"></param>
                 /// 
        public static void alert(this IJSRuntime js, string message, bool repeatOnce = false) =>
           InvokeMethod(js, "alert", new object[] { message }, repeatOnce);


        //public static ValueTask<string> prompt(this IJSRuntime jsRuntime, string message)
        //{
        //    // Implemented in JsConsoleExtension.js
        //    return jsRuntime.InvokeAsync<string>(
        //        "JsConsoleExtensionJsFunctions.showPrompt",
        //        message);
        //}

        public static ValueTask<string> prompt(this IJSRuntime js, string message, string defaultText = "", bool repeatOnce = false) =>
            InvokeStringValueMethod(js, "window.prompt", new object[] { message, defaultText }, repeatOnce);


        //public static void title(this IJSRuntime js, string message) =>
        //    JSRuntimeExtensions.InvokeVoidAsync(js, $"(message) => window.document.title = message ;", new object[] { message });
        /// 
        /// ===================================================================================================================4
        /// 


        public static void assert(this IJSRuntime js, bool condition, object message, bool repeatOnce = false) =>
            InvokeMethod(js, "console.assert", new object[] { condition, message }, repeatOnce);

        public static void clear(this IJSRuntime js) =>
            InvokeMethod(js, "console.clear", new object[0], false);

        public static void count(this IJSRuntime js, string label = "", bool repeatOnce = false) =>
            InvokeMethod(js, "console.count", label ?? null, repeatOnce);

        public static void countReset(this IJSRuntime js, string label = "", bool repeatOnce = false) =>
            InvokeMethod(js, "console.countreset", label ?? null, repeatOnce);

        public static void debug(this IJSRuntime js, object input, bool repeatOnce = false) =>
            InvokeMethod(js, "console.debug", input, repeatOnce);

        public static void dirXML(this IJSRuntime js, object input, bool repeatOnce = false) =>
            InvokeMethod(js, "console.dirxml", input, repeatOnce);

        public static void error(this IJSRuntime js, object message, bool repeatOnce = false) =>
            InvokeMethod(js, "console.error", message, repeatOnce);

        /// <summary>
        /// This confirm returns a boolean
        /// </summary>
        /// <param name="js"></param>
        /// <param name="message"></param>
        /// <returns>boolean</returns>
        /// <usage>
        ///     <pre>
        ///         @inject IJSRuntime console
        ///         
        ///         bool answer = await console.confirm( $"Can you CONFIRM this!? What say ye?");
        ///
        ///     </pre>          
        ///
        /// </usage>
        /// 
        public static ValueTask<bool> confirm(this IJSRuntime js, string message, bool repeatOnce = false) =>
            InvokeBoolValueMethod(js, "window.confirm", message, repeatOnce);



        public static void group(this IJSRuntime js, string groupName, bool repeatOnce = false) =>
            InvokeMethod(js, "console.group", groupName, repeatOnce);

        public static void groupCollapsed(this IJSRuntime js, string groupName, bool repeatOnce = false) =>
            InvokeMethod(js, "console.groupCollapsed", groupName, repeatOnce);

        public static void groupEnd(this IJSRuntime js) =>
            InvokeMethod(js, "console.groupEnd", new object[0], false);

        public static async Task icon(this IJSRuntime js, string src)
        {
            IJSObjectReference _module = await Module(js);

            await _module.InvokeAsync<string>("changeFavicon", src);

            static async Task<IJSObjectReference> Module(IJSRuntime js)
            {
                var __module = await js.InvokeAsync<IJSObjectReference>("import", ImportPath).AsTask();
                return __module;
            }
        }

        public static void info(this IJSRuntime js, object message, bool repeatOnce = false) =>
        InvokeMethod(js, "console.info", message, repeatOnce);

        public static void log(this IJSRuntime js, object input, bool repeatOnce = false) =>
            InvokeMethod(js, "console.log", input, repeatOnce);

        public static string methodName(this IJSRuntime js)
        {
            return currentMethod.GetMethodContextName();
        }
        public static string moduleName(this IJSRuntime js)
        {
            return currentMethod.DeclaringType.ReflectedType.FullName;
        }
        public static void profile(this IJSRuntime js, string profileName, bool repeatOnce = false) =>
            InvokeMethod(js, "console.profile", profileName, repeatOnce);

        public static void profileEnd(this IJSRuntime js, string profileName, bool repeatOnce = false) =>
           InvokeMethod(js, "console.profile", profileName, repeatOnce);


        public static void table(this IJSRuntime js, object data, string[] columns = null, bool repeatOnce = false) =>
             InvokeMethod(js, "console.table", new object[] { data, columns }, repeatOnce);

        public static void time(this IJSRuntime js, string timerName, bool repeatOnce = false) =>
            InvokeMethod(js, "console.time", timerName, repeatOnce);

        public static void timeEnd(this IJSRuntime js, string timerName, bool repeatOnce = false) =>
            InvokeMethod(js, "console.timeEnd", timerName, repeatOnce);

        public static void timeLog(this IJSRuntime js, string timerName, bool repeatOnce = false) =>
            InvokeMethod(js, "console.timelog", timerName, repeatOnce);

        public static void timeStamp(this IJSRuntime js, string label, bool repeatOnce = false) =>
            InvokeMethod(js, "console.timestamp", label, repeatOnce);


        static string ImportPath = "./_content/JSconsoleExtensionsLib/JSconsoleExtensions.js";
        public static async Task title(this IJSRuntime js, string title)
        {
            IJSObjectReference _module = await Module(js);

            await _module.InvokeAsync<string>("blazorSetTitle", title);

            static async Task<IJSObjectReference> Module(IJSRuntime js)
            {
                var __module = await js.InvokeAsync<IJSObjectReference>("import", ImportPath).AsTask();
                return __module;
            }



            //string ImportPath = "./_content/JSconsoleExtensionsLib/JSconsoleExtensions.js";
            //try
            //{
            //    var Module = InvokeAsync<IJSObjectReference>("import", new object[] { ImportPath }).Result;

            //    Module.InvokeAsync<string>("blazorSetTitle", title);
            //}
            //catch(Exception ex)
            //{
            //    JSconsoleExtensions.alert(js,ex.ToString());
            //}

            //var objectReference = js.InvokeAsync<IJSObjectReference>("getJavaScriptObject");

            //// Get a ref to our js file
            //var module = js.InvokeAsync<IJSObjectReference>(
            //  "import", "./_content/JSconsoleExtensionsLib5/JSconsoleExtensions.js");

            //  module.InvokeAsync<string>(js, "blazorSetTitle", title);

            //////async ValueTask<string> Prompt(string message)
            //////{
            //////    return await module.InvokeAsync<string>(js, "showPrompt", message);
            //////}    
            ////InvokeMethod(js, "(function(){ window.document.title = '" + title + "'; })();", new object[] { title }, false);
        }



        public static void trace(this IJSRuntime js) =>
            InvokeMethod(js, "console.trace", new object[0], false);


        public static void warn(this IJSRuntime js, object message, bool repeatOnce = false) =>
            InvokeMethod(js, "console.warn", message, repeatOnce);
    }

}

public static class ReflectionExtensions
{
    /// <summary>
    /// Because the method MethodInfo.GetCurrentMethod().Name returns "MoveNext" instead of the method name
    /// for async methods, 
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public static string GetMethodContextName(this MethodBase method)
    {
        if (method.DeclaringType.GetInterfaces().Any(i => i == typeof(IAsyncStateMachine)))
        {
            var generatedType = method.DeclaringType;
            var originalType = generatedType.DeclaringType;
            var foundMethod = originalType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
            return foundMethod.DeclaringType.Name + "." + foundMethod.Name;
        }
        else
        {
            return method.DeclaringType.Name + "." + method.Name;
        }
    }
}
