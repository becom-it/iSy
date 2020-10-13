export class Settings {
	constructor() {
		/**
		 * @type {HTMLCanvasElement}*/
		this.canvas = null;
		/**
		 * @type {CanvasRenderingContext2D}*/
		this.context = null;

		this.offsetTop = 100;

		this.primaryNodeWidth = 310;
		this.primaryNodeHeight = 165;
		this.secondaryNodeWidth = 240;
		this.secondaryNodeHeight = 65;
		this.primaryImageDiameter = 50;
		this.secondaryImageDiameter = 50;
		this.nodeBackgroundColor = "#3CAADC";
		this.nodeFontColor = "#fff";

		this.spacingMin = 20;
		this.currentSpacing = 0;
		this.maxNodes = 0;

		this.nameFontSize = 12;
		this.jobTitleFontSize = 8;
		this.titleFontSize = 6;

		this.topLineLength = 40;
		this.bottomLineLength = 40;

		this.backNodeLeft = 100;
		this.backNodeTop = 50;

		this.imageMargin = 10;
    }
}