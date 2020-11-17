import { Drawable, Rectangle, Text, Point, Line, HorizontalAlignment } from "./models/drawables.js";
import { EmployeeNode, OrgChartVm } from "./models/orgChartModels.js";
import { Settings } from "./models/settings.js";

export class OrgChartService {

    constructor() {
        this.settings = new Settings();

        /** @type {EmployeeNode} */
        this.currentTopNode = null;
        /** @type {EmployeeNode[]} */
        this.currentChildNodes = [];
        /** @type {Drawable[]} */
        this.drawableItems = [];

        this.startingNodeIndex = 0;

        this.canGoLeft = false;
        this.canGoRight = false;

        this.internalNav = false;
    }

    /**
     * @param {HTMLCanvasElement} canvas
     * @param {any} clickInvoker
     */
    initialize(canvas, clickInvoker) {
        this.settings.canvas = canvas;
        this.settings.context = canvas.getContext("2d");

        this.clickInvoker = clickInvoker;

        this.settings.canvas.addEventListener("click", (evt) => {
            this.itemClicked(evt);
            evt.stopPropagation();
        });

        this.calculateMaxNodes();
    }

    /**
     * @param {OrgChartVm} viewModel
     */
    drawOrg(viewModel) {
        this.settings.context.clearRect(0, 0, this.settings.canvas.width, this.settings.canvas.height);
        this.canGoLeft = false;
        this.canGoRight = false;

        if (this.internalNav) {
            this.internalNav = false;
        } else {
            this.startingNodeIndex = 0;
        }

        this.drawableItems = [];

        let nodeDimensions1 = EmployeeNode.calculateOrgNode(this.settings, true);
        let nodeDimensions2 = EmployeeNode.calculateOrgNode(this.settings, false);

        let drawPoint = new Point();
        drawPoint.x = 0;
        drawPoint.y = this.settings.nodeOffset;

        let nDim = null;
        if (viewModel.manager) {
            nDim = (viewModel.manager.isCurrent ? nodeDimensions1 : nodeDimensions2);
            drawPoint.x = this.settings.canvas.width / 2 - nDim.width / 2;
            let managerNode = new EmployeeNode(viewModel.manager, this.settings, nDim, drawPoint);
            drawPoint.y = drawPoint.y + nDim.height + this.settings.nodeOffset;
            this.drawableItems = this.drawableItems.concat(managerNode.drawMe());
            this.drawableItems.push(managerNode.drawBottomStub(this.settings.nodeOffset, false));
        }

        nDim = null;
        nDim = (viewModel.employee.isCurrent ? nodeDimensions1 : nodeDimensions2);
        drawPoint.x = this.settings.canvas.width / 2 - nDim.width / 2;
        let mainNode = new EmployeeNode(viewModel.employee, this.settings, nDim, drawPoint);
        drawPoint.y = drawPoint.y + nDim.height + this.settings.nodeOffset;
        this.drawableItems = this.drawableItems.concat(mainNode.drawMe());
        this.drawableItems.push(mainNode.drawBottomStub(this.settings.nodeOffset / 2));

        //Mitarbeiter -> spacing berechnen
        let spacing1 = 0;
        let spacing2 = 0;
        let horLineLength = 0;

        let hasPrim = false;
        viewModel.employees.forEach(x => {
            if (x.isCurrent) { hasPrim = true; }
        });
        if (viewModel.employees.length >= this.settings.maxNodes) {
            let nodeWs1 = (this.settings.maxNodes - 1) * nodeDimensions2.width + nodeDimensions1.width;
            let nodeWs2 = this.settings.maxNodes * nodeDimensions2.width;

            spacing1 = (this.settings.canvas.width - nodeWs1) / (this.settings.maxNodes + 1);
            spacing2 = (this.settings.canvas.width - nodeWs2) / (this.settings.maxNodes + 1);

            if (hasPrim) {
                horLineLength = (this.settings.maxNodes - 1) * nodeDimensions2.width + nodeDimensions1.width + (this.settings.maxNodes - 1) * spacing1;
            } else {
                horLineLength = this.settings.maxNodes * nodeDimensions2.width + (this.settings.maxNodes - 1) * spacing2;
            }
        } else {
            nodeWs = (viewModel.employees.length - 1) * nodeDimensions2.width + nodeDimensions1.width;
            if (hasPrim) {
                nodeWs = (viewModel.employees.length - 1) * nodeDimensions2.width + nodeDimensions1.width;
            } else {
                nodeWs = viewModel.employees.length * nodeDimensions2.width;
            }
            spacing1 = (this.settings.canvas.width - nodeWs) / (viewModel.employees.length + 1);
            spacing2 = spacing1;
            if (hasPrim) {
                (viewModel.employees.length - 1) * nodeDimensions2.width + nodeDimensions1.width + (viewModel.employees.length - 1) * spacing;
            } else {
                horLineLength = viewModel.employees.length * nodeDimensions2.width + (viewModel.employees.length - 1) * spacing;
            }
        }

        let horLineSPoint = new Point();
        let horLineEPoint = new Point();
        horLineSPoint.y = drawPoint.y - (this.settings.nodeOffset / 2);
        horLineEPoint.y = horLineSPoint.y;
        horLineSPoint.x = (hasPrim ? spacing1 : spacing2);
        horLineEPoint.x = horLineSPoint.x + horLineLength;
        let horLine = new Line(horLineSPoint, horLineEPoint, this.settings.nodeBackgroundColor);
        this.drawableItems.push(horLine);

        let empCol = 0;
        let empRow = 0;
        drawPoint.x = 0;

        let prevHasPrim = false;
        for (let i = 0; i < viewModel.employees.length; i++) {
            let primInThisRow = false;
            for (let y = empRow * this.settings.maxNodes; y < (empRow + 1) * this.settings.maxNodes; y++) {
                if (viewModel.employees[y]) {
                    if (viewModel.employees[y].isCurrent) {
                        primInThisRow = true;
                        break;
                    }
                }
            }

            drawPoint.x = drawPoint.x + (primInThisRow ? spacing1 : spacing2);
            drawPoint.y = drawPoint.y + ((prevHasPrim ? nodeDimensions1 : nodeDimensions2).height + this.settings.nodeOffset) * empRow;

            let empNode = new EmployeeNode(viewModel.employees[i], this.settings, (viewModel.employees[i].isCurrent ? nodeDimensions1 : nodeDimensions2), drawPoint);
            this.drawableItems = this.drawableItems.concat(empNode.drawMe());

            drawPoint.x = drawPoint.x + (viewModel.employees[i].isCurrent ? nodeDimensions1 : nodeDimensions2).width;
            empCol++;
            if (empCol >= this.settings.maxNodes) {
                empCol = 0;
                drawPoint.x = 0;
                if (hasPrim) prevHasPrim = true;
                empRow++;
            }
        }

        this.drawableItems.forEach(i => i.drawMe(this.settings.context));
    }

    calculateMaxNodes() {
        let dnWidth = (this.settings.primaryNodeWidth + 3 * this.settings.secondaryNodeWidth) / 4;
        let nodes = (this.settings.canvas.width - this.settings.spacingMin) / (dnWidth - this.settings.spacingMin);
        this.settings.maxNodes = Math.floor(nodes) > 4 ? 4 : Math.floor(nodes);
    }

    /**
     * 
     * @param {MouseEvent} event
     */
    itemClicked(event) {
        let rect = this.settings.canvas.getBoundingClientRect();
        let x = event.clientX - rect.left;
        let y = event.clientY - rect.top;

        this.drawableItems.forEach(item => {
            if (item instanceof Rectangle) {

                if (x > item.startPoint.x && x < item.startPoint.x + item.width && y > item.startPoint.y && y < item.startPoint.y + item.height) {
                    this.clickInvoker.invokeMethodAsync('iSy', item.id);
                }
            }
        });
    }
}