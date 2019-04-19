function BaseList(msgNum, messageMenu, data) {
    this.msgNum = msgNum;
    this.messageMenu = messageMenu;
    this.data = data;
}

BaseList.prototype.initEvent = function ($ent) { throw new Error("Method initEvent not implemented"); }
BaseList.prototype.initElements = function () { throw new Error("Method initElements not implemented"); }
BaseList.prototype.createMsgList = function () { throw new Error("Method createMsgList not implemented"); }