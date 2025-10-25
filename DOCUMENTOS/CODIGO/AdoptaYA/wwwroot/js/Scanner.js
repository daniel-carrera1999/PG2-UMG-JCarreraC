window._scannerHandlers = [];

window.startGlobalScanner = function (dotNetHelper) {
    const handler = function (e) {
        const activeElement = document.activeElement;
        const isUserTyping =
            activeElement &&
            (activeElement.tagName === 'INPUT' ||
             activeElement.tagName === 'TEXTAREA' ||
             activeElement.isContentEditable);

        if (isUserTyping) return;

        if (!handler.buffer) handler.buffer = '';
        if (handler.timer) clearTimeout(handler.timer);

        if (e.key === 'Enter') {
            if (handler.buffer.length > 0) {
                dotNetHelper.invokeMethodAsync('OnScannerInputAsync', handler.buffer);
                handler.buffer = '';
            }
        } else {
            handler.buffer += e.key;
        }

        handler.timer = setTimeout(() => {
            handler.buffer = '';
        }, 500);
    };

    window._scannerHandlers.push(handler);
    document.addEventListener('keydown', handler);
};

window.stopGlobalScanner = function () {
    window._scannerHandlers?.forEach(h => document.removeEventListener('keydown', h));
    window._scannerHandlers = [];
};
