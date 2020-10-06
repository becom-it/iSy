import { Drawable, Rectangle, Text, Point, Line, HorizontalAlignment } from "./models/drawables.js";
import { EmployeeNode, NodeType, OrgChartVm } from "./models/orgChartModels.js";
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
		if (viewModel.manager) {
			let node = new EmployeeNode(viewModel.manager, this.settings, NodeType.BACKNODE);
			this.drawableItems = this.drawableItems.concat(node.drawMe());
		}

		this.currentTopNode = new EmployeeNode(viewModel.employee, this.settings, NodeType.TOPNODE);
		this.drawableItems = this.drawableItems.concat(this.currentTopNode.drawMe());
		this.drawableItems.push(this.currentTopNode.drawBottomStub());

		let count = 0;
		let lastIndex = this.settings.maxNodes - 1 + this.startingNodeIndex;
		if (lastIndex > viewModel.employees.length - 1) lastIndex = viewModel.employees.length - 1;

		for (let index = this.startingNodeIndex; index <= lastIndex; index++) {
			let emp = viewModel.employees[index];
			let empNode = new EmployeeNode(emp, this.settings, NodeType.CHILDNODE, (viewModel.employees.length > this.settings.maxNodes ? this.settings.maxNodes : viewModel.employees.length));
			this.drawableItems = this.drawableItems.concat(empNode.drawMe(count));
			this.drawableItems.push(empNode.drawTopStub());
			this.currentChildNodes.push(empNode);
			count++;
		}

		//Navigationsbuttons?
		//Links
		if (this.startingNodeIndex > 0) {
			this.canGoLeft = true;
			let startPoint = new Point();
			startPoint.x = this.currentChildNodes[0].centerTop.x - this.settings.currentSpacing - this.settings.topLineLength;
			startPoint.y = this.currentChildNodes[0].centerTop.y - this.settings.topLineLength - this.settings.topLineLength / 2;
			let btn = new Rectangle("left", startPoint, this.settings.topLineLength, this.settings.topLineLength, "#D3D3D3");
			let text = new Text("<", this.settings.nameFontSize, null, this.settings.bottomLineLength / 2 + this.settings.nameFontSize / 2, this.settings.nodeFontColor);
			text.horizontalAlignment = HorizontalAlignment.CENTER;
			btn.appendChild(text);
			this.drawableItems.push(btn);
		}

		//Rechts
		if (lastIndex < viewModel.employees.length - 1) {
			this.canGoRight = true;
			let startPoint = new Point();
			startPoint.x = this.currentChildNodes[this.currentChildNodes.length - 1].centerTop.x + this.settings.currentSpacing;
			startPoint.y = this.currentChildNodes[this.currentChildNodes.length - 1].centerTop.y - this.settings.topLineLength - this.settings.topLineLength / 2;
			let btn = new Rectangle("right", startPoint, this.settings.topLineLength, this.settings.topLineLength, "#D3D3D3");

			let text = new Text(">", this.settings.nameFontSize, null, this.settings.bottomLineLength / 2 + this.settings.nameFontSize / 2, this.settings.nodeFontColor)
			text.horizontalAlignment = HorizontalAlignment.CENTER;
			btn.appendChild(text);
			this.drawableItems.push(btn);
		}

		//Horizontale Linie zeichnen
		let startPoint = new Point();
		startPoint.x = this.currentChildNodes[0].centerTop.x;
		startPoint.y = this.settings.offsetTop + this.settings.nodeHeight + this.settings.bottomLineLength;
		let endPoint = new Point();
		endPoint.x = this.currentChildNodes[this.currentChildNodes.length - 1].centerTop.x;
		endPoint.y = startPoint.y;
		//Eventuelle stummel zeichnen
		if (this.canGoLeft) startPoint.x -= this.settings.currentSpacing;
		if (this.canGoRight) endPoint.x += this.settings.currentSpacing;
		this.drawableItems.push(new Line(startPoint, endPoint, this.settings.nodeBackgroundColor));

		this.drawableItems.forEach(i => i.drawMe(this.settings.context));
	}

	calculateMaxNodes() {
		let nodes = (this.settings.canvas.width - this.settings.spacingMin) / (this.settings.nodeWidth - this.settings.spacingMin);
		this.settings.maxNodes = Math.floor(nodes);
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
					if (item.id == this.currentTopNode.employee.id) return;
					this.internalNav = true;
					console.log(`Pressed on ${item.id}`);
					if (item.id == "right") {
						this.startingNodeIndex = this.startingNodeIndex + this.settings.maxNodes;
						this.drawOrganigramm(this.currentTopNode.employee);
						return;
					}
					if (item.id == "left") {
						this.startingNodeIndex = this.startingNodeIndex - this.settings.maxNodes;
						this.drawOrganigramm(this.currentTopNode.employee);
						return;
					}
					this.startingNodeIndex = 0;
					this.clickInvoker.invokeMethodAsync('iSy', item.id);
					return;
				}
			}
		});
    }
}