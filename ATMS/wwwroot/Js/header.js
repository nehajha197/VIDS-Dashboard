window.headerClock = {
    _timerId: null,

    start: function (dotNetObject) {
        if (window.headerClock._timerId !== null) {
            clearInterval(window.headerClock._timerId);
            window.headerClock._timerId = null;
        }

        async function updateTime() {
            const now = new Date();
            const formattedTime =
                now.toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) +
                " | " +
                now.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: false });

            try {
                await dotNetObject.invokeMethodAsync('UpdateTime', formattedTime);
            } catch (err) {
                console.warn('headerClock: circuit not connected, stopping timer', err);
                window.headerClock.stop(); // don't keep hammering a dead circuit
            }
        }
            
        updateTime();
        window.headerClock._timerId = setInterval(updateTime, 1000);
    },

    stop: function () {
        if (window.headerClock._timerId !== null) {
            clearInterval(window.headerClock._timerId);
            window.headerClock._timerId = null;
        }
    }
};