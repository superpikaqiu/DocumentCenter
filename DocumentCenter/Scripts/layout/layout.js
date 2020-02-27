var global = {};

layui.use(['element', 'table', 'form'], function () {
    var element = layui.element;
    var table = layui.table;
    var form = layui.form;

    //初始化页面表单元素
    global.initPage = function () {
        $.ajax({
            url: "/user/getUserInfo",
            type: "get",
            success: function (result) {
                global.user = { id: result.UserId, name: result.UserName };
                global.cdpmOrigin = result.CdpmOrigin;

                if (result.LoginFromVTask) {
                    $("#logoutBtn").remove();
                }

                if (!result.DesignVolumeID) {
                    $("#menu-designVolume").remove();
                }
                else {
                    $("#menu-designVolume").show();
                }

                $("#userName").text(global.user.name);
            }
        });
    }

    global.init();
});

//页面入口函数
global.init = function () {
    global.initPage();
    global.refreshLeftMenu();
}


global.refreshLeftMenu = function () {
    var path = location.pathname.toLowerCase();

    var menuList = [{
        "path": "/myfile/index",
        "level": 2,
        "menu-id": "menu-personal",
        "nav-id": "nav-myfile"
    },
    {
        "path": "/fileshare/index",
        "level": 2,
        "menu-id": "menu-personal",
        "nav-id": "nav-share"
    }, {
        "path": "/designvolume/index",
        "level": 1,
        "menu-id": "menu-designVolume",
        "nav-id": null
    }];

    var menu = null;
    for (var i = 0; i < menuList.length; i++) {
        if (menuList[i].path == path) {
            menu = menuList[i];
            break;
        }
    }

    if (menu == null) {
        menu = menuList[0];
    }

    //$("#leftMenu li").removeClass("layui-nav-itemed");
    $("#" + menu["menu-id"]).addClass("layui-nav-itemed");
    if (menu.level > 1) {
        $("#leftMenu dd").removeClass("layui-this");
        $("#" + menu["nav-id"]).addClass("layui-this");
    }
}

function logout(){
    $.ajax({
        url: "/user/logout",
        type: "post",
        success: function () {
            window.location.href = "/login/index";
        }
    })
}


