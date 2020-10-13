import { Drawable, EmployeeImage, Point, Rectangle, Text, Line, HorizontalAlignment, OrgNodeDimensions } from "./drawables.js";
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
        this.isCurrent = false;
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
     * @param {OrgNodeDimensions} orgNodeDimensions
     * @param {Point} startPoint
     */
    constructor(employee, settings, orgNodeDimensions, startPoint) {
        /**
         * @type {Point}*/
        this.drawPoint = { ...startPoint };
        /**
         * @type {Point}*/
        this.centerTop = new Point();
        /**
         * @type {Point}*/
        this.centerBottom = new Point();

        /**@type {OrgNodeDimensions} */
        this.nodeDimensions = orgNodeDimensions;

        this.employee = employee;
        this.settings = settings;

        this.setCenterPoints();
    }

    drawMe() {
        /**@type {Drawable[]} */
        let ret = [];

        let node = new Rectangle(this.employee.id, this.nodeDimensions.width, this.nodeDimensions.height, this.drawPoint, this.settings.nodeBackgroundColor);

        //Text schreiben -> Name
        //Ist der Name zu Breit?
        let t1Dim = Text.measureText(this.settings.context, `${this.employee.firstName} ${this.employee.lastName}`, this.settings.nameFontSize);

        //Check t3 Length
        let t3Text = `${this.employee.jobTitle}`
        let t3ShortText = t3Text;
        let t3Dim = Text.measureText(this.settings.context, t3Text, this.settings.jobTitleFontSize);
        if (t3Dim.width > this.nodeDimensions.textMaxWidth) {
            t3ShortText = t3Text + "...";
            t3Dim = Text.measureText(this.settings.context, t3ShortText, this.settings.jobTitleFontSize);
            while (t3Dim.width > this.nodeDimensions.textMaxWidth) {
                t3Text = t3Text.substr(0, t3Text.length - 2)
                t3ShortText = t3Text + "...";
                t3Dim = Text.measureText(this.settings.context, t3ShortText, this.settings.jobTitleFontSize);
            }
        }

        if (t1Dim.width > this.nodeDimensions.textMaxWidth) {
            //Text zu Breit
            //Zuerst t3 -> ganz unten

            let t3Point = new Point();
            t3Point.x = this.drawPoint.x + this.nodeDimensions.padding;
            t3Point.y = this.drawPoint.y - this.nodeDimensions.padding + this.nodeDimensions.height - (t3Dim.actualBoundingBoxAscent - t3Dim.actualBoundingBoxDescent)
            let t3 = new Text(t3ShortText, this.settings.jobTitleFontSize, t3Point, this.settings.nodeFontColor);
            node.appendChild(t3);

            let t2Point = new Point();
            t2Point.x = this.drawPoint.x + this.nodeDimensions.padding;
            t2Point.y = t3Point.y - (t1Dim.actualBoundingBoxAscent - t1Dim.actualBoundingBoxDescent) - this.settings.twoLineNameVerticalSpacing;
            let t2 = new Text(`${this.employee.lastName}`, this.settings.nameFontSize, t2Point, this.settings.nodeFontColor);
            node.appendChild(t2);


            let t1Point = new Point();
            t1Point.x = this.drawPoint.x + this.nodeDimensions.padding;
            t1Point.y = t2Point.y - (t1Dim.actualBoundingBoxAscent - t1Dim.actualBoundingBoxDescent) - this.settings.twoLineNameVerticalSpacing;
            let t1 = new Text(`${this.employee.firstName}`, this.settings.nameFontSize, t1Point, this.settings.nodeFontColor);
            node.appendChild(t1);
        } else {
            let t2Point = new Point();
            t2Point.x = this.drawPoint.x + this.nodeDimensions.padding;
            t2Point.y = this.drawPoint.y + this.nodeDimensions.padding + (2 / 3 * this.nodeDimensions.textMaxHeigth) - (t1Dim.actualBoundingBoxAscent - t1Dim.actualBoundingBoxDescent);
            let t2 = new Text(`${this.employee.firstName} ${this.employee.lastName}`, this.settings.nameFontSize, t2Point, this.settings.nodeFontColor);
            node.appendChild(t2);

            let t3Point = new Point();
            t3Point.x = this.drawPoint.x + this.nodeDimensions.padding;
            t3Point.y = this.drawPoint.y + this.nodeDimensions.padding + (2 / 3 * this.nodeDimensions.textMaxHeigth) + this.settings.twoLineNameVerticalSpacing;
            let t3 = new Text(t3ShortText, this.settings.jobTitleFontSize, t3Point, this.settings.nodeFontColor);
            node.appendChild(t3);
        }

        let img = new EmployeeImage(`${this.employee.firstName} ${this.employee.lastName}`, this.employee.photo, this.nodeDimensions);
        node.appendChild(img);

        ret.push(node);
        return ret;
    }

    /**
     * 
     * @param {number} length
     * @returns {Drawable}
     */
    drawBottomStub(length) {
        let start = new Point();
        start.x = this.centerBottom.x;
        start.y = this.centerBottom.y
        let end = new Point();
        end.x = start.x;
        end.y = start.y + length;
        let line = new Line(start, end, this.settings.nodeBackgroundColor);

        return line;
    }

    /**
     * @param {boolean} primary
     * @param {boolean} primary
     * @returns {OrgNodeDimensions}*/
    static calculateOrgNode(settings, primary) {
        let orgNodeDimensions = new OrgNodeDimensions();

        if (primary) {
            orgNodeDimensions.width = settings.primaryNodeWidth;
            orgNodeDimensions.height = settings.primaryNodeHeight;
            orgNodeDimensions.imageDiameter = settings.primaryImageDiameter;
            orgNodeDimensions.padding = orgNodeDimensions.imageDiameter / 2;
        } else {
            orgNodeDimensions.width = settings.secondaryNodeWidth;
            orgNodeDimensions.height = settings.secondaryNodeHeight;
            orgNodeDimensions.imageDiameter = settings.secondaryImageDiameter;
            orgNodeDimensions.padding = (orgNodeDimensions.height - orgNodeDimensions.imageDiameter) / 2;
        }

        orgNodeDimensions.imageCenter = new Point();
        orgNodeDimensions.imageCenter.x = orgNodeDimensions.width - (orgNodeDimensions.padding + orgNodeDimensions.imageDiameter / 2);
        orgNodeDimensions.imageCenter.y = orgNodeDimensions.height / 2;

        orgNodeDimensions.textMaxWidth = orgNodeDimensions.width - (3 * orgNodeDimensions.padding + orgNodeDimensions.imageDiameter);
        orgNodeDimensions.textMaxHeigth = orgNodeDimensions.height - 2 * orgNodeDimensions.padding

        return orgNodeDimensions;
    }

    /**
     * Setting the center points (top and botton) for the current node
     */
    setCenterPoints() {
        this.centerTop.x = this.drawPoint.x + this.nodeDimensions.width / 2;
        this.centerTop.y = this.drawPoint.y;
        this.centerBottom.x = this.drawPoint.x + this.nodeDimensions.width / 2;
        this.centerBottom.y = this.drawPoint.y + this.nodeDimensions.height;
    }
}