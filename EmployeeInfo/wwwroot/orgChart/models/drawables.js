

/**
 * @readonly
 * @enum {{name: string}}
 * */
export const HorizontalAlignment = Object.freeze({
    UNDEFINED: { name: "Undefined" },
    LEFT: { name: "Left" },
    CENTER: { name: "Center" },
    RIGHT: { name: "Right" }
});

export class Point {
    constructor() {
        this.x = 0;
        this.y = 0;
    }
}

export class OrgNodeDimensions {
    constructor() {
        this.width = 0;
        this.height = 0;
        this.padding = 0;
        this.imageDiameter = 0;
        /**
         * @type {Point}*/
        this.imageCenter = null;
        this.textMaxWidth = 0;
        this.textMaxHeigth = 0;
    }
}

export class Drawable {
    constructor() {
        /**
         * @type {CanvasRenderingContext2D}
         * @public
         * */
        this.context = null;
        /**
         * @type {Drawable[]}
         * @public
         * */
        this.children = [];

        /**
         * @type {Drawable}
         * @public
         * */
        this.parent = null;
    }

    initMe() {

    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context
     */
    drawMe(context) {
        this.context = context;
    }

    /**
     * 
     * @param {Drawable} child
     */
    appendChild(child) {
        child.parent = this;
        child.initMe();
        this.children.push(child);
    }

    drawChilds() {
        if (this.children.length > 0) {
            this.children.forEach(x => x.drawMe(this.context));
        }
    }

    /**
     * @returns {Drawable}
     * */
    getLastChild() {
        let ret = null;
        if (this.children.length > 0) {
            ret = this.children[this.children.length - 1];
        }
        return null;
    }
}

export class Rectangle extends Drawable {

    /**
     * 
     * @param {string} id
     * @param {number} width
     * @param {number} height
     * @param {Point} startPoint
     * @param {?string} backgroundColor
     */
    constructor(id, width, height, startPoint, backgroundColor) {
        super();
        this.id = id;
        this.width = width;
        this.height = height;
        this.backgroundColor = backgroundColor;

        /**
         * @type {Point}
         * */
        this.startPoint = startPoint;
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context
     */
    drawMe(context) {
        super.drawMe(context);
        this.context.save();

        if (this.backgroundColor != "") {
            this.context.fillStyle = this.backgroundColor;
        }
        this.context.fillRect(this.startPoint.x, this.startPoint.y, this.width, this.height);

        this.context.restore();
        super.drawChilds();
    }

    /**
     * Get the top center of the rectangle
     *
     * @returns {Point}
     */
    getCenterTop() {
        let point = new Point();
        point.x = this.startPoint.x + this.width / 2;
        point.y = this.startPoint.y;
        return point
    }

    /**
     * Get the botton center of the rectangle
     * 
     * @returns {Point}
     */
    getCenterBottom() {
        let point = new Point();
        point.x = this.startPoint.x + this.width / 2;
        point.y = this.startPoint.y + this.height;
        return point
    }

    /**
     * Get the center of the rectangle
     * 
     * @returns {Point}
     */
    getCenter() {
        let point = new Point();
        point.x = this.startPoint.x + this.width / 2;
        point.y = this.startPoint.y + this.height / 2;
        return point
    }

}

export class EmployeeImage extends Drawable {
    /**
     * 
     * @param {string} id
     * @param {string} base64Img
     * @param {OrgNodeDimensions} orgNodeDimensions
     */
    constructor(id, base64Img, orgNodeDimensions) {
        super();
        this.id = id;
        this.base64Img = base64Img;
        this.orgNodeDimensions = orgNodeDimensions;
    }

    initMe() {
        super.initMe();



        //let ratio = this.image.width / this.image.height;
        let ratio = 1.465648854961832;

        this.startPoint = new Point();
        this.startPoint.x = this.parent.startPoint.x + this.orgNodeDimensions.imageCenter.x;
        this.startPoint.y = this.parent.startPoint.y + this.orgNodeDimensions.imageCenter.y;
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context
     */
    drawMe(context) {
        super.drawMe(context);
        this.context.save();

        this.image = new Image();
        this.image.src = "data:image/png;base64," + this.base64Img;

        this.image.onload = (ev) => {
            this.context.save();
            this.context.beginPath();
            this.context.arc(this.startPoint.x, this.startPoint.y, this.orgNodeDimensions.imageDiameter / 2, 0, Math.PI * 2, true);
            this.context.fillStyle = "white";
            this.context.fill();
            this.context.closePath();
            this.context.clip();

            let hRatio = this.orgNodeDimensions.imageDiameter / this.image.width;
            let vRatio = this.orgNodeDimensions.imageDiameter / this.image.height;
            let ratio = Math.min(hRatio, vRatio);

            let imgW = this.image.width * ratio;
            let imgH = this.image.height * ratio;

            this.context.drawImage(this.image
                , this.startPoint.x - (imgW / 2)
                , this.startPoint.y - (imgH / 2)
                , imgW, imgH);
                //, this.orgNodeDimensions.imageDiameter, this.orgNodeDimensions.imageDiameter);

            //this.context.drawImage(this.image
            //    , this.startPoint.x - this.orgNodeDimensions.imageDiameter / 2
            //        , this.startPoint.y - this.orgNodeDimensions.imageDiameter / 2
            //        , this.orgNodeDimensions.imageDiameter, this.orgNodeDimensions.imageDiameter);

            this.context.beginPath();
            //this.context.arc(this.parent.startPoint.x + this.parent.width - 55, this.parent.startPoint.y + 8, 25, 0, Math.PI * 2, true);
            this.context.arc(this.startPoint.x - this.orgNodeDimensions.imageDiameter / 2, this.startPoint.y - this.orgNodeDimensions.imageDiameter / 2, this.orgNodeDimensions.imageDiameter / 2, 0, Math.PI * 2, true);
            this.context.clip();
            this.context.closePath();
            this.context.restore();
        };

        this.context.restore();
        super.drawChilds();
    }
}

export class EmployeeImage2 extends Drawable {

    /**
     * 
     * @param {string} id
     * @param {string} base64Img
     * @param {number} margin
     */
    constructor(id, base64Img, margin) {
        super();
        this.id = id;
        this.base64Img = base64Img;
        this.margin = margin;
    }

    initMe() {
        super.initMe();



        //let ratio = this.image.width / this.image.height;
        let ratio = 1.465648854961832;

        this.startPoint = new Point();
        this.startPoint.x = this.parent.startPoint.x + this.margin;
        this.startPoint.y = this.parent.startPoint.y + this.margin;

        this.height = this.parent.height - 2 * this.margin;
        this.width = this.height / ratio;
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context
     */
    drawMe(context) {
        super.drawMe(context);
        this.context.save();

        this.image = new Image();
        this.image.src = "data:image/png;base64," + this.base64Img;

        this.image.onload = (ev) => {
            this.context.save();
            this.context.beginPath();
            this.context.arc(this.parent.startPoint.x + this.parent.width - 30, this.parent.startPoint.y + 33, 25, 0, Math.PI * 2, true);
            this.context.closePath();
            this.context.clip();

            this.context.drawImage(this.image, this.parent.startPoint.x + this.parent.width - 55, this.parent.startPoint.y + 8, 50, 50);

            //this.context.drawImage(this.image, this.startPoint.x,
            //    this.startPoint.y,
            //    this.width,
            //    this.height);

            this.context.beginPath();
            this.context.arc(this.parent.startPoint.x + this.parent.width - 55, this.parent.startPoint.y + 8, 25, 0, Math.PI * 2, true);
            this.context.clip();
            this.context.closePath();
            this.context.restore();
        };

        this.context.restore();
        super.drawChilds();
    }
}

export class Text extends Drawable {
    /**
     * 
     * @param {string} text
     * @param {number} fontSize
     * @param {?Point} startPoint
     * @param {?string} fontColor
     */
    constructor(text, fontSize, startPoint, fontColor) {
        super();
        this.text = text;
        this.fontSize = fontSize;
        this.startPoint = startPoint;
        this.fontColor = fontColor;
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context
     */
    drawMe(context) {
        super.drawMe(context);
        this.context.save();

        this.context.font = this.fontSize + "pt sans-serif";
        if (this.fontColor !== "") this.context.fillStyle = this.fontColor;

        this.context.fillText(this.text, this.startPoint.x, this.startPoint.y);

        this.context.restore();
        super.drawChilds();
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context
     * @param {string} text
     * @param {number} fontSize
     * @returns {TextMetrics}
     */
    static measureText(context, text, fontSize) {
        context.font = fontSize + "pt sans-serif";
        return context.measureText(text);
    }
}

export class Text2 extends Drawable {
    /**
     * 
     * @param {string} text
     * @param {number} fontSize
     * @param {?Point} startPoint
     * @param {?number} xOffset
     * @param {?number} yOffset
     * @param {?string} fontColor
     */
    constructor(text, fontSize, startPoint, xOffset, yOffset, fontColor) {
        super();
        this.text = text;
        this.fontSize = fontSize;
        this.startPoint = startPoint;
        this.xOffset = xOffset;
        this.yOffset = yOffset;
        this.fontColor = fontColor;
        this.horizontalAlignment = HorizontalAlignment.UNDEFINED;
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context
     */
    drawMe(context) {
        super.drawMe(context);
        this.context.save();

        this.context.font = this.fontSize + "pt sans-serif";
        if (this.fontColor !== "") this.context.fillStyle = this.fontColor;

        if (this.horizontalAlignment !== HorizontalAlignment.UNDEFINED && this.parent !== null) {
            let tWidth = Text.measureText(context, this.text, this.fontSize);
            switch (this.horizontalAlignment) {
                case HorizontalAlignment.LEFT:
                    if (this.parent instanceof Rectangle) {
                        this.startPoint = new Point();
                        this.startPoint.y = this.parent.getCenter().y + this.yOffset;
                        this.startPoint.x = this.parent.startPoint.x + this.xOffset;
                    }
                    break;
                case HorizontalAlignment.CENTER:
                    if (this.parent instanceof Rectangle) {
                        this.startPoint = new Point();
                        this.startPoint.y = this.parent.getCenterTop().y + this.yOffset;

                        let hasImg = false;
                        let img = null;
                        this.parent.children.forEach(x => {
                            if (x instanceof EmployeeImage) {
                                hasImg = true;
                                img = x;
                            }
                        });

                        if (hasImg && img !== null) {
                            let imgW = img.width + img.margin;
                            let space = this.parent.width - imgW;
                            let right = space / 2 + tWidth.width / 2;

                            this.startPoint.x = this.parent.startPoint.x + this.parent.width - right;
                        } else {
                            this.startPoint.x = this.parent.getCenterTop().x - tWidth.width / 2;
                        }
                    }
                    break;
                case HorizontalAlignment.RIGHT:
                    break;
                default:
            }
        }

        this.context.fillText(this.text, this.startPoint.x, this.startPoint.y);

        this.context.restore();
        super.drawChilds();
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context
     * @param {string} text
     * @param {number} fontSize
     * @returns {TextMetrics}
     */
    static measureText(context, text, fontSize) {
        context.font = fontSize + "pt sans-serif";
        return context.measureText(text);
    }
}

export class Line extends Drawable {

    /**
     * 
     * @param {Point} startPoint
     * @param {Point} endPoint
     * @param {?string} color
     */
    constructor(startPoint, endPoint, color) {
        super();
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.color = color;
    }

    /**
     * 
     * @param {CanvasRenderingContext2D} context
     */
    drawMe(context) {
        super.drawMe(context);
        this.context.save();

        if (this.color !== "") this.context.strokeStyle = this.color;
        this.context.beginPath();
        this.context.moveTo(this.startPoint.x, this.startPoint.y);
        this.context.lineTo(this.endPoint.x, this.endPoint.y);
        this.context.stroke();

        this.context.restore();
        super.drawChilds();
    }
}