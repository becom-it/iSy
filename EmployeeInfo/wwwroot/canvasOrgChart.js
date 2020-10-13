import { OrgChartService } from "./orgChart/orgChartService.js";
import { OrgChartVm } from "./orgChart/models/orgChartModels.js";

class CanvasHandler {

    constructor() {
        this.isInitialized = false;
    }

    Initialize(canvasId, clickInvoker) {
        
        this.isDrawing = false;
        this.canvasId = canvasId;
        this.clickInvoker = clickInvoker;
        this.canvas = document.getElementById(this.canvasId);
        this.viewModel = null;

        window.addEventListener("resize", (e) => { this.resize(); }, false);
        this.resize();

        this.service = new OrgChartService();
        this.service.initialize(this.canvas, clickInvoker);

        this.isInitialized = true;
    }

    /**
     * @param {OrgChartVm} viewModel
     */
    DrawOrg(viewModel) {

        this.isDrawing = true;
        this.viewModel = viewModel;
        this.service.drawOrg(viewModel);
        this.isDrawing = false;
    }

    resize() {
        
        if (this.canvas !== null && this.isDrawing === false) {
            console.log("Canvas found! Fit to parent...");
            this.canvas.style.width = '100%';
            this.canvas.style.height = '100%';
            // ...then set the internal size to match
            this.canvas.width = this.canvas.offsetWidth;
            this.canvas.height = this.canvas.offsetHeight;
            console.log("Done!");

            if (this.viewModel !== null) {
                this.service.calculateMaxNodes();
                this.DrawOrg(this.viewModel);
            }
        }
    }
}

window.canvasOrgChartJSInterop = new CanvasHandler();