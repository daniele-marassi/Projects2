//setInterval(ResizeBody, 200);

function ResizeBody() {
    var navbarHeight = 0;
    var footerHeight = 0;
    if (document.getElementById('navbar') != null)
        navbarHeight = document.getElementById('navbar').offsetHeight;
    if (document.getElementById('footer') != null)
        footerHeight = document.getElementById('footer').offsetHeight;
    document.getElementById('mainContainer').style.height = (window.innerHeight - navbarHeight - footerHeight - 50) + 'px';
    //document.getElementById('mainContainer').style.width = (window.innerWidth - 20) + 'px';
}

setInterval(RepositionMenu, 200);

function RepositionMenu() {
    document.getElementById('navbar').style.marginTop = window.scrollY + 'px';
    if ($(document).height() > $(window).height())
        document.getElementById('navbar').style.width = (window.innerWidth - 20) + 'px';
    else
        document.getElementById('navbar').style.width = (window.innerWidth) + 'px';
}

function MenuClick() {
    //empty function to override
}

//window.addEventListener('resize', function (event) {
//    console.log(new Date().toLocaleString());
//}, true);

