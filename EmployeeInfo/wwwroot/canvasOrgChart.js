class CanvasHandler {
    Initialize(canvasId, clickInvoker) {
        this.canvasId = canvasId;
        this.clickInvoker = clickInvoker;
        this.canvas = document.getElementById(this.canvasId);

        this.canvas.addEventListener("click", (e) => {
            this.clickInvoker.invokeMethodAsync('iSy', 'EmployeeClicked');
            this.clickInvoker.dispose();
            e.stopPropagation();
        });

        window.addEventListener("resize", (e) => { this.resize(); }, false);
        this.resize();
    }

    resize() {
        
        if (this.canvas !== null) {
            console.log("Canvas found! Fit to parent...");
            this.canvas.style.width = '100%';
            this.canvas.style.height = '100%';
            // ...then set the internal size to match
            this.canvas.width = this.canvas.offsetWidth;
            this.canvas.height = this.canvas.offsetHeight;
            console.log("Done!");
        }
    }
}

window.canvasOrgChartJSInterop = new CanvasHandler();