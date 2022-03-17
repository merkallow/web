﻿// console.log('executing js');
////import makeBlockie from 'blockies';



window.switchChain = async function(chainId) {
    console.log('hz');
        await window.ethereum.request({
        method: 'wallet_switchEthereumChain',
        params: [{ chainId: chainId }],
    });
}


function showAlert(message) {
    alert(message);
}

function selectElement(id) {
    var el = document.getElementById(id);
    el.select();
}

window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
            console.log("Copied to clipboard!");
        })
            .catch(function (error) {
                alert(error);
            });
    }
};

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function eraseCookie(name) {
    document.cookie = name + '=; Max-Age=-99999999;';
}

// function setBlockie() {
//     console.log("setting blockie");
//     const img = new Image();
//     img.src = makeBlockie('0x7cB57B5A97eAbe94205C07890BE4c1aD31E486A8');

//     //document.body.appendChild(img);
// }