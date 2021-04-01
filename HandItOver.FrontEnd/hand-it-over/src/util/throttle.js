export function throttle(callback, limit) {
    let wait = false;
    let lastUnexecutedArg = null;
    return function (arg) {
        if (!wait) {
            callback.call(this, arg);
            wait = true;
            setTimeout(function () {
                if (lastUnexecutedArg) {
                    callback.call(this, lastUnexecutedArg);
                    lastUnexecutedArg = null;
                }
                wait = false;
            }, limit);
        } else {
            lastUnexecutedArg = arg;
        }
    };
}