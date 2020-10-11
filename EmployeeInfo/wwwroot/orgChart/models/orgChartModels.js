import { Drawable, EmployeeImage, Point, Rectangle, Text, Line, HorizontalAlignment } from "./drawables.js";
import { Settings } from "./settings.js";

export class OrgChartVm {
    constructor() {
        this.manager = new OrgChartEmployee();
        this.employee = new OrgChartEmployee();
        this.employees = [];
    }
}

export class OrgChartEmployee {
    constructor() {
        this.id = "";
        this.firstName = "";
        this.lastName = "";
        this.photo = "";
        this.jobTitle = "";
    }
}

/**
 * @readonly
 * @enum {{name: string}}
 * */
export const NodeType = Object.freeze({
    MANAGERNODE: { name: "Managernode" },
    BACKNODE: { name: "Backnode" },
    TOPNODE: { name: "Topnode" },
    CHILDNODE: { name: "Childnode" }
});

export class EmployeeNode {

    /**
     * 
     * @param {OrgChartEmployee} employee
     * @param {Settings} settings
     * @param {NodeType} type
     * @param {number} nodeCount
     */
    constructor(employee, settings, type, nodeCount) {
        /**
         * @type {Point}*/
        this.drawPoint = new Point();
        /**
         * @type {Point}*/
        this.centerTop = new Point();
        /**
         * @type {Point}*/
        this.centerBottom = new Point();

        this.employee = employee;
        this.settings = settings;
        this.type = type;
        this.nodeCount = nodeCount;

        this.calculateStartPoint(nodeCount);
    }

    /**
     * 
     * @param {number} itemCount
     * @returns {Drawable[]}
     */
    drawMe(itemCount) {
        /**@type {Drawable[]} */
        let ret = [];

        if (this.type === NodeType.CHILDNODE) this.calculateChildstartPoint(itemCount);

        //let node = new Rectangle(this.employee.id, this.settings.nodeWidth, this.settings.nodeHeight, this.drawPoint, this.settings.nodeBackgroundColor);
        let node = new Rectangle(this.employee.id, this.settings.secondaryNodeWidth, this.settings.secondaryNodeHeight, this.drawPoint, this.settings.nodeBackgroundColor);
        let img = new EmployeeImage(`${this.employee.firstName} ${this.employee.lastName}`, this.employee.photo, this.settings.imageMargin);
        node.appendChild(img);

        let text = new Text(`${this.employee.firstName} ${this.employee.lastName}`, this.settings.nameFontSize, null, 10, 0, this.settings.nodeFontColor);
        let space = node.width;// - img.width - 10;
        let textW = Text.measureText(this.settings.context, `${this.employee.firstName} ${this.employee.lastName}`, this.settings.nameFontSize);
        if (textW.width > space) {
            let text = new Text(this.employee.firstName, this.settings.nameFontSize, null, 10, -10, this.settings.nodeFontColor);
            text.horizontalAlignment = HorizontalAlignment.CENTER;
            node.appendChild(text);
            text = new Text(this.employee.lastName, this.settings.nameFontSize, null, 10, 0, this.settings.nodeFontColor);
            text.horizontalAlignment = HorizontalAlignment.CENTER;
            node.appendChild(text);
        } else {
            text.horizontalAlignment = HorizontalAlignment.LEFT;
            node.appendChild(text);
        }

        //let jtLength = Text.measureText(this.settings.context, `${this.employee.jobTitle}`, this.settings.jobTitleFontSize);
        //if(jtLength)
        let text2 = new Text(`${this.employee.jobTitle}`, this.settings.jobTitleFontSize, null, 10, 15, this.settings.nodeFontColor);
        text2.horizontalAlignment = HorizontalAlignment.LEFT;
        node.appendChild(text2);

        ret.push(node);
        return ret;
    }

    /**
     * @returns {Drawable}*/
    drawBottomStub() {
        let start = new Point();
        start.x = this.centerBottom.x;
        start.y = this.centerBottom.y
        let end = new Point();
        end.x = start.x;
        end.y = start.y + this.settings.bottomLineLength;
        let line = new Line(start, end, this.settings.nodeBackgroundColor);

        return line;
    }

    /**
     * @returns {Drawable}*/
    drawTopStub() {
        let start = new Point();
        start.x = this.centerTop.x;
        start.y = this.centerTop.y - this.settings.topLineLength
        let end = new Point();
        end.x = start.x;
        end.y = this.centerTop.y;
        let line = new Line(start, end, this.settings.nodeBackgroundColor);

        return line;
    }

    /**
     * Calculates the starting point of the node. If it is a child node, the number of childnodes which need to be drawn is required to iterate over them
     * moving the x-position of each node to the right
     * 
     * @param {number} nodeCount
     */
    calculateStartPoint(nodeCount) {
        this.drawPoint = new Point();
        switch (this.type) {
            case NodeType.MANAGERNODE:
                this.drawPoint.x = this.settings.canvas.width / 2 - this.settings.secondaryNodeWidth / 2;
                this.drawPoint.y = this.settings.backNodeTop;
                break;
            case NodeType.BACKNODE:
                this.drawPoint.x = this.settings.backNodeLeft;
                this.drawPoint.y = this.settings.backNodeTop;
                break;
            case NodeType.TOPNODE:
                this.drawPoint.x = this.settings.canvas.width / 2 - this.settings.primaryNodeWidth / 2;
                this.drawPoint.y = this.settings.offsetTop;
                break;
            case NodeType.CHILDNODE:
                this.settings.currentSpacing = (this.settings.canvas.width - (nodeCount * this.settings.secondaryNodeWidth)) / (nodeCount + 1);
                this.drawPoint.x = this.settings.currentSpacing;
                this.drawPoint.y = this.settings.offsetTop + this.settings.secondaryNodeHeight + this.settings.bottomLineLength + this.settings.topLineLength;
                break;
            default:
        }
        this.setCenterPoints();
    }

    /**
     * Setting the center points (top and botton) for the current node
     */
    setCenterPoints() {
        let w = 0;
        let y = 0;

        if (this.type == NodeType.TOPNODE) {
            w = this.settings.primaryNodeWidth;
            y = this.settings.primaryNodeHeight;
        } else {
            w = this.settings.secondaryNodeWidth;
            y = this.settings.secondaryNodeHeight;
        }

        this.centerTop.x = this.drawPoint.x + w / 2;
        this.centerTop.y = this.drawPoint.y;
        this.centerBottom.x = this.drawPoint.x + w / 2;
        this.centerBottom.y = this.drawPoint.y + y;
    }

    /**
     * 
     * Calculates the starting point for a child. The item count is required to move the x-position to the right for each child
     * 
     * @param {number} itemCount
     */
    calculateChildstartPoint(itemCount) {
        this.drawPoint.x = this.drawPoint.x + (itemCount * (this.settings.currentSpacing + this.settings.nodeWidth));
        this.setCenterPoints();
    }
}