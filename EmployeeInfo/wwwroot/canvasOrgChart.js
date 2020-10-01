window.canvasOrgChartJSInterop = {
    Initialize: canvasId => {
        let canv = document.getElementById(canvasId);
        if (canv !== null) {
            console.log("Canvas found! Fit to parent...");
            canv.style.width = '100%';
            canv.style.height = '100%';
            // ...then set the internal size to match
            canv.width = canv.offsetWidth;
            canv.height = canv.offsetHeight;
            console.log("Done!");
        }
    }
}