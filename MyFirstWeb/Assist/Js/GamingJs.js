function Playing(list) {

    var yourModel = list;
    var member = JSON.parse(yourModel);
    alert(member[0].BodyList.length);
    //alert(getRandom(member[0].BodyList.length));
    $('#labA').text(member[0].PlayerID + " Please " + member[0].BodyList[getRandom(member[0].BodyList.length) - 1] + " touch " + member[0].ColorList[getRandom(member[0].ColorList.length) - 1])
    $('#labB').text((member[1].PlayerID + " Please " + member[1].BodyList[getRandom(member[1].BodyList.length) - 1] + " touch " + member[1].ColorList[getRandom(member[1].ColorList.length) - 1]))

        //alert(yourModel);
}

function getRandom(x) {
    return Math.floor(Math.random() * x) + 1;
};

