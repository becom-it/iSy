﻿using EmployeeInfo.Services;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInfo.Extensions
{
    public static class CanvasOrgChartInteropExtension
    {
        public static async Task InitializeOrgCanvas(this IJSRuntime jsRuntime, string id, DotNetObjectReference<CanvasClickInvoker> clickRef)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("canvasOrgChartJSInterop.Initialize", id, clickRef);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while initializing the org chart canvas: {ex.Message}", ex);
            }
        }
    }
}
