using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using iSy.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace iSy.Components
{
    public partial class LoginComponent
    {
        private IJSObjectReference _loginModule;

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        public string LogonError { get; set; }

        public string Uname { get; set; }
        public string Password { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _loginModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/login.js");

            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            if (Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("emsg", out var _emsg))
            {
                LogonError = _emsg;
            }
        }

        static string encode(string param)
        {
            return HttpUtility.UrlEncode(param);
        }

        async Task keyDown(string key)
        {
            if(key == "Enter")
            {
                await login();
            }
        }

        async Task login()
        {
            await _loginModule.InvokeVoidAsync("callLogin", encode(Uname), encode(Password));
        }
    }
}
