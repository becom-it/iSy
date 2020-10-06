export class Settings {
	constructor() {
		/**
		 * @type {HTMLCanvasElement}*/
		this.canvas = null;
		/**
		 * @type {CanvasRenderingContext2D}*/
		this.context = null;

		this.offsetTop = 100;

		this.nodeWidth = 200;
		this.nodeHeight = 100;
		this.nodeBackgroundColor = "#3CAADC";
		this.nodeFontColor = "#fff";

		this.spacingMin = 20;
		this.currentSpacing = 0;
		this.maxNodes = 0;

		this.nameFontSize = 10;
		this.titleFontSize = 6;

		this.topLineLength = 40;
		this.bottomLineLength = 40;

		this.backNodeLeft = 100;
		this.backNodeTop = 50;

		this.imageMargin = 10;
    }
}