class CanvasHandler {
    Initialize(canvasId) {
        this.canvasId = canvasId;
        window.addEventListener("resize", (e) => { this.resize(); }, false);
        this.resize();
    }

    resize() {
        let canv = document.getElementById(this.canvasId);
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

window.canvasOrgChartJSInterop = new CanvasHandler();