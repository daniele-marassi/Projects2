using Mair.DS.Client.Web.Models.Params.Page;
using Mair.DS.Client.Web.Models.Results.Page;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DS.Client.Web.Common
{
    public class PageManager
    {
        /// <summary>
        /// Next Page
        /// </summary>
        /// <param name="pageManagerParam"></param>
        /// <returns></returns>
        public async Task<PageManagerResult> NextPage(PageManagerParam pageManagerParam)
        {
            var result = new PageManagerResult() { };
            if (pageManagerParam.FromRecord + pageManagerParam.NumberOfrecords >= pageManagerParam.MaxRecords)
            {
                //disable next button
                var _par = new List<string>();
                _par.Add("next_btn");

                var _obj = _par.Cast<object>().ToArray();

                await pageManagerParam.JSRuntime.InvokeVoidAsync("Disable", _obj);
                result.ChangePage = false;
            }
            else
            {
                pageManagerParam.CurrentPage++;
                //get number Of records
                var _par = new List<string>();
                _par.Add("records_cmb");

                var _obj = _par.Cast<object>().ToArray();

                var _numberOfrecords = await pageManagerParam.JSRuntime.InvokeAsync<string>("GetValue", _obj);

                pageManagerParam.NumberOfrecords = int.Parse(_numberOfrecords);

                pageManagerParam.FromRecord += pageManagerParam.NumberOfrecords;

                if (pageManagerParam.FromRecord > pageManagerParam.MaxRecords) pageManagerParam.FromRecord = pageManagerParam.MaxRecords;

                //enable prev button
                _par = new List<string>();
                _par.Add("prev_btn");

                _obj = _par.Cast<object>().ToArray();

                await pageManagerParam.JSRuntime.InvokeVoidAsync("Enable", _obj);

                //reset all tag values
                _par = new List<string>();
                _par.Add("TagValue_Txt");

                _obj = _par.Cast<object>().ToArray();

                await pageManagerParam.JSRuntime.InvokeVoidAsync("ResetValues", _obj);

                result.ChangePage = true;
            }

            result.CurrentPage = pageManagerParam.CurrentPage;
            result.FromRecord = pageManagerParam.FromRecord;
            result.MaxRecords = pageManagerParam.MaxRecords;
            result.NumberOfrecords = pageManagerParam.NumberOfrecords;

            return result;
        }

        /// <summary>
        /// Init Page
        /// </summary>
        /// <param name="pageManagerParam"></param>
        /// <returns></returns>
        public async Task<PageManagerResult> InitPage(PageManagerParam pageManagerParam)
        {
            var result = new PageManagerResult() { };
            //get number Of records
            var _par = new List<string>();
            _par.Add("records_cmb");
            var _obj = _par.Cast<object>().ToArray();

            var _numberOfrecords = await pageManagerParam.JSRuntime.InvokeAsync<string>("GetValue", _obj);

            pageManagerParam.NumberOfrecords = int.Parse(_numberOfrecords);

            pageManagerParam.CurrentPage = 1;
            pageManagerParam.FromRecord = 0;

            if (pageManagerParam.FromRecord + pageManagerParam.NumberOfrecords >= pageManagerParam.MaxRecords)
            {
                //disable next button
                _par = new List<string>();
                _par.Add("next_btn");

                _obj = _par.Cast<object>().ToArray();

                await pageManagerParam.JSRuntime.InvokeVoidAsync("Disable", _obj);
            }
            else
            {
                //enable next button
                _par = new List<string>();
                _par.Add("next_btn");

                _obj = _par.Cast<object>().ToArray();

                await pageManagerParam.JSRuntime.InvokeVoidAsync("Enable", _obj);
            }

            //disable prev button
            _par = new List<string>();
            _par.Add("prev_btn");

            _obj = _par.Cast<object>().ToArray();

            await pageManagerParam.JSRuntime.InvokeVoidAsync("Disable", _obj);

            //reset all tag value
            _par = new List<string>();
            _par.Add("TagValue_Txt");

            _obj = _par.Cast<object>().ToArray();

            await pageManagerParam.JSRuntime.InvokeVoidAsync("ResetValues", _obj);

            result.ChangePage = true;
            result.CurrentPage = pageManagerParam.CurrentPage;
            result.FromRecord = pageManagerParam.FromRecord;
            result.MaxRecords = pageManagerParam.MaxRecords;
            result.NumberOfrecords = pageManagerParam.NumberOfrecords;

            return result;
        }

        /// <summary>
        /// Previous Page
        /// </summary>
        /// <param name="pageManagerParam"></param>
        /// <returns></returns>
        public async Task<PageManagerResult> PreviousPage(PageManagerParam pageManagerParam)
        {
            var result = new PageManagerResult() { };
            pageManagerParam.CurrentPage--;
            //enable next button
            var _par = new List<string>();
            _par.Add("next_btn");

            var _obj = _par.Cast<object>().ToArray();

            await pageManagerParam.JSRuntime.InvokeVoidAsync("Enable", _obj);

            if (pageManagerParam.CurrentPage < 1)
            {
                pageManagerParam.CurrentPage = 1;

                //disable prev button
                _par = new List<string>();
                _par.Add("prev_btn");

                _obj = _par.Cast<object>().ToArray();

                await pageManagerParam.JSRuntime.InvokeVoidAsync("Disable", _obj);

                result.ChangePage = false;
            }
            else
            {
                pageManagerParam.FromRecord -= pageManagerParam.NumberOfrecords;
                if (pageManagerParam.FromRecord < 0) pageManagerParam.FromRecord = 0;

                //reset all tag values
                _par = new List<string>();
                _par.Add("TagValue_Txt");

                _obj = _par.Cast<object>().ToArray();

                await pageManagerParam.JSRuntime.InvokeVoidAsync("ResetValues", _obj);

                result.ChangePage = true;
            }

            result.CurrentPage = pageManagerParam.CurrentPage;
            result.FromRecord = pageManagerParam.FromRecord;
            result.MaxRecords = pageManagerParam.MaxRecords;
            result.NumberOfrecords = pageManagerParam.NumberOfrecords;

            return result;
        }
    }
}