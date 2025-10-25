window.auth = window.auth || {};

// Funciones existentes
window.auth.setTempSession = function (token, userJson) {
    document.cookie = `temp_token=${token}; path=/`;
    document.cookie = `temp_user=${encodeURIComponent(userJson)}; path=/`;
};

window.auth.setTempRefreshToken = function (token, exp) {
    document.cookie = `temp_refresh_token=${token}; path=/`;
    document.cookie = `temp_exp=${exp}; path=/`;
};

// Fragmentar y guardar el JSON en varias cookies temp_user_0, temp_user_1, ... n
window.auth.setTempUserFragmented = function (userJson, fragmentSize) {
    fragmentSize = fragmentSize || 2000; // length de la cookie (2000 chars es seguro)
    // Borra cookies viejas, por si acaso
    let i = 0;
    while (true) {
        let cname = "temp_user_" + i;
        if (!document.cookie.split('; ').find(row => row.startsWith(cname + '='))) break;
        document.cookie = cname + "=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT";
        i++;
    }
    // Fragmenta y guarda
    for (let i = 0, len = userJson.length; i * fragmentSize < len; i++) {
        let fragment = userJson.substring(i * fragmentSize, (i + 1) * fragmentSize);
        document.cookie = "temp_user_" + i + "=" + encodeURIComponent(fragment) + "; path=/";
    }
};

// NUEVA función para renovar la cookie de sesión sin recargar la página
window.auth.refreshSession = async function(payloadJson) {
    await fetch('/auth/refresh-session', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: payloadJson
    });
};
