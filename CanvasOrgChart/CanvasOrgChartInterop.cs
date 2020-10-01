using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CanvasOrgChart
{
    public static class CanvasOrgChartInterop
    {
        public static async Task InitializeOrgCanvas(this IJSRuntime jsRuntime, string id)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("canvasOrgChartJSInterop.SetupSmiley", id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while initializing the org chart canvas: {ex.Message}", ex);
            }
        }
    }
}
