var cookie = {
    isCookie: function (_w) {
        let _t = _w;
        if (_t.localStorage) { return true; }
        else { return false; }
    },
    setCookie: function (_k, _v) {//_v为对象
        if (_k == '' || $.isEmptyObject(_v)) {
            throw Error('键值对不能为空...');
        }
        else { localStorage.setItem(_k, _v) };
    },
    getCookie: function (_k) {
        if (_k != '') {
            return localStorage.getItem(_k);
        } else {
            throw Error('键不能为空...');
        }
    },
    removeCookie: function (_k) {
        if (_k != '') {
            return localStorage.removeItem(_k);
        } else {
            throw Error('键不能为空...');
        }
    },
    clearCookie: function () {
        if (this.isCookie(window)) {
            localStorage.clear();
        }
        else {
            throw Error('不支持localStorage...');
        }
    }
}
