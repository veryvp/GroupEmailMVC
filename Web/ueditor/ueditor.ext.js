//百度编译器扩展
//显示时转换Body
function ShowTransBody(body) {
    if (body != "") {
        body = body.replace(/&#60;/g, "&lt;");
        body = body.replace(/&#62;/g, "&gt;");
        return body;
    }
    return body;
}
//保存时转换Body
function SaveTransBody(body) {
    if (body != "") {
        body = body.replace(/&lt;/g, "&#60;");
        body = body.replace(/&gt;/g, "&#62;");
        return body;
    }
    return body;
}