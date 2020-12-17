//document.addEventListener("DOMContentLoaded", () => {
//    let mainContainer = document.querySelector(".lc-main");
//    if (mainContainer) {
//        let unam = document.getElementById("login-uname");
//        let ps = document.getElementById("login-pwd");
//        unam.addEventListener("keypress", (e) => {
//            console.log(e);
//        });

//    }
//});

export function callLogin(uname, pw) {

    let target = window.location.pathname;

    window.location.href = `/login?paramUsername=${uname}&paramPassword=${pw}&target=${target}`;
}